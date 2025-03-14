import { writable, type Writable } from "svelte/store";

interface Movie {
  path: string;
  sub: string;
  name: string;
}

export const currentMovie: Writable<Movie> = writable({
  path: "",
  sub: "",
  name: "",
});
export const isPlaying = writable(false);
export const progress = writable(0);
export const volume = writable(1.0);
export const subtitleTrack = writable("");
export const videoPaused = writable(true);
export const formattedTime = writable("");
