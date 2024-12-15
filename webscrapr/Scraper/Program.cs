using Microsoft.Playwright;

class Program
{
    public static async Task Main(string[] args)
    {
        var url = "https://www.amazon.com/Mens-Y-Eau-Parfum-3-3-oz/dp/B07GB6B37N/?_encoding=UTF8&pd_rd_w=fPwu8&content-id=amzn1.sym.255b3518-6e7f-495c-8611-30a58648072e%3Aamzn1.symc.a68f4ca3-28dc-4388-a2cf-24672c480d8f&pf_rd_p=255b3518-6e7f-495c-8611-30a58648072e&pf_rd_r=QYNQ5S4HCBN6TN36MM5N&pd_rd_wg=PGuaS&pd_rd_r=d19fc31a-5d53-48fc-af71-f341fd796743&ref_=pd_hp_d_atf_ci_mcx_mr_ca_hp_atf_d";

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