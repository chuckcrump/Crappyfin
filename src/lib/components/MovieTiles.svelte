<script lang="ts">
  import { blur } from "svelte/transition";
  import { currentMovie, isPlaying } from "./stores";
  let {movie} = $props();

  function setMovie(movie: any) {
    currentMovie.set(movie);
    //isPlaying.set(true);
  }
</script>

<div class="h-full w-full" transition:blur={{duration: 200}} >
  <div class="flex flex-col h-full shadow-lg" >
    <div class=" flex-grow bg-[#343434] overflow-hidden rounded-lg hover:scale-[103%] transition-all ease-in-out duration-200" style="aspect-ratio: 2/3;">
      <a
        class="flex w-full h-full items-center justify-center"
        href="/watch/{movie.uuid}"
      >
        {#if movie.coverPath == ""}
          <p class="flex items-center justify-center w-full h-full bg-[#343434] text-white text-center p-4">No image found</p>
        {:else}
          <!-- svelte-ignore a11y_img_redundant_alt-->
          <img class="w-full h-full object-cover" src="{import.meta.env.VITE_TS_SERVER_URL}/movies/preview/{movie.uuid}/cover" alt="No image found" loading="lazy" >
        {/if}
    </a>
    </div>
  </div>
  <p class="text-white text-md text-center mt-2 truncate px-1">{movie.name}</p>
</div>