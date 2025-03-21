<script lang="ts">
  import SideBar from "$lib/components/SideBar.svelte";
  import "../main.css"
  let { children } = $props();
</script>

<SideBar/>

{@render children()}

