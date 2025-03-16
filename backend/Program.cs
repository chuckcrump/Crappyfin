using System.Diagnostics;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();
// SwaggerUI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Crappyfin Backend",
        Version = "v1",
        Description = "The main c# backend for crappyfin ASP.NET minimal api",
        Contact = new OpenApiContact
        {
            Name = "Andy",
            Url = new Uri("https://github.com/chuckcrump")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

Process.Start("../go_streaming/stream");

//app.MapGet("/list", () =>
//{
//    return "Hello there! use the TypeScript backend for now :)";
//}).WithName("Test").WithOpenApi();

app.MapMovieEndpoints();

app.Run();
