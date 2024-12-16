using Microsoft.Playwright;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Write("Paste your Amazon link: ");
        var url = Console.ReadLine();
        // start playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        // goes to url
        await page.GotoAsync(url);
        // searches page for dollars and cents
        var dollarLocator = page.Locator(".a-price-whole");
        var centsLocator = page.Locator(".a-price-fraction");

        string dollars = string.Empty;
        string cents = string.Empty;
        
        
        
        // action handling
        if (await dollarLocator.CountAsync() > 0)
        {
            dollars = await dollarLocator.First.InnerTextAsync();
            dollars = await dollarLocator.First.InnerTextAsync();
            dollars = dollars.Trim(); // Remove leading/trailing whitespace
            dollars = dollars.Replace("\n", "").Replace("\r", ""); // Remove inline newlines and dollar sign
        }

        if (await centsLocator.CountAsync() > 0)
        {
            cents = await centsLocator.First.InnerTextAsync();
            cents = await centsLocator.First.InnerTextAsync();
            cents = cents.Trim(); // Remove leading/trailing whitespace
            cents = cents.Replace("\n", "").Replace("\r", ""); // Remove inline newlines
        }

        if (!string.IsNullOrEmpty(dollars))
        {
            dollars = dollars.Trim();
            cents = cents.Trim();
            Console.WriteLine($"Price: ${dollars}{cents}");
        }
        else
        {
            Console.WriteLine("Price not found.");
        }
        await browser.CloseAsync();
    }
}