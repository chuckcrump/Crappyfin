<script lang="ts">
  import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
  import MovieTiles from "$lib/components/MovieTiles.svelte";
  import MoviePlayer from "$lib/components/MoviePlayer.svelte";
  import { isPlaying } from "$lib/components/stores.js";
  import { blur } from "svelte/transition";
  import { onMount } from "svelte";
  import { error } from "@sveltejs/kit";
  //export let data
  type MovieObj = {
    uuid: string,
    name: string,
  }
  let movies: MovieObj[] = []
  
  async function load() {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_TS_SERVER_URL}/movies/list`,
        {keepalive: true}
      );
      if (!response.ok) {
        throw error(404, "Failed to fetch movies");
        return;
      }
      movies = await response.json();
    } catch (err) {
      throw error(404, `${err}`)
    }
  }

  onMount(async () => {
    await load()
  })

</script>

<div class="flex justify-center" style="margin-top: 48px;">
  {#if movies.length === 0}
    <div class="flex h-screen items-center justify-center absolute inset-1">
      <LoadingSpinner size="50" />
    </div>
  {:else}
      {#if $isPlaying}
        <div in:blur={{duration: 600}}>
          <MoviePlayer />
        </div>
      {:else}
        <div class="movie-grid">
          {#each movies as movie}
            <MovieTiles movie={movie} />
          {/each}
        </div>
      {/if}
  {/if}
</div>
