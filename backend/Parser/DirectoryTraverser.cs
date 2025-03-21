using System.Text.Json;
using backend.Database;

namespace backend.Parser;

public static class DirectoryTraverser
{
    public static async Task<List<MovieClass>> Traverse(string mediaPath)
    {
        List<MovieClass> movies = [];
        var mediaDirs = Directory.GetDirectories(mediaPath);
        var jsonPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
        foreach (var movie in mediaDirs)
        {
            var idPath = Path.Combine(movie, ".id.txt");
            if (!File.Exists(idPath))
            {
                AddId(Guid.NewGuid().ToString(), movie);
            }
            movies.Add(ProcessMovie(movie));
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await using (var sw = new StreamWriter(jsonPath + "/movie.json"))
        {
            await sw.WriteAsync(JsonSerializer.Serialize(movies, options));
        }
        await using (var dbContext = new MovieDbContext())
        {
            foreach (var movie in movies)
            {
                var existing = await dbContext.Movies.FindAsync(movie.Uuid);
                if (existing != null)
                {
                    existing.Name = movie.Name;
                    existing.VideoPath = movie.VideoPath;
                    existing.CoverPath = movie.CoverPath;
                    existing.SubtitlePath = movie.SubtitlePath;
                    existing.Mimes = movie.Mimes;
                    dbContext.Movies.Update(existing);
                }
                else
                {
                    await dbContext.Movies.AddAsync(movie);
                }
            }
            await dbContext.SaveChangesAsync();
        }
        return movies;
    }

    private static MovieClass ProcessMovie(string path)
    {
        var movie = new MovieClass("", "", "", "", "", "")
        {
            Name = Path.GetFileName(path)
        };

        var idPath = Path.Combine(path, ".id.txt");
        if (File.Exists(idPath))
        {
            movie.Uuid = File.ReadAllText(idPath).Trim();
        }
        else
        {
            movie.Uuid = Guid.NewGuid().ToString();
            AddId(movie.Uuid, path);
        }

        var movieFiles = Directory.GetFiles(path);
        bool vttExists = movieFiles.Any(f => Path.GetExtension(f).Equals(".vtt", StringComparison.OrdinalIgnoreCase));

        foreach (var file in movieFiles)
        {
            var mimeType = MimeTypes.GetMimeType(file);
            movie.Mimes = mimeType;
            var extension = Path.GetExtension(file);

            if (mimeType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            {
                movie.VideoPath = file;
            }
            else if (mimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
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
                    //Console.WriteLine("No subtitle found");
                    break;
            }
        }
        return movie;
    }
    private static void AddId(string uuid, string path)
    {
        File.WriteAllText(path + "/.id.txt", uuid);
    }
}
