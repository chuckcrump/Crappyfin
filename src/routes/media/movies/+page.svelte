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
  --gap: 1rem;
  --padding: 16px;
  
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
  gap: var(--gap);
  padding-top: var(--padding);
  padding-bottom: var(--padding);
  width: calc(100% - 2 * var(--padding));
  max-width: 100%;
  box-sizing: border-box;
}

.movie-grid > * {
  aspect-ratio: var(--ratio);
  width: 100%;
  height: auto;
  margin-bottom: 1rem;
}

@media (min-width: 512px) {
  .movie-grid {
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    --gap: 1rem;
  }
}

@media (min-width: 640px) {
  .movie-grid {
    grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
    --gap: 1rem;
  }
}

@media (min-width: 768px) {
  .movie-grid {
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    --gap: 1rem;
  }
}

@media (min-width: 1024px) {
  .movie-grid {
    grid-template-columns: repeat(auto-fill, minmax(190px, 1fr));
    --gap: 1rem;
  }
}

@media (min-width: 1280px) {
  .movie-grid {
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    --gap: 1rem;
  }
}
</style>