namespace backend.Parser;

public static class SrtParser
{
    public static string Parser(string filePath)
    {
        List<string> updatedLines = [];
        var lines = File.ReadLines(filePath);
        foreach (var line in lines)
        {
            if (line.Contains("-->"))
            {
                var newLine = line.Replace(",", ".");
                updatedLines.Add(newLine);
            }
            else
            {
                updatedLines.Add(line);
            }
        }

        using (var sw = new StreamWriter(filePath + ".vtt"))
        {
            sw.Write("WEBVTT\n\n");
            foreach (var line in updatedLines)
            {
                sw.WriteLine(line);
            }
            Console.WriteLine("Complete!");
        }
        return filePath + ".vtt";
    }
}
