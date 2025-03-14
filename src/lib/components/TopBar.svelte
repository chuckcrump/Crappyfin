<script lang="ts">
  import { isPlaying } from "./stores";
  //import "@fortawesome/fontawesome-free/js/all"
</script>

{#if !$isPlaying}
  <div class="flex flex-row items-center top-0 h-12 w-screen bg-[#343434]">
  </div>
{/if}