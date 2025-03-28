package main

import (
	"net/http"
)

func enableCors(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Content-Type, Authorization")

		if r.Method == http.MethodOptions {
				w.WriteHeader(http.StatusOK)
				return
		}

		next.ServeHTTP(w, r)
	})
}

const chunkSize = 5 * 1024 * 1024

func main() {
	parseJson()
	mux := http.NewServeMux()
	mux.HandleFunc("/go/media/stream", func(w http.ResponseWriter, r *http.Request) {
		streamMovie(w, r)
	})
	mux.HandleFunc("/go/media/subtitle", func(w http.ResponseWriter, r *http.Request) {
		sendSubtitle(w, r)
	})
	cors := enableCors(mux)
	http.ListenAndServe("0.0.0.0:8081", cors)
}
