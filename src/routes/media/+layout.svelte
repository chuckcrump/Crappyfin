<script lang="ts">
  import TopBar from '$lib/components/TopBar.svelte';

  let { children } = $props();
</script>

<TopBar/>

{@render children()}