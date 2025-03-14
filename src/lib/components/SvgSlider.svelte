<script lang="ts">

	export let value = 0.0;
  export let seek: () => void;

	let svg: SVGSVGElement;
  let screen = window.innerWidth;

	function handleClick(event: any) {
    // Create an SVG point based on the mouse click's client coordinates
    const point = svg.createSVGPoint();
    point.x = event.clientX;
    point.y = event.clientY;

    const ctm = svg.getScreenCTM();

    // Convert the point to the SVG's coordinate system
    const svgPoint = point.matrixTransform(ctm!.inverse());

    const newX = Math.min(Math.max(svgPoint.x, 0), screen)

    value = (newX / screen) * 100
    if (seek) seek()
  }

	function startDrag(event: any) {
    // Define a handler to update value on mouse move
    const handleMove = (e: any) => {
      const point = svg.createSVGPoint();
      point.x = e.clientX;
      point.y = e.clientY;
      const ctm = svg.getScreenCTM();

      const svgPoint = point.matrixTransform(ctm!.inverse());
      const newX = Math.min(Math.max(svgPoint.x, 0.0), screen);
      value = (newX / screen) * 100; 
      console.log(value)
      if (seek) seek();
    };

    // Define a handler to clean up once dragging stops
    const handleUp = () => {
      window.removeEventListener('mousemove', handleMove);
      window.removeEventListener('mouseup', handleUp);
    };

    // Add global listeners so dragging works even if the cursor leaves the circle
    window.addEventListener('mousemove', handleMove);
    window.addEventListener('mouseup', handleUp);
	}
  $: screen = window.innerWidth;
</script>
<svg bind:this={svg} viewBox={`-1 -5 ${screen} 10`} width="80%" class=" overflow-visible">
	<g>
		<!-- svelte-ignore a11y_click_events_have_key_events -->
		<!-- svelte-ignore a11y_no_static_element_interactions -->
		<line
			x1={0} y1={0} 
			x2={screen} y2={0}
			stroke="gray"
			stroke-width={15}
      stroke-linecap="round"
			onclick={handleClick}
		/>
		<!-- svelte-ignore a11y_click_events_have_key_events -->
		<!-- svelte-ignore a11y_no_static_element_interactions -->
		<line
			x1={0} y1={0}
			x2={(value / 100) * screen} y2={0}
			stroke="blue"
			stroke-width={15}
      stroke-linecap="round"
			onclick={handleClick}
		/>
		<!-- <circle cx={value} cy={0} r={1} on:mousedown={startDrag} /> -->
		<!-- svelte-ignore a11y_no_static_element_interactions -->
		<circle
			cx={(value / 100) * screen} cy={0} r={3}
			fill="blue"
			stroke="blue"
			stroke-width={10}
			onmousedown={startDrag}
		/>
	</g>
</svg>

<style>
	circle {
		fill: white;
	}
	line {
		stroke-linecap: round;
	}
</style>