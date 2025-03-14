import { error } from "console";
import fs from "fs";

function parse_my_srt(srt: string) {
  let vtt = "WEBVTT\n\n";

  srt = srt.replace(/\r\n/g, "\n");
  const subtitles = srt.trim().split(/\n\d+\n/);

  subtitles.forEach((subtitle) => {
    if (subtitle.trim() === "") return;
    const lines = subtitle.split("\n");

    if (lines.length >= 2) {
      const time = lines[0]
        .replace(",", ".")
        .replace("-->", "-->")
        .replace(",", ".");
      vtt += time + "\n";

      for (let i = 1; i < lines.length; i++) {
        vtt += lines[i] + "\n";
      }
      vtt += "\n";
    }
  });
  return vtt;
}

export function convert_file(input_path: string, output_path: string) {
  fs.readFile(input_path, "utf-8", (err, data) => {
    if (err) {
      console.error(error);
    }
    const vtt = parse_my_srt(data);
    fs.writeFile(output_path, vtt, "utf-8", (err) => {
      if (err) {
        console.error("failed to write file", error);
      }
      console.log("");
    });
  });
}
