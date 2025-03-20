import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params }) => {
  const response = await fetch(
    `${import.meta.env.VITE_TS_SERVER_URL}/movies/stream/${params.uuid}`,
    { keepalive: true }
  );
  if (!response.ok) {
    return { error: "movie not found" };
  }
  const movieData = await response.json();
  return { movieData };
};
