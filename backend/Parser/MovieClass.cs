using Microsoft.AspNetCore.SignalR.Protocol;

namespace Parser;

public class MovieClass(string name, string videoPath, string subtitlePath, string coverPath, string mimes)
{
  public string Name { get; set; } = name;
  public string VideoPath { get; set; } = videoPath;
  public string SubtitlePath { get; set; } = subtitlePath;
  public string CoverPath { get; set; } = coverPath;
  public string Mimes { get; set; } = mimes;
}