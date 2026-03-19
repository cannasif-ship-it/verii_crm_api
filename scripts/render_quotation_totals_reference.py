#!/usr/bin/env python3
import argparse
import json
from pathlib import Path
from typing import Optional

from PIL import Image, ImageDraw, ImageFont


DEFAULT_PAGE_WIDTH = 794
DEFAULT_PAGE_HEIGHT = 1123
DEFAULT_SPEC_PATH = Path(__file__).resolve().parents[2] / "pdf-samples" / "quotation-totals-layout-spec.json"


def load_font(size: int, bold: bool = False):
    candidates = [
        "/System/Library/Fonts/Supplemental/Arial Bold.ttf" if bold else "/System/Library/Fonts/Supplemental/Arial.ttf",
        "/System/Library/Fonts/Supplemental/Helvetica.ttc",
    ]
    for candidate in candidates:
        path = Path(candidate)
        if path.exists():
            try:
                return ImageFont.truetype(str(path), size)
            except OSError:
                continue
    return ImageFont.load_default()


def format_currency(value: float, currency_code: Optional[str]) -> str:
    formatted = f"{value:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
    return f"{formatted} {currency_code}".strip() if currency_code else formatted


def build_rows(sample: dict) -> list[dict]:
    options = sample.get("options", {})
    values = sample.get("values", {})
    currency_mode = options.get("currencyMode", "none")
    currency_code = values.get("currency") if currency_mode == "code" else None

    rows: list[dict] = []
    if options.get("showGross", True):
        rows.append({"label": options.get("grossLabel", "Brut Toplam"), "value": format_currency(values.get("gross", 0), currency_code)})
    if options.get("showDiscount", True):
        rows.append({"label": options.get("discountLabel", "Iskonto"), "value": format_currency(values.get("discount", 0), currency_code)})
    rows.append({"label": options.get("netLabel", "Net Toplam"), "value": format_currency(values.get("net", 0), currency_code)})
    if options.get("showVat", True):
        rows.append({"label": options.get("vatLabel", "KDV"), "value": format_currency(values.get("vat", 0), currency_code)})
    rows.append(
        {
            "label": options.get("grandLabel", "Genel Toplam"),
            "value": format_currency(values.get("grand", 0), currency_code),
            "emphasize": options.get("emphasizeGrandTotal", True),
        }
    )
    return rows


def draw_text(draw: ImageDraw.ImageDraw, xy: tuple[int, int], text: str, font, fill: str, anchor: Optional[str] = None):
    draw.text(xy, text, font=font, fill=fill, anchor=anchor)


def load_layout_spec(path: Path) -> dict:
    if not path.exists():
        return {}
    return json.loads(path.read_text(encoding="utf-8")).get("quotationTotals", {})


def main() -> int:
    parser = argparse.ArgumentParser(description="Render a preview-like PNG reference for quotation totals regression.")
    parser.add_argument("sample_json")
    parser.add_argument("output_png")
    parser.add_argument("--layout-spec", default=str(DEFAULT_SPEC_PATH))
    args = parser.parse_args()

    sample = json.loads(Path(args.sample_json).read_text(encoding="utf-8"))
    layout = load_layout_spec(Path(args.layout_spec).expanduser().resolve())
    output_path = Path(args.output_png).expanduser().resolve()
    output_path.parent.mkdir(parents=True, exist_ok=True)

    page = sample.get("page", {})
    block = sample.get("block", {})
    options = sample.get("options", {})
    note = sample.get("note", {})

    page_width = int(page.get("width", DEFAULT_PAGE_WIDTH))
    page_height = int(page.get("height", DEFAULT_PAGE_HEIGHT))

    image = Image.new("RGBA", (page_width, page_height), (255, 255, 255, 255))
    draw = ImageDraw.Draw(image)

    x = int(block.get("x", 460))
    y = int(block.get("y", 820))
    width = int(block.get("width", 260))
    height = int(block.get("height", 220))

    title_font = load_font(int(layout.get("titleFontSize", 13)), bold=True)
    label_font = load_font(int(layout.get("rowLabelFontSize", 11)), bold=False)
    value_font = load_font(int(layout.get("rowValueFontSize", 11)), bold=True)
    note_title_font = load_font(int(layout.get("noteTitleFontSize", 10)), bold=True)
    note_body_font = load_font(int(layout.get("noteTextFontSize", 10)), bold=False)

    draw.rounded_rectangle((x, y, x + width, y + height), radius=12, fill="#ffffff", outline="#cbd5e1", width=1)
    outer_padding_x = int(layout.get("outerPaddingX", 14))
    outer_padding_top = int(layout.get("outerPaddingTop", 12))
    row_gap = int(layout.get("rowGap", 6))
    column_gap = int(layout.get("columnGap", 8))
    row_height = int(layout.get("rowHeight", 26))
    row_pad_x = int(layout.get("rowPaddingX", 10))
    note_top_gap = int(layout.get("noteTopGap", 10))
    note_padding_x = int(layout.get("notePaddingX", 10))
    note_padding_top = int(layout.get("notePaddingTop", 10))
    note_padding_bottom = int(layout.get("notePaddingBottom", 10))
    note_text_spacing = int(layout.get("noteTextSpacing", 4))

    draw_text(draw, (x + outer_padding_x, y + outer_padding_top), sample.get("title", "Teklif Toplamlari"), title_font, layout.get("titleColor", "#64748b"))

    rows = build_rows(sample)
    row_layout = options.get("layout", "single")
    row_width = width - (outer_padding_x * 2)
    start_y = y + outer_padding_top + int(layout.get("titleBottomGap", 10)) + int(layout.get("titleFontSize", 13))
    col_width = row_width if row_layout != "two-column" else (row_width - column_gap) // 2

    for index, row in enumerate(rows):
        col = 0 if row_layout != "two-column" else index % 2
        grid_row = index if row_layout != "two-column" else index // 2
        row_x = x + outer_padding_x + col * (col_width + column_gap)
        row_y = start_y + grid_row * (row_height + row_gap)
        emphasize = bool(row.get("emphasize"))
        fill = layout.get("rowEmphasisFill", "#0f172a") if emphasize else "#ffffff"
        outline = layout.get("rowEmphasisFill", "#0f172a") if emphasize else layout.get("rowBorderColor", "#e2e8f0")
        draw.rounded_rectangle((row_x, row_y, row_x + col_width, row_y + row_height), radius=6, fill=fill, outline=outline, width=1)
        label_color = layout.get("rowLabelEmphasisColor", "#cbd5e1") if emphasize else layout.get("rowLabelColor", "#64748b")
        value_color = layout.get("rowValueEmphasisColor", "#ffffff") if emphasize else layout.get("rowValueColor", "#0f172a")
        draw_text(draw, (row_x + row_pad_x, row_y + 7), row["label"], label_font, label_color)
        draw_text(draw, (row_x + col_width - row_pad_x, row_y + 7), row["value"], value_font, value_color, anchor="ra")

    show_note = bool(options.get("showNote", False))
    note_text = str(note.get("text", "")).strip()
    hide_empty_note = bool(options.get("hideEmptyNote", True))
    if show_note and not (hide_empty_note and note_text == ""):
        note_rows = len(rows) if row_layout != "two-column" else (len(rows) + 1) // 2
        note_top = start_y + note_rows * (row_height + row_gap) + note_top_gap
        note_height = max(52, height - (note_top - y) - outer_padding_top)
        draw.rounded_rectangle((x + outer_padding_x, note_top, x + width - outer_padding_x, note_top + note_height), radius=6, fill="#f8fafc", outline="#e2e8f0", width=1)
        draw_text(draw, (x + outer_padding_x + note_padding_x, note_top + note_padding_top), note.get("title", "Not"), note_title_font, layout.get("titleColor", "#64748b"))
        draw.multiline_text(
            (x + outer_padding_x + note_padding_x, note_top + note_padding_top + 18),
            note_text,
            font=note_body_font,
            fill="#475569",
            spacing=note_text_spacing)

    image.save(output_path)
    print(str(output_path))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
