using System.Text.Json;
using backend;

namespace Parser;

public class DirectoryTraverser
{
    public static List<MovieClass> Traverse(string mediaPath)
    {
        List<MovieClass> Movies = new();
        string[] mediaDirs = Directory.GetDirectories(mediaPath);
        string jsonPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
        foreach (var movie in mediaDirs)
        {
            Movies.Add(ProcessMovie(movie));
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        using (StreamWriter sw = new StreamWriter(jsonPath + "/movie.json"))
        {
            sw.Write(JsonSerializer.Serialize(Movies, options));
        }
        return Movies;
    }

    private static MovieClass ProcessMovie(string path)
    {
        MovieClass movie = new MovieClass("", "", "", "", "");
        movie.Name = Path.GetFileName(path);

        string[] movieFiles = Directory.GetFiles(path);
        bool vttExists = movieFiles.Any(f => Path.GetExtension(f).Equals(".vtt", StringComparison.OrdinalIgnoreCase));
        foreach (var file in movieFiles)
        {
            string mimeType = MimeTypes.GetMimeType(file);
            movie.Mimes = mimeType;
            string extension = Path.GetExtension(file);

            if (mimeType.StartsWith("video/", StringComparison.OrdinalIgnoreCase)) {
                movie.VideoPath = file;
            } else if (mimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
                movie.CoverPath = file;
            }

            switch (extension)
            {
                case ".srt":
                    if (!vttExists)
                    {
                        movie.SubtitlePath = SrtParser.Parser(file);
                    }
                    break;
                case ".vtt":
                    movie.SubtitlePath = file;
                    break;
                default:
                    Console.WriteLine("Found unknown: " + file);
                    break;
            }
        }
        return movie;
    }
}
