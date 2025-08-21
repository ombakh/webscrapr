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
        try
        {
            if (string.IsNullOrWhiteSpace(title) || title == "Unknown Title" || title == "Invalid URL")
            {
                Console.WriteLine("Invalid or unknown title. Cannot create CSV file.");
                return;
            }

            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string targetDirectory = Path.Combine(homeDirectory, "RiderProjects", "webscrapr", "webscrapr", "Scraper", "csv");
        
            Console.WriteLine($"Target directory: {targetDirectory}");

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
                Console.WriteLine("Target directory created.");
            }

            string sanitizedTitle = string.Concat(title.Split(Path.GetInvalidFileNameChars()));
            if (string.IsNullOrWhiteSpace(sanitizedTitle))
            {
                Console.WriteLine("Sanitized title is empty or invalid.");
                return;
            }

            string fileName = $"{sanitizedTitle.Replace(" ", "_")}.csv";
            string filePath = Path.Combine(targetDirectory, fileName);
            Console.WriteLine($"File path: {filePath}");
            
            if (File.Exists(filePath) == false)
            {
                using var writer = new StreamWriter(filePath, append: false);
                writer.WriteLine("Date,Time,Price");
                Console.WriteLine($"CSV file '{fileName}' created successfully at {filePath}.");
            }
            else
            {
                Console.WriteLine($"CSV file '{fileName}' already exists. Skipping creation.");
                using var writer = new StreamWriter(filePath, append: false);
                writer.WriteLine("Date,Time,Price");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating CSV file: {ex}");
        }
    }
}
