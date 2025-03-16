<script lang="ts">
  import { isPlaying } from "./stores";
  //import "@fortawesome/fontawesome-free/js/all"
</script>

{#if !$isPlaying}
  <div class=" h-2000px w-screen">
    <div class="flex z-10 shadow-2xl sticky flex-row items-center top-0 h-12 w-screen bg-[#343434]">
    </div>
  </div>
{/if}