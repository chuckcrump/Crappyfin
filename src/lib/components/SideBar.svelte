<script lang="ts">
  import { fly } from "svelte/transition";
  import { showSidebar } from "./stores";


</script>

{#if $showSidebar}
  <div transition:fly={{duration: 200, x: "-256"}} class="z-20 flex fixed p-2 flex-col h-screen w-64 bg-[#343434]">
    <h1 class=" text-2xl text-white">Crappyfin</h1>
    <a href="/media/movies">content</a>
  </div>
{/if}