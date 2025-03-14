<script lang="ts">
  import { fly } from "svelte/transition";
  import { progress, subtitleTrack, volume, videoPaused, formattedTime } from "./stores";
  import { onMount } from "svelte";
  let {video, volumeSetter} = $props()

  function toggle() {
    if (video.paused) {
      videoPaused.set(false)
      video.play()
    } else {
      videoPaused.set(true)
      video.pause()
    }
  }

  function seekVideo() {
    console.log("seeker")
    if (video) {
      video.currentTime = ($progress) / 100 * video.duration;
    }
  }

  function changeSubtitle(event: any) {
    subtitleTrack.set(event.target.value)
    updateSubtitle()
  }
  
  function updateSubtitle() {
    if (video && video.textTracks) {
      for (let track of video.textTracks) {
        track.mode = track.language === $subtitleTrack ? "showing" : "hidden"
      }
    }
  }
  onMount(() => {
    document.addEventListener("keydown", function(event) {
      if (event.key === "ArrowLeft") {
        if (video) {
          event.preventDefault()
          video.currentTime = video.currentTime - 5;
        }
      } else if (event.key === "ArrowRight") {
        if (video) {
          event.preventDefault();
          video.currentTime = video.currentTime + 5;
        }
      }
    })
  })
</script>

<div transition:fly={{duration: 300, y: 100}} class="flex flex-row items-center w-screen fixed p-2 bottom-4 gap-1">
  <button aria-label="p and p" onclick={toggle} class="text-white bg-transparent hover:bg-gray-400/20 rounded-full p-1.5">
    {#if $videoPaused}
      <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-play-fill" viewBox="0 0 16 16">
        <path d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393"/>
      </svg>
    {:else}
      <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-pause-fill" viewBox="0 0 16 16">
        <path d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5m5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5"/>
      </svg>
    {/if}
  </button>
  <p class="text-white">{$formattedTime}</p>
  <input class="w-full media-slider" type="range" min="0" max="100" step="0.01" bind:value={$progress} oninput={seekVideo}>
  <input class="w-40 media-slider ml-1" type="range" min="0" max="1" step="0.01" bind:value={$volume} oninput={volumeSetter}>
  <select bind:value={$subtitleTrack} onchange={changeSubtitle} class="text-white bg-black">
    <option value="none">None</option>
    <option value="en">English</option>
  </select>
</div>
