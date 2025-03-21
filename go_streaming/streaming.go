package main

import (
	"fmt"
	"io"
	"net/http"
	"net/url"
	"os"
	"strconv"
	"strings"
)

func streamMovie(w http.ResponseWriter, r *http.Request) {
	url, err := url.Parse(r.URL.String())
	if err != nil {
		return 
	}
	queries := url.Query()
	uuid := queries.Get("uuid")
	videoPath := ""
	for _, movie := range movies {
		if movie.UUID == uuid {
			videoPath = movie.VideoPath
		}
	}

	file, err := os.Open(videoPath)
	if err != nil {
		http.Error(w, "Could not open video file", http.StatusInternalServerError)
		return
	}
	defer file.Close()

	fileInfo, err := file.Stat()
	if err != nil {
		http.Error(w, "Could not get video file info", http.StatusInternalServerError)
		return
	}
	fileSize := fileInfo.Size()

	rangeHeader := r.Header.Get("Range")

	w.Header().Set("Accept-Ranges", "bytes")

	if rangeHeader != "" {
		parts := strings.Split(rangeHeader, "=")
		if len(parts) != 2 || parts[0] != "bytes" {
			http.Error(w, "Invalid range header", http.StatusBadRequest)
			return
		}

		ranges := strings.Split(parts[1], "-")
		if len(ranges) != 2 {
			http.Error(w, "Invalid range header format", http.StatusBadRequest)
			return
		}

		startStr, endStr := ranges[0], ranges[1]

		start := int64(0)
		if startStr != "" {
			start, err = strconv.ParseInt(startStr, 10, 64)
			if err != nil {
				http.Error(w, "Invalid start range", http.StatusBadRequest)
				return
			}
		}

		end := fileSize - 1
		if endStr != "" {
			end, err = strconv.ParseInt(endStr, 10, 64)
			if err != nil {
				http.Error(w, "Invalid end range", http.StatusBadRequest)
				return
			}
		}

		if start > end || start >= fileSize || end < 0 {
			w.Header().Set("Content-Range", fmt.Sprintf("bytes */%d", fileSize))
			w.WriteHeader(http.StatusRequestedRangeNotSatisfiable)
			return
		}

		chunkSizeToSend := end - start + 1
		if chunkSizeToSend > chunkSize {
			end = start + chunkSize - 1
			if end >= fileSize {
				end = fileSize - 1
			}
			chunkSizeToSend = end - start + 1
		}

		buffer := make([]byte, chunkSizeToSend)
		n, err := file.ReadAt(buffer, start)
		if err != nil && err != io.EOF {
			http.Error(w, "Could not read file chunk", http.StatusInternalServerError)
			return
		}

		w.Header().Set("Content-Range", fmt.Sprintf("bytes %d-%d/%d", start, end, fileSize))
		w.Header().Set("Content-Length", strconv.Itoa(n))
		w.Header().Set("Content-Type", "video/mp4")
		w.WriteHeader(http.StatusPartialContent)
		_, err = w.Write(buffer[:n])
		if err != nil {
			fmt.Println("Error writing response:", err)
		}
		return
	}

	w.Header().Set("Content-Length", strconv.Itoa(int(fileSize)))
	w.Header().Set("Content-Type", "video/mp4")
	w.WriteHeader(http.StatusOK)
	_, err = io.Copy(w, file)
	if err != nil {
		fmt.Println("Error writing full content:", err)
	}
}

func sendSubtitle(w http.ResponseWriter, r *http.Request) {
	url, err := url.Parse(r.URL.String())
	if err != nil {
		return
	}
	subUuid := url.Query().Get("uuid")
	subPath := ""
	for _, movie := range movies {
		if movie.UUID == subUuid {
			subPath = movie.SubtitlePath
		}
	}
	http.ServeFile(w, r, subPath)
}