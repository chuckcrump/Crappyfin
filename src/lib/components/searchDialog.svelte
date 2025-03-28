<script lang="ts">
  import { blur } from "svelte/transition";
  import { searchQuery, showSearchDialog } from "./stores";

  let search: string = ""

  function hideSearchDialog() {
    showSearchDialog.set(false)
  }
  function handleInput() {
    searchQuery.set(search)
    console.log($searchQuery)
  }

</script>

{#if $showSearchDialog}
  <div transition:blur={{duration: 200}} class="flex z-20 fixed w-3/4 h-3/4 p-2 rounded-2xl bg-[#343434]/60 backdrop-blur-3xl">
    <input oninput={handleInput} bind:value={search} class="text-white outline-0 p-1 rounded-xl h-10 w-full" type="text" placeholder="Search...">
    <button aria-label="hide" onclick={hideSearchDialog} class=" h-fit w-fit p-1 rounded-xl active:scale-90 transition-all ease-in-out">
      <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="white" class="bi bi-x" viewBox="0 0 16 16">
        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708"/>
      </svg>
    </button>
  </div>
{/if}