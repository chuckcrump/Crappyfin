package main

import (
	"net/http"
)

const chunkSize = 5 * 1024 * 1024

func main() {
	parseJson()
	http.HandleFunc("/go/media/stream", func(w http.ResponseWriter, r *http.Request) {
		streamMovie(w, r)
	})
	http.ListenAndServe("0.0.0.0:8081", nil)
}
