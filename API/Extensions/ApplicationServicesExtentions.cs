using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace API.Extensions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Adding ProductRepository service
            services.AddScoped<IProductRepository, ProductRepository>();

            //Adding GenericRepository service
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Adding basket service
            services.AddScoped<IBasketRepository, BasketRepository>();

            //Adding Token service
            services.AddScoped<ITokenService, TokenService>();

            //Adding Order service
            services.AddScoped<IOrderService, OrderService>();

            //Adding Unit of work Service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //override controller behavior
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });


            return services;
        }
    }
}
