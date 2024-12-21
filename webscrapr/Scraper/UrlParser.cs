namespace webscrapr.Scraper;

using System;
using System.IO;

public class UrlParser
{
    public string ExtractTitleFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                if (!segment.StartsWith("dp") && !segment.StartsWith("gp"))
                {
                    return segment.Replace("-", " "); // hypens -> spaces
                }
            }

            return "Unknown Title";
        }
        catch (Exception)
        {
            return "Invalid URL";
        }
    }

    public void CreateCsvFile(string title)
    {
        string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string targetDirectory = Path.Combine(homeDirectory, "webscrapr", "Scraper", "csv");

        try
        {
            if (string.IsNullOrWhiteSpace(title) || title == "Unknown Title" || title == "Invalid URL")
            {
                Console.WriteLine("Invalid or unknown title. Cannot create CSV file.");
                return;
            }

            // sanitize title for file name
            string sanitizedTitle = string.Concat(title.Split(Path.GetInvalidFileNameChars()));
            string fileName = $"{sanitizedTitle.Replace(" ", "_")}.csv";
            string filePath = Path.Combine(targetDirectory, fileName);

            // ensure directory exists
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // write header
            using var writer = new StreamWriter(filePath);
            writer.WriteLine("Date,Time,Price");

            Console.WriteLine($"CSV file '{fileName}' created successfully at {filePath}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating CSV file: {ex.Message}");
        }
    }
}
