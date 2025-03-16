using Microsoft.EntityFrameworkCore;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
    {
        DotNetEnv.Env.Load("../.env");

        var Movie = routes.MapGroup("/movies");
        Movie.MapGet("/list", GetMovies);
        Movie.MapGet("/preview/{uuid}/cover", Preview);
        Movie.MapGet("/stream/{uuid}", StartStream);
    }
    private static async Task<IResult> GetMovies()
    {
        //if (mediaPath == null) {
        using (var context = new MovieDbContext())
        {
            string mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
            await Parser.DirectoryTraverser.Traverse(mediaPath!);
            List<Parser.MovieClass> movies = await context.Movies.ToListAsync();
            //var movies = context.Movies.Select(m => new { m.Name, m.Uuid }).ToList();
            if (movies.Count == 0) {
                return Results.Text("Media Path not specified");
            }
            return Results.Json(movies);
        }
        //}
    }
    private static async Task<IResult> Preview(string uuid)
    {
        using (var context = new MovieDbContext())
        {
            var selectedMovie = await context.Movies.FirstOrDefaultAsync(m => m.Uuid == uuid);
            if (selectedMovie == null || !File.Exists(selectedMovie!.CoverPath))
            {
                return Results.NotFound("cover not found" + selectedMovie!.CoverPath);
            }
            var type = "image/jpeg";
            return Results.File(selectedMovie.CoverPath, type);
        }
    }
    private static async Task<IResult> StartStream(string uuid)
    {
        using (var context = new MovieDbContext())
        {
            var match = await context.Movies.FirstOrDefaultAsync(m => m.Uuid == uuid);
            if (match == null || !File.Exists(match.VideoPath))
            {
                return Results.NotFound("movie file not found");
            }
            return Results.Text("hi this is not complete");
        }
    }
}
