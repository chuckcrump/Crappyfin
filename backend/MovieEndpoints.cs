using backend.Database;
using backend.Parser;
using Microsoft.EntityFrameworkCore;

namespace backend;
public static class MovieEndpoints
{
    private record StreamUrl(string Url = "http://localhost:8081/go/stream/STREAMTOKEN");
    public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
    {
        DotNetEnv.Env.Load("../.env");

        var movie = routes.MapGroup("/movies");
        movie.MapGet("/list", GetMovies).WithName("List of movies").Produces<MovieClass>(200, "application/json");
        movie.MapGet("/preview/{uuid}/cover", Preview).Accepts<string>("0419cde2-7e8f-4942-960a-b41bff94b98a");
        movie.MapGet("/download/{uuid}", Download);
    }
    private static async Task<IResult> GetMovies()
    {
        await using var context = new MovieDbContext();
        var mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
        await DirectoryTraverser.Traverse(mediaPath!);
        var movies = await context.Movies.ToListAsync();
        // For future use
        //var movies = context.Movies.Select(m => new { m.Name, m.Uuid }).ToList();
        return movies.Count == 0 ? Results.Text("Media Path not specified") : Results.Json(movies);
    }
    private static async Task<IResult> Preview(string uuid)
    {
        await using var context = new MovieDbContext();
        var selectedMovie = await context.Movies.FirstOrDefaultAsync(m => m.Uuid == uuid);
        if (selectedMovie == null || !File.Exists(selectedMovie!.CoverPath))
        {
            return Results.NotFound("cover not found" + selectedMovie!.CoverPath);
        }
        const string type = "image/jpeg";
        return Results.File(selectedMovie.CoverPath, type);
    }
    private static async Task<IResult> Download(string uuid)
    {
        await using var context = new MovieDbContext();
        var moviePath = await context.Movies.FirstOrDefaultAsync(m => m.Uuid == uuid);
        if (moviePath == null || !File.Exists(moviePath.VideoPath))
        {
            return Results.NotFound("Video not found");
        }
        string name = Path.GetFileName(moviePath.VideoPath);
        const string type = "application/octet-stream";
        var bytes = new FileStream(moviePath.VideoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Results.File(bytes, type, name);
    }
}
