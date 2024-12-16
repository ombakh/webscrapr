namespace webscrapr.Scraper;
using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Write("Paste your Amazon link: ");
        var url = Console.ReadLine();

        if (!IsValidUrl(url))
        {
            Console.WriteLine("Invalid URL. Please provide a valid Amazon link.");
            return;
        }

        // Use UrlParser to extract title and create CSV file
        var urlParser = new UrlParser();
        string title = urlParser.ExtractTitleFromUrl(url);

        if (title == "Invalid URL" || title == "Unknown Title")
        {
            Console.WriteLine("Unable to extract title from the URL.");
            return;
        }

        urlParser.CreateCsvFile(title);

        // Fetch price
        string price = await GetPriceFromAmazonAsync(url);

        if (!string.IsNullOrEmpty(price))
        {
            Console.WriteLine($"Price: ${price}");
        }
        else
        {
            Console.WriteLine("Price not found.");
        }
    }

    private static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static async Task<string> GetPriceFromAmazonAsync(string url)
    {
        try
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            await page.GotoAsync(url);

            var dollarLocator = page.Locator(".a-price-whole");
            var centsLocator = page.Locator(".a-price-fraction");

            string dollars = await GetTextFromLocatorAsync(dollarLocator);
            string cents = await GetTextFromLocatorAsync(centsLocator);

            await browser.CloseAsync();

            if (!string.IsNullOrEmpty(dollars) && !string.IsNullOrEmpty(cents))
            {
                return $"{dollars}{cents}";
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
            return string.Empty;
        }
    }

    private static async Task<string> GetTextFromLocatorAsync(ILocator locator)
    {
        if (await locator.CountAsync() > 0)
        {
            string text = await locator.First.InnerTextAsync();
            return text.Trim().Replace("\n", "").Replace("\r", "");
        }
        return string.Empty;
    }
}