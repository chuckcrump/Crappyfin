package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"strconv"

	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
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

func stream_video(c *fiber.Ctx) error {
	path := c.Query("path")
	if path == "" {
		return c.Status(fiber.StatusNotFound).SendString("No path sent")
	}

	videoPath := path
	file, err := os.Open(videoPath)
	if err != nil {
		return c.Status(fiber.StatusNotFound).SendString("Video not found")
	}
	defer file.Close()

	stat, _ := file.Stat()
	fileSize := stat.Size()

	c.Set("Content-Type", "video/mp4")
	c.Set("Accept-Ranges", "bytes")

	rangeHeader := c.Get("Range")
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

		c.Set("Content-Range", fmt.Sprintf("bytes %d-%d/%d", start, end, fileSize))
		c.Set("Content-Length", strconv.FormatInt(end - start + 1, 10))
		c.Status(http.StatusPartialContent)

		file.Seek(start, 0)
		return c.SendStream(io.LimitReader(file, end - start + 1))
	}

	c.Set("Content-Length", strconv.FormatInt(fileSize, 10))
	return c.SendStream(file)
}

func stream_subtitle(c *fiber.Ctx) error {
  sub_path := c.Query("sub")
  return c.SendFile(sub_path)
}

func main() {
	app := fiber.New()
	app.Use(cors.New(cors.Config{
		AllowOrigins: "*",
		AllowMethods: "GET,POST,OPTIONS",
		AllowHeaders: "Origin, Content-Type, Accept, Authorization",
	}))
	app.Get("/go/media/stream")
	app.Listen(":8081")

	//mux := http.NewServeMux()
	//mux.HandleFunc("/go/media/stream", stream_video)
	//mux.HandleFunc("/go/media/subtitles", stream_subtitle)
	//mux.HandleFunc("/go/media/token", handleTokens)
	//cors := enableCors(mux)
	//fmt.Println("Server running at http://0.0.0.0:8081/go")
	//http.ListenAndServe("0.0.0.0:8081", cors)
}
