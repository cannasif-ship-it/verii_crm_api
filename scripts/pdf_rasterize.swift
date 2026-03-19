import AppKit
import Foundation
import PDFKit

if CommandLine.arguments.count < 3 {
    FileHandle.standardError.write(Data("usage: pdf_rasterize.swift <input.pdf> <output-dir>\n".utf8))
    exit(1)
}

let inputPath = NSString(string: CommandLine.arguments[1]).expandingTildeInPath
let outputDirPath = NSString(string: CommandLine.arguments[2]).expandingTildeInPath
let outputDirUrl = URL(fileURLWithPath: outputDirPath, isDirectory: true)

try FileManager.default.createDirectory(at: outputDirUrl, withIntermediateDirectories: true)

guard let document = PDFDocument(url: URL(fileURLWithPath: inputPath)) else {
    FileHandle.standardError.write(Data("failed to load pdf: \(inputPath)\n".utf8))
    exit(2)
}

let dpi: CGFloat = 144
let scale: CGFloat = dpi / 72.0
let pageCount = document.pageCount

for pageIndex in 0..<pageCount {
    guard let page = document.page(at: pageIndex) else { continue }

    let mediaBox = page.bounds(for: .mediaBox)
    let imageSize = NSSize(width: mediaBox.width * scale, height: mediaBox.height * scale)

    let image = NSImage(size: imageSize)
    image.lockFocus()

    guard let context = NSGraphicsContext.current?.cgContext else {
        image.unlockFocus()
        continue
    }

    context.setFillColor(NSColor.white.cgColor)
    context.fill(CGRect(origin: .zero, size: imageSize))
    context.saveGState()
    context.scaleBy(x: scale, y: scale)
    page.draw(with: .mediaBox, to: context)
    context.restoreGState()
    image.unlockFocus()

    guard
        let tiffData = image.tiffRepresentation,
        let bitmap = NSBitmapImageRep(data: tiffData),
        let pngData = bitmap.representation(using: .png, properties: [:])
    else {
        continue
    }

    let outputFile = outputDirUrl.appendingPathComponent(String(format: "page-%03d.png", pageIndex + 1))
    try pngData.write(to: outputFile)
}

let manifest = outputDirUrl.appendingPathComponent("manifest.json")
let payload = ["pageCount": pageCount]
let manifestData = try JSONSerialization.data(withJSONObject: payload, options: [.prettyPrinted])
try manifestData.write(to: manifest)
