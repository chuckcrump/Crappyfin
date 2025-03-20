<script lang="ts">
  import { page } from "$app/state";
  console.log(page.error?.message)
</script>

<div class="flex w-screen h-screen flex-col items-center justify-center text-white gap-2">
  <h1 class="text-3xl">Uh Oh!</h1>
  <h3 class=" text-2xl">{page.error?.message}</h3>
  <p>Is your server online? check <a class="text-blue-500 underline" href="/admin/status">Here.</a> </p>
</div>