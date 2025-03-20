<script lang="ts">
  import SideBar from "$lib/components/SideBar.svelte";
  import "../main.css"
  let { children } = $props();
  export const ssr = false;
</script>

  <SideBar/>

{@render children()}

