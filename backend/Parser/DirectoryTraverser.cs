using System.Text.Json;
using backend;

namespace Parser;

public class DirectoryTraverser
{
    public static async Task<List<MovieClass>> Traverse(string mediaPath)
    {
        List<MovieClass> Movies = new();
        string[] mediaDirs = Directory.GetDirectories(mediaPath);
        string jsonPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
        foreach (var movie in mediaDirs)
        {
            string idPath = Path.Combine(movie, ".id.txt");
            if (!File.Exists(idPath))
            {
                addId(Guid.NewGuid().ToString(), movie);
            }
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
        using (var DbContext = new MovieDbContext())
        {
            foreach (var movie in Movies)
            {
                var existing = await DbContext.Movies.FindAsync(movie.Uuid);
                if (existing != null)
                {
                    existing.Name = movie.Name;
                    existing.VideoPath = movie.VideoPath;
                    existing.CoverPath = movie.CoverPath;
                    existing.SubtitlePath = movie.SubtitlePath;
                    existing.Mimes = movie.Mimes;
                    DbContext.Movies.Update(existing);
                }
                else
                {
                    await DbContext.Movies.AddAsync(movie);
                }
            }
            await DbContext.SaveChangesAsync();
        }
        return Movies;
    }

    private static MovieClass ProcessMovie(string path)
    {
        var movie = new MovieClass("", "", "", "", "", "");
        movie.Name = Path.GetFileName(path);

        string idPath = Path.Combine(path, ".id.txt");
        if (File.Exists(idPath))
        {
            movie.Uuid = File.ReadAllText(idPath).Trim();
        }
        else
        {
            movie.Uuid = Guid.NewGuid().ToString();
            addId(movie.Uuid, path);
        }

        string[] movieFiles = Directory.GetFiles(path);
        bool vttExists = movieFiles.Any(f => Path.GetExtension(f).Equals(".vtt", StringComparison.OrdinalIgnoreCase));

        foreach (var file in movieFiles)
        {
            string mimeType = MimeTypes.GetMimeType(file);
            movie.Mimes = mimeType;
            string extension = Path.GetExtension(file);

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
                    movie.SubtitlePath = "no subtitle found";
                    break;
            }
        }
        return movie;
    }
    private static void addId(string uuid, string path)
    {
        File.WriteAllText(path + "/.id.txt", uuid);
    }
}
