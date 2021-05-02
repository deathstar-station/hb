using System.Reflection;
using FluentValidation;
using Hepsiburada.Application.Campaign.Commands.CreateCampaign;
using Hepsiburada.Application.Campaign.Queries.GetCampaignInfo;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Order.Commands.CreateOrder;
using Hepsiburada.Application.Product.Commands.CreateProduct;
using Hepsiburada.Application.Product.Queries.GetProductInfo;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Hepsiburada.RepositoryImpl;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hepsiburada.Presentation
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly)

             .AddTransient<IRepository<ProductEntity>, ProductRepository>()
             .AddTransient<IRepository<CampaignEntity>, CampaignRepository>()
             .AddTransient<IRepository<OrderEntity>, OrderRepository>()

             .AddTransient<IValidator<CreateProductCommand>, CreateProductValidator>()
             .AddTransient<IValidator<GetProductInfoQuery>, GetProductInfoValidator>()
             .AddTransient<IValidator<CreateCampaignCommand>, CreateCampaignValidator>()
             .AddTransient<IValidator<GetCampaignInfoQuery>, GetCampaignInfoValidator>()
             .AddTransient<IValidator<CreateOrderCommand>, CreateOrderValidator>()

             .AddSingleton<ITimeService, TimeService>()
             .AddSingleton<IProductPriceService, ProductPriceService>()
             .AddSingleton<ICreateOrderService, CreateOrderService>()

             .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))

            .AddTransient<App>()
            .BuildServiceProvider();
        }
    }
}