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

<div class="h-full w-full pb-7" out:blur={{duration: 200}} >
  <div class="flex flex-col h-full" >
    <div class=" flex-grow bg-[#343434] overflow-hidden rounded-lg hover:scale-[103%] transition-all ease-in-out" style="aspect-ratio: 2/3;">
      <button
        class="flex w-full h-full items-center justify-center"
        onclick={() => setMovie(movie)}>
        {#if movie.coverPath == ""}
          <p class="flex items-center justify-center w-full h-full bg-[#343434] text-white text-center p-4">No image found</p>
        {:else}
          <!-- svelte-ignore a11y_img_redundant_alt -->
          <img class="w-full h-full" src="{import.meta.env.VITE_TS_SERVER_URL}/movies/previews?path={movie.coverPath}" alt="No image found" loading="lazy" >
        {/if}
      </button>
    </div>
  </div>
  <p class="text-white text-md text-center mt-2 truncate px-1">{movie.name}</p>
</div>