using Hepsiburada.Presentation.Requests;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hepsiburada.Presentation
{
    public class App
    {
        private readonly IMediator _mediator;

        public App(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Run()
        {
            var files = Directory.GetFiles($"{Environment.CurrentDirectory}\\scenerarios").ToList();
            Console.WriteLine("Dosyalar okunuyor...");

            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".txt")
                    continue;

                Console.WriteLine($"\r\n\r\n {Path.GetFileName(file)} reading... \r\n\r\n");

                var lines = (await File.ReadAllTextAsync(file)).Split('\n');

                foreach (var line in lines)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var request = GetRequest(line);
                        if (request.IsValid())
                        {
                            var result = await request.Process();
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Parameters");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"İşlem Sırasında Bir Hata Oluştu: {ex.Message}");
                    }
                }

                Console.WriteLine("\nSenaryo çalıştırıldı. Sonraki senaryo için bir tuşa basınız...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private Request GetRequest(string line)
        {
            var lineItems = line.Split(' ');
            switch (lineItems[0])
            {
                case "create_product":
                    return new CreateProductRequest(lineItems, _mediator);
                case "create_campaign":
                    return new CreateCampaignRequest(lineItems, _mediator);
                case "create_order":
                    return new CreateOrderRequest(lineItems, _mediator);
                case "get_product_info":
                    return new GetProductInfoRequest(lineItems, _mediator);
                case "increase_time":
                    return new IncreaseTimeRequest(lineItems, _mediator);
                case "get_campaign_info":
                    return new GetCampaignInfoRequest(lineItems, _mediator);
                default:
                    throw new Exception("Invalid Command");
            }
        }
    }
}
