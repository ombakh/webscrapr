using HtmlAgilityPack;

namespace Scraper;

public class Program
{
    public static async Task Main(string[] args)
    {
        var url = "https://www.wikipedia.org/wiki/OpenAI";
        var httpClient = new HttpClient(); // gets http requests
        var html = await httpClient.GetStringAsync(url); // await = wait for task to complete
        
        var htmlDocument = new HtmlDocument(); // new html doc
        htmlDocument.LoadHtml(html); // loads html doc
        
        var divs = htmlDocument.DocumentNode.Descendants("div")
            .Where(node => node.GetAttributeValue("class", "")
                .Contains("mw-parser-output")).ToList();

        foreach (var div in divs) 
        {
            Console.WriteLine(div.InnerText.Trim());
        }
        
        
    }
}
