package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"strconv"
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

func stream_video(w http.ResponseWriter, r *http.Request) {
	movie_path := r.URL.Query().Get("movie")
	if movie_path == "" {
		w.WriteHeader(http.StatusNotFound)
	}
  //formatted_path := movie_path[3:]
	videoPath := movie_path 
	file, err := os.Open(videoPath)
	if err != nil {
		http.Error(w, "File not found", http.StatusNotFound)
		return
	}
	defer file.Close()

	stat, _ := file.Stat()
	fileSize := stat.Size()

	w.Header().Set("Content-Type", "video/mp4")
	w.Header().Set("Accept-Ranges", "bytes")

	rangeHeader := r.Header.Get("Range")
	if rangeHeader != "" {
		var start, end int64
		fmt.Sscanf(rangeHeader, "bytes=%d-%d", &start, &end)
		chunks := int64(4 * 1024 * 1024)

		if end == 0 || end >= fileSize {
			end = fileSize - 1
		}
		if end - start + 1 > chunks {
			end = start + chunks - 1
			if end >= fileSize {
				end = fileSize - 1
			}
		}

		w.Header().Set("Content-Range", fmt.Sprintf("bytes %d-%d/%d", start, end, fileSize))
		w.Header().Set("Content-Length", strconv.FormatInt(end-start+1, 10))
		w.WriteHeader(http.StatusPartialContent)

		file.Seek(start, 0)
		io.CopyN(w, file, end-start+1)
		return
	}

	w.Header().Set("Content-Length", strconv.FormatInt(fileSize, 10))
	io.Copy(w, file)
}

func stream_subtitle(w http.ResponseWriter, r *http.Request) {
  sub_path := r.URL.Query().Get("sub")
  http.ServeFile(w, r, sub_path)
}

func main() {
	mux := http.NewServeMux()
	mux.HandleFunc("/go/media/stream", stream_video)
	mux.HandleFunc("/go/media/subtitles", stream_subtitle)
	cors := enableCors(mux)
	fmt.Println("Server running at http://0.0.0.0:8081/go")
	http.ListenAndServe("0.0.0.0:8081", cors)
}
