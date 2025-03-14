<script lang="ts">
  import { onMount } from "svelte";
  import { blur, fly } from "svelte/transition";
  import MediaControls from "./MediaControls.svelte";
  import { progress, subtitleTrack, volume, videoPaused, currentMovie, isPlaying } from "./stores";
  
  //let movies: any[] = []
  let currentMovieUrl = ""
  let currentSubUrl = ""
  let currentMovieName = ""
  let video: HTMLVideoElement | null = null;
  let showControls = false
  let timeout: any;
  let currentSubText: string;
  let trackElement: HTMLTrackElement | null = null;

  async function loadMovie(movie: any) {
    currentMovieUrl = `${import.meta.env.VITE_GO_SERVER_URL}/go/media/stream?movie=${movie.videoPath}`
    currentSubUrl = `${import.meta.env.VITE_GO_SERVER_URL}/go/media/subtitles?sub=${movie.subtitlePath}`
    if (movie.name) {
      currentMovieName = movie.name
    }
    hideSubs()
  }
  $: loadMovie($currentMovie);

  function toggleMovie() {
    if (video?.paused) {
      video?.play();
      videoPaused.set(false);
    } else {
      video?.pause();
      videoPaused.set(true);
    } 
  }

  function progressUpdate() {
    if (video) {
      progress.set((video.currentTime / video.duration) * 100);
    }
  }
  function updateSubText() {
    if (video) {
      console.log("chagei")
      const track = video?.textTracks[0];
      if (!track) console.log("No subtitle")
      const activeCues = track?.activeCues;
      currentSubText = activeCues && activeCues.length > 0 ? (activeCues[0] as VTTCue).text : ""
    }
  }
  
  function adjust() {
    const targetVol = event?.target as HTMLInputElement
    volume.set(parseFloat(targetVol.value))
  }

  function hideSubs() {
    if (trackElement) {
      const track = (trackElement as HTMLTrackElement)?.track;
      if (track) {
        track.mode = "hidden";
      }
    }
  }

  $: if (video) {
    video.volume = $volume
  }
  
  onMount(async () => {
    hideSubs()
    subtitleTrack.set("")
    progress.set(0)
    videoPaused.set(true)
    if (video) {
      video.volume = 1.0
    }
    window.addEventListener("keydown", (event) => {
      if (event.code === "Space") {
        toggleMovie()
      }
    })
  })

</script>

{#if $isPlaying}
  <!-- svelte-ignore a11y_no_static_element_interactions -->
  <div
    class="video_container items-center justify-center h-screen bg-black"
    transition:blur={{duration: 600}}
    onmousemove={() => {
      if (timeout) {
        clearTimeout(timeout)
      }
      showControls = true;
      timeout = setTimeout(() => {
      showControls = false
    }, 2000);}}
  >
    <!-- svelte-ignore a11y_media_has_caption -->
    <video
      crossorigin="anonymous"
      onloadedmetadata={toggleMovie}
      bind:this={video}
      ontimeupdate={progressUpdate}
      onclick={toggleMovie}
    >
      <source src={currentMovieUrl} type="video/mp4" >
      <track class=" hidden" bind:this={trackElement} oncuechange={updateSubText} src={currentSubUrl} kind="subtitles" srclang="en" label="English"/>
    </video>
    <div class="flex items-center justify-center fixed bottom-[75px] max-w-[500px] text-wrap">
      <p class=" text-center text-[27px] text-white">{@html currentSubText}</p>
    </div>
    {#if showControls}
      <div class="flex flex-row top-1 left-1 fixed items-center" transition:fly={{duration: 200, y: -100}}>
        <button  aria-label="back" class="text-white p-1 m-1 hover:bg-gray-400/20 rounded-full h-[40px] w-[40px]" onclick={() => {isPlaying.set(false); progress.set(0); subtitleTrack.set("")}}>
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-arrow-left-short" viewBox="0 0 16 16">
            <path fill-rule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5"/>
          </svg>
        </button>
        <p class="text-white text-[20px] h-fit">{currentMovieName}</p>
      </div>
      <MediaControls video={video} volumeSetter={adjust}></MediaControls>
    {/if}
  </div>
{/if}

<style>
  .video_container {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
  }
  video {
    width: 100vw;
  }
</style>
