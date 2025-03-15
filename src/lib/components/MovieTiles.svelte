<script lang="ts">
  import { blur } from "svelte/transition";
  import { currentMovie, isPlaying } from "./stores";
  let {movie} = $props();

  function setMovie(movie: any) {
    currentMovie.set(movie);
    console.log($currentMovie)
    isPlaying.set(true);
  }
</script>

<div class="h-full w-full" out:blur={{duration: 200}} >
  <div class="flex flex-col h-full items-center bg-[#343434] rounded-[10px] hover:scale-[103%] transition-all ease-in-out">
    <button
      class="flex w-full h-full items-center justify-center rounded-lg overflow-hidden"
      onclick={() => setMovie(movie)}>
        {#if movie.cover_image == ""}
          <p class="w-full h-full bg-[#343434] text-white text-center">No image found</p>
        {:else}
          <img class="w-full h-full rounded-lg" src="{import.meta.env.VITE_TS_SERVER_URL}/movies/previews?path={movie.coverPath}" alt="No image found">
        {/if}
    </button>
  </div>
  <p class="text-white text-md text-center mt-2">{movie.name}</p>
</div>