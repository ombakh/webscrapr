using Microsoft.Playwright;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Write("Paste your Amazon link: ");
        var url = Console.ReadLine();
        // Start Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        // Navigate to the URL
        await page.GotoAsync(url);

        // Locate the dollar and cents parts of the price
        var dollarLocator = page.Locator(".a-price-whole");
        var centsLocator = page.Locator(".a-price-fraction");

        string dollars = string.Empty;
        string cents = string.Empty;

        if (await dollarLocator.CountAsync() > 0)
        {
            dollars = await dollarLocator.First.InnerTextAsync();
        }

        if (await centsLocator.CountAsync() > 0)
        {
            cents = await centsLocator.First.InnerTextAsync();
        }

        if (!string.IsNullOrEmpty(dollars))
        {
            Console.WriteLine($"Price: ${dollars}{cents}");
        }
        else
        {
            Console.WriteLine("Price not found.");
        }

        await browser.CloseAsync();
    }
}