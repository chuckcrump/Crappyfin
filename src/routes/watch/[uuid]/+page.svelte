<script lang="ts">
  import { blur } from 'svelte/transition';
  import { page } from '$app/state';
  import MoviePlayer from '$lib/components/MoviePlayer.svelte';
  import { onMount } from 'svelte';
  let uuid: string | undefined;
  
  onMount(async () => {
    uuid = page.params.uuid;
  })

</script>

<div class=" h-screen w-screen bg-black" in:blur={{duration: 400}}>
  <MoviePlayer uuid={uuid} />
</div>
