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

<div class=" flex flex-col items-center" out:blur={{duration: 200}} >
  <div class=" flex flex-col items-center bg-[#343434] rounded-[10px] hover:scale-[103%] transition-all ease-in-out">
    <button
      class=" flex text-wrap w-56 h-[334px] items-center justify-center rounded-xl"
      onclick={() => setMovie(movie)}>
        {#if movie.cover_image == ""}
          <p class="h-[320px] bg-[#343434] text-white">No image found</p>
        {:else}
          <img class="w-56 rounded-lg" src="{import.meta.env.VITE_TS_SERVER_URL}/media/previews?path={movie.cover_image}" alt="">
        {/if}
    </button>
  </div>
  <p class="text-white text-md">{movie.name}</p>
</div>