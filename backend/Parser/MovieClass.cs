
namespace backend.Parser;

public class MovieClass(string uuid, string name, string videoPath, string subtitlePath, string coverPath, string mimes)
{
  public string Uuid { get; set; } = uuid;
  public string Name { get; set; } = name;
  public string VideoPath { get; set; } = videoPath;
  public string SubtitlePath { get; set; } = subtitlePath;
  public string CoverPath { get; set; } = coverPath;
  public string Mimes { get; set; } = mimes;
}