import { Hono } from "hono";
import { cors } from "hono/cors";
import { get_movies } from "./media_parser";
import { exec } from "child_process";

exec("go_streaming/stream", (stdout) => {
  console.log("go started");
});

interface Request {
  movie_path?: string;
  subitle?: string;
}

const app = new Hono();

app.use(
  cors({
    origin: "*",
    allowMethods: ["GET", "POST", "PUT", "DELETE"],
  })
);

app.get("/movies/list", async (c) => {
  let movie_array = await get_movies("media");
  return c.json(movie_array, 200);
});

app.get("/movies/previews", async (c) => {
  const path = c.req.query("path");
  if (!path) {
    return c.text("Path is needed!");
  }
  const file = Bun.file(path);
  const array_buffer = await file.arrayBuffer();
  return new Response(array_buffer, {
    headers: {
      "Content-Type": "image/jpg",
    },
  });
});

console.log("running on port 8080");
Bun.serve({
  fetch: app.fetch,
  port: 8080,
  hostname: "0.0.0.0",
});
