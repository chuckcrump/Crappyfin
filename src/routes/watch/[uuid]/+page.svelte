<script lang="ts">
  import { blur } from 'svelte/transition';
  import { page } from '$app/state';
  let uuid: string | undefined = $state("");
  $effect: uuid = page.url.pathname.split("/").pop()
  let { data } = $props()
</script>

<div in:blur={{duration: 400}}>
  <p>{uuid}</p>
  <!-- svelte-ignore a11y_media_has_caption -->
  <video src="{import.meta.env.VITE_GO_SERVER_URL}/go/media/stream?uuid={uuid}" controls></video>
</div>
