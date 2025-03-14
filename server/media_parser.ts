import fs from "fs";
import { convert_file } from "./srt_parser";

type Files = {
  uuid?: string;
  name?: string;
  path?: string;
  subtitle_path?: string;
  cover_image?: string;
};

let files: Files[] = [];

function get_relevant_files(movie_path: string, name: string) {
  let subtitle_file: string = "";
  let movie_file: string = "";
  let cover_image: string = "";
  let vtt_exists: boolean = false;
  let movie_obj: Files = {};
  const inside_files = fs.readdirSync(movie_path);

  for (const file of inside_files) {
    let full_path = `${movie_path}/${file}`;
    let extension = file.split(".").pop();

    if (extension === "mp4") {
      movie_file = file;
    } else if (extension === "vtt") {
      subtitle_file = file;
      vtt_exists = true;
    } else if (extension === "srt" && !vtt_exists) {
      let vtt_name = `${file}.vtt`;
      convert_file(full_path, `${movie_path}/${vtt_name}`);
      subtitle_file = vtt_name;
    } else if (extension === "jpg" || extension === "jpeg") {
      console.log(file);
      cover_image = `${movie_path}/${file}`;
    }
  }

  if (movie_file) {
    const duplicate = files.some((exist_file) => exist_file.name === name);
    if (!duplicate) {
      movie_obj = {
        uuid: crypto.randomUUID(),
        name: name,
        path: movie_path + "/" + movie_file,
        subtitle_path: subtitle_file ? `${movie_path}/${subtitle_file}` : "",
        cover_image: cover_image,
      };
      files.push(movie_obj);
    }
  }
  return movie_obj;
}

export async function get_movies(path: string) {
  const movies = fs.readdirSync(path);
  movies.forEach(async (movie) => {
    const meta = fs.statSync(`${path}/${movie}`);
    if (meta.isDirectory()) {
      get_relevant_files(`${path}/${movie}`, movie);
    } else {
      files.push({
        uuid: crypto.randomUUID(),
        name: movie,
        path: `${path}/${movie}`,
      });
    }
  });
  return files;
}

//get_movies("../media")
