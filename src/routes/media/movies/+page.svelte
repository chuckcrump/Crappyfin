<script lang="ts">
  import { blur } from "svelte/transition";
  import { onMount } from "svelte";
  import { error } from "@sveltejs/kit";
  import { isPlaying, searchQuery } from "$lib/components/stores.js";
  import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
  import MovieTiles from "$lib/components/MovieTiles.svelte";
  import MoviePlayer from "$lib/components/MoviePlayer.svelte";
  import SearchDialog from "$lib/components/searchDialog.svelte";
  //export let data
  type MovieObj = {
    uuid: string,
    name: string,
  }
  let movies: MovieObj[] | null = null
  let filteredMovies: MovieObj[] | null = []

  searchQuery.subscribe(search)  

  function search() {
    console.log(searchQuery)
    if ($searchQuery === "") {
      filteredMovies = movies
      return
    }
    console.log("search")
    if (movies) {
      filteredMovies = movies.filter(movie => {
        return movie.name.toLowerCase().includes($searchQuery.toLowerCase())
      })
      console.log(filteredMovies)
    }
  }

  async function load() {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_TS_SERVER_URL}/movies/list`,
        {keepalive: true}
      );
      if (!response.ok) {
        throw error(404, "Failed to fetch movies");
      }
      movies = await response.json();
      filteredMovies = movies
    } catch (err) {
      throw error(404, `${err}`)
    }
  }

  onMount(async () => {
    await load()
  })
</script>

<div class="flex justify-center" style="margin-top: 48px;">
  <SearchDialog/>
  {#if !filteredMovies}
    <div class="flex h-screen items-center justify-center absolute inset-1">
      <LoadingSpinner size="50" />
    </div>
  {:else}
    <div class="movie-grid">
      {#each filteredMovies as movie}
        <MovieTiles movie={movie} />
      {/each}
    </div>
  {/if}
</div>
