namespace webscrapr.Scraper;

using System;
using System.IO;

public class UrlParser
{
    public string ExtractTitleFromUrl(string url)
    {
        try
        {
            // Extract the part after "amazon.com/"
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Look for the part that matches the title (likely the 3rd segment after "amazon.com")
            foreach (var segment in segments)
            {
                if (!segment.StartsWith("dp") && !segment.StartsWith("gp"))
                {
                    return segment.Replace("-", " "); // Convert hyphens to spaces for readability
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
            string fileName = $"{title.Replace(" ", "_")}.csv";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            // Write a header row to the CSV file
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