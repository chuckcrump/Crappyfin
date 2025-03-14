<script lang="ts">
  let {size} = $props();
</script>

<div
  style="height: {size}px; width: {size}px;"
  class="spinner fixed transform -translate-x-1/2 -translate-y-1/2 rounded-full border-[6px] border-transparent border-t-indigo-500 border-b-indigo-500 " 
>
</div>

<style>
  .spinner  {
    animation: spin 1s ease-in-out infinite;
  }
  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
</style>