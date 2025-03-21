package main

import (
	"encoding/json"
	"fmt"
	"os"
)

type Movie struct {
	UUID string `json:"uuid"`
	Name string `json:"name"`
	VideoPath string `json:"videoPath"`
	SubtitlePath string `json:"subtitlePath"`
	CoverPath string `json:"coverPath"`
	Mimes string `json:"mimes"`
}

var movies []Movie

func parseJson() {
	jsonData, err := os.ReadFile("/home/andy/projects/crappyfin/streaming-frontend/media/movie.json")
	if err != nil {
		fmt.Println("failed to read file")
		return
	}
	movieData := string(jsonData)
	err = json.Unmarshal([]byte(movieData), &movies)
	if err != nil {
		fmt.Println("failed to parse json")
		return
	}
}