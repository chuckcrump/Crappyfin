<script lang="ts">
  import { blur } from 'svelte/transition';

  let { data } = $props()
</script>

<div in:blur={{duration: 400}}>
</div>
