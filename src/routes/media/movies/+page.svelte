<script lang="ts">
  import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
  import MovieTiles from "$lib/components/MovieTiles.svelte";
  import MoviePlayer from "$lib/components/MoviePlayer.svelte";
  import { isPlaying } from "$lib/components/stores.js";
  import { blur } from "svelte/transition";
  export let data 
</script>

<div class="flex justify-center">
  {#if !data.movies}
    <LoadingSpinner size="40" />
  {:else}
      {#if $isPlaying}
        <div in:blur={{duration: 600}}>
          <MoviePlayer />
        </div>
      {:else}
        <div class="movie-grid">
          {#each data.movies as movie}
            <MovieTiles movie={movie} />
          {/each}
        </div>
      {/if}
  {/if}
</div>

<style>
  .movie-grid {
    --ratio: 2/3;
    --default-width: 140px;

    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(--default-width), 1fr);
    gap: 1rem;
    padding: 16px;
    width: 100%;
    max-width: 100vw;
  }
  .movie-grid > :global(*) {
    aspect-ratio: var(--ratio);
    width: 100%;
    height: auto;
    max-width: 100%;
    margin-bottom: 3rem;
  }
  @media (min-width: 512px) {
    .movie-grid {
      --default-width: 350px;
    }
  }
  @media (min-width: 640px) {
    .movie-grid {
      --default-width: 300px;
    }
  }
  @media (min-width: 768px) {
    .movie-grid {
      grid-template-columns: repeat(4, 1fr);
    }
  }
  @media (min-width: 1024px) {
    .movie-grid {
      grid-template-columns: repeat(5, 1fr);
    }
  }
  @media (min-width: 1280px) {
    .movie-grid {
      grid-template-columns: repeat(6, 1fr);
    }
  }
</style>