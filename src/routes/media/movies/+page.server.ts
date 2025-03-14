import type { PageServerLoad } from "./$types";
import { error } from "@sveltejs/kit";

export const load: PageServerLoad = async () => {
  try {
    const response = await fetch("http://192.168.0.90:8080/media/list");
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
