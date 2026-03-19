#!/usr/bin/env python3
import argparse
import json
import subprocess
import sys
import tempfile
import time
from pathlib import Path

from PIL import Image, ImageChops, ImageStat


SCRIPT_DIR = Path(__file__).resolve().parent
RASTERIZER_SCRIPT = SCRIPT_DIR / "pdf_rasterize.swift"
DEFAULT_LAYOUT_SPEC = SCRIPT_DIR.parent.parent / "pdf-samples" / "windo-quotation-layout-spec.json"
IMAGE_EXTENSIONS = {".png", ".jpg", ".jpeg"}


def rasterize_all_pages(pdf_path: Path, output_dir: Path) -> list[Path]:
    cmd = [
        "env",
        "CLANG_MODULE_CACHE_PATH=/tmp/clang-module-cache",
        "swift",
        str(RASTERIZER_SCRIPT),
        str(pdf_path),
        str(output_dir),
    ]
    last_error = None
    for _ in range(3):
        try:
            subprocess.run(cmd, check=True, capture_output=True, text=True)
            break
        except subprocess.CalledProcessError as error:
            last_error = error
            time.sleep(0.25)
    else:
        raise last_error
    return sorted(output_dir.glob("page-*.png"))


def resolve_reference_pages(input_path: Path, output_dir: Path) -> list[Path]:
    if input_path.suffix.lower() in IMAGE_EXTENSIONS:
        output_dir.mkdir(parents=True, exist_ok=True)
        target = output_dir / f"page-001{input_path.suffix.lower()}"
        target.write_bytes(input_path.read_bytes())
        return [target]

    return rasterize_all_pages(input_path, output_dir)


def compare_images(reference_path: Path, candidate_path: Path, diff_path: Path) -> dict:
    reference = Image.open(reference_path).convert("RGBA")
    candidate = Image.open(candidate_path).convert("RGBA")

    if reference.size != candidate.size:
        candidate = candidate.resize(reference.size)

    diff = ImageChops.difference(reference, candidate)
    bbox = diff.getbbox()
    diff.save(diff_path)

    stat = ImageStat.Stat(diff)
    mean = stat.mean
    rms = stat.rms

    differing_pixels = 0
    if bbox is not None:
        differing_pixels = sum(
            1
            for rgba in diff.getdata()
            if rgba[0] > 0 or rgba[1] > 0 or rgba[2] > 0 or rgba[3] > 0
        )

    total_pixels = reference.size[0] * reference.size[1]
    return {
        "reference": str(reference_path),
        "candidate": str(candidate_path),
        "diff": str(diff_path),
        "size": {"width": reference.size[0], "height": reference.size[1]},
        "mean_rgba": mean,
        "rms_rgba": rms,
        "differing_pixels": differing_pixels,
        "total_pixels": total_pixels,
        "difference_ratio": 0 if total_pixels == 0 else differing_pixels / total_pixels,
        "bounding_box": bbox,
    }


def load_layout_spec(spec_path: Path) -> dict:
    if not spec_path.exists():
        return {"anchors": []}
    return json.loads(spec_path.read_text(encoding="utf-8"))


def resolve_page_type(page_index: int, page_count: int) -> str:
    if page_index == 0:
        return "first"
    if page_index == page_count - 1:
        return "last"
    return "middle"


def compare_block(reference_path: Path, candidate_path: Path, block: dict) -> dict:
    reference = Image.open(reference_path).convert("RGBA")
    candidate = Image.open(candidate_path).convert("RGBA")

    if reference.size != candidate.size:
        candidate = candidate.resize(reference.size)

    rect = block["rect"]
    width, height = reference.size
    crop_box = (
        int(rect["x"] * width),
        int(rect["y"] * height),
        int((rect["x"] + rect["width"]) * width),
        int((rect["y"] + rect["height"]) * height),
    )

    reference_crop = reference.crop(crop_box)
    candidate_crop = candidate.crop(crop_box)
    diff = ImageChops.difference(reference_crop, candidate_crop)
    differing_pixels = sum(
        1
        for rgba in diff.getdata()
        if rgba[0] > 0 or rgba[1] > 0 or rgba[2] > 0 or rgba[3] > 0
    )
    total_pixels = reference_crop.size[0] * reference_crop.size[1]

    return {
        "id": block["id"],
        "label": block["label"],
        "rect": crop_box,
        "difference_ratio": 0 if total_pixels == 0 else differing_pixels / total_pixels,
        "differing_pixels": differing_pixels,
        "total_pixels": total_pixels,
    }


def main() -> int:
    parser = argparse.ArgumentParser(description="Compare a reference PDF/image and a candidate PDF page-by-page.")
    parser.add_argument("reference_pdf")
    parser.add_argument("candidate_pdf")
    parser.add_argument("--output-dir", default=None)
    parser.add_argument("--skip-reference-pages", type=int, default=0)
    parser.add_argument("--skip-candidate-pages", type=int, default=0)
    parser.add_argument("--layout-spec", default=str(DEFAULT_LAYOUT_SPEC))
    args = parser.parse_args()

    layout_spec = load_layout_spec(Path(args.layout_spec).expanduser().resolve())
    reference_pdf = Path(args.reference_pdf).expanduser().resolve()
    candidate_pdf = Path(args.candidate_pdf).expanduser().resolve()
    output_dir = Path(args.output_dir).expanduser().resolve() if args.output_dir else Path(tempfile.mkdtemp(prefix="pdf-visual-diff-"))
    output_dir.mkdir(parents=True, exist_ok=True)

    reference_raster_dir = output_dir / "reference-pages"
    candidate_raster_dir = output_dir / "candidate-pages"
    reference_pngs = resolve_reference_pages(reference_pdf, reference_raster_dir)[args.skip_reference_pages :]
    candidate_pngs = rasterize_all_pages(candidate_pdf, candidate_raster_dir)[args.skip_candidate_pages :]

    page_count = max(len(reference_pngs), len(candidate_pngs))
    per_page = []
    for index in range(page_count):
        reference_png = reference_pngs[index] if index < len(reference_pngs) else None
        candidate_png = candidate_pngs[index] if index < len(candidate_pngs) else None
        diff_png = output_dir / f"diff-page-{index + 1:03d}.png"

        if reference_png is None or candidate_png is None:
            per_page.append(
                {
                    "page": index + 1,
                    "reference": str(reference_png) if reference_png else None,
                    "candidate": str(candidate_png) if candidate_png else None,
                    "diff": str(diff_png),
                    "missing": "reference" if reference_png is None else "candidate",
                    "difference_ratio": 1.0,
                }
            )
            continue

        page_report = compare_images(reference_png, candidate_png, diff_png)
        page_report["page"] = index + 1
        page_type = resolve_page_type(index, page_count)
        page_report["page_type"] = page_type
        page_report["blocks"] = [
            compare_block(reference_png, candidate_png, block)
            for block in layout_spec.get("anchors", [])
            if page_type in block.get("pageTypes", [])
        ]
        per_page.append(page_report)

    average_ratio = sum(page["difference_ratio"] for page in per_page) / len(per_page) if per_page else 0
    max_ratio = max((page["difference_ratio"] for page in per_page), default=0)
    block_summary: dict[str, list[float]] = {}
    for page in per_page:
        for block in page.get("blocks", []):
            block_summary.setdefault(block["id"], []).append(block["difference_ratio"])
    report = {
        "reference_pdf": str(reference_pdf),
        "candidate_pdf": str(candidate_pdf),
        "output_dir": str(output_dir),
        "reference_page_count": len(reference_pngs),
        "candidate_page_count": len(candidate_pngs),
        "compared_page_count": len(per_page),
        "average_difference_ratio": average_ratio,
        "max_difference_ratio": max_ratio,
        "blocks": {
            block_id: {
                "average_difference_ratio": sum(values) / len(values),
                "max_difference_ratio": max(values),
                "samples": len(values),
            }
            for block_id, values in block_summary.items()
        },
        "pages": per_page,
    }

    report_path = output_dir / "report.json"
    report_path.write_text(json.dumps(report, indent=2), encoding="utf-8")
    print(json.dumps(report, indent=2))
    return 0


if __name__ == "__main__":
    sys.exit(main())
