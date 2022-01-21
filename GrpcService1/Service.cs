using PuppeteerSharp;
using PuppeteerSharp.Media;
using Razor.Templates.ViewModels;
using Razor.Templating.Core;

namespace GrpcService1
{
    public class Service
    {
        public Service()
        {
            Console.WriteLine("Hello, World!");

            var model = new InvoiceViewModel
            {
                CreatedAt = DateTime.Now,
                Due = DateTime.Now.AddDays(10),
                Id = 12533,
                AddressLine = "Jumpy St. 99",
                City = "Trampoline",
                ZipCode = "22-113",
                CompanyName = "Jumping Rabbit Co.",
                PaymentMethod = "Check",
                Items = new List<InvoiceItemViewModel>
                {
                    new InvoiceItemViewModel("Website design", 621.99m),
                    new InvoiceItemViewModel("Website creation", 1231.99m)
                }
            };

            Task.Run(async () =>
            {
                var html = await RazorTemplateEngine.RenderAsync("~/Views/Invoice.cshtml", model);

                Console.WriteLine(html);

                using (StreamWriter outputFile = new(Path.Combine("Invoice.html")))
                {
                    outputFile.WriteLine(html);
                }

                using var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();
                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                await using var page = await browser.NewPageAsync();
                await page.EmulateMediaTypeAsync(MediaType.Screen);
                await page.SetContentAsync(html);
                await page.PdfAsync("Invoice.pdf", new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true
                });
            });
        }
    }
}
