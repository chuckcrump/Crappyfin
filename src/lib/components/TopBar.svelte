<script lang="ts">
  import { List } from "svelte-iconoir";
  import { isPlaying, showSidebar } from "./stores";
  //import "@fortawesome/fontawesome-free/js/all"
  function toggleSideBar() {
    showSidebar.set(!$showSidebar)
  }
</script>

{#if !$isPlaying}
  <div class=" h-2000px w-screen">
    <div class="flex fixed z-10 p-1 shadow-2xl flex-row items-center top-0 h-12 w-screen bg-[#343434]">
      <button onclick={toggleSideBar} class="z-10 btn-normal hover:bg-[#444444]" aria-label="sidebar-toggle">
        <!--svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="white" class="bi bi-list" viewBox="0 0 16 16">
          <path fill-rule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5"/>
        </svg-->
        <List stroke-width={2} color="white" />
      </button>
      <div class="flex flex-row justify-center items-center w-full gap-1 mr-auto">
        <button class="btn-category">Movies</button>
        <button class="btn-category">Shows</button>
        <button class="btn-category">Music</button>
      </div>
      <p>User</p>
    </div>
  </div>
{/if}