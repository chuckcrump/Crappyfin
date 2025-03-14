<script lang="ts">
  import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
  import MovieTiles from "$lib/components/MovieTiles.svelte";
  import MoviePlayer from "$lib/components/MoviePlayer.svelte";
  import { isPlaying } from "$lib/components/stores.js";
  import { blur } from "svelte/transition";
  export let data 
</script>


<div class="flex">
  {#if !data.movies}
    <LoadingSpinner size="40" />
  {:else}
      {#if $isPlaying}
        <div in:blur={{duration: 600}} >
          <MoviePlayer />
        </div>
      {:else}
        <div class="flex flex-row gap-2 m-2" style="margin: 8px;">
          {#each data.movies as movie}
            <MovieTiles movie={movie} />
          {/each}
        </div>
      {/if}
  {/if}
</div>