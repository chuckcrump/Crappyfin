public static class MovieEndpoints
{
    private static string ?mediaPath;
    public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
    {
        DotNetEnv.Env.Load("../.env");
        mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH")!;
        
        var Movie = routes.MapGroup("/movies");
        Movie.MapGet("/list", GetMovies);
        Movie.MapGet("/previews", SendPreview);
    }
    private static IResult GetMovies()
    {
        //if (mediaPath == null) {
            List<Parser.MovieClass> movies = Parser.DirectoryTraverser.Traverse(mediaPath);
            return Results.Json(movies);
        //}
        //return Results.Text("Media Path not specified"); 
    }
    private static IResult SendPreview(HttpContext context)
    {
        string imagePath = context.Request.Query["path"]!;
        if (!File.Exists(imagePath))
        {
            return Results.NotFound("file not found");
        }
        var type = "image/jpeg";
        return Results.File(imagePath, type);
    }
}
