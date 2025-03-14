<script lang="ts">
  import { page } from "$app/state";
  console.log(page.error?.message)
</script>

<div class="flex w-screen h-screen flex-col items-center justify-center text-white gap-2">
  <h1 class="text-3xl">Uh Oh!</h1>
  <h3>{page.error?.message}</h3>
</div>