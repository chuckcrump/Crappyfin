import type { PageServerLoad } from "./$types";
import { error } from "@sveltejs/kit";

export const load: PageServerLoad = async ({ depends }) => {
  depends("movies:list");
  try {
    const response = await fetch(
      `${import.meta.env.VITE_TS_SERVER_URL}/movies/list`,
      { keepalive: true }
    );
    if (!response.ok) {
      throw error(response.status, {
        message: "Failed to fetch movies",
      });
    }
    const movies = await response.json();
    return { movies };
  } catch (err: any) {
    if (err.status) {
      throw err;
    }
    throw error(500, {
      message: "Failed to fetch movies",
    });
  }
};
