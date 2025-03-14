<script lang="ts">
  let { error } = $props();
</script>

<h1 class=" text-3xl">{error.message || "Uh ooh!"}</h1>