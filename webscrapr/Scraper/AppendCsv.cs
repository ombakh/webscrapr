namespace webscrapr.Scraper;
using System;
using System.IO;

public class AppendCsv
{
    public static void Append(string filePath, string data)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist. Creating a new CSV file...");
            File.WriteAllText(filePath, "Date,Time,Price\n");
        }

        using (StreamWriter writer = new StreamWriter(filePath, append: true));
    }
}