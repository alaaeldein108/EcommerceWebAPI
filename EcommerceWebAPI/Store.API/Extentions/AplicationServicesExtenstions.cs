using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductService.Dtos;
using Store.Service.Services.ProductService;
using System.Runtime.CompilerServices;
using Store.Service.HandleResponse;
using Store.Service.CasheService;
using Store.Repository.BasketRepository;
using Store.Service.BasketService.Dtos;
using Store.Service.BasketService;
using Store.Service.Services.TokenServices;
using Store.Service.Services.TokenService;
using Store.Service.UserService;
using Store.Service.OrderService.Dtos;
using Store.Service.PaymentService;
using Store.Service.OrderService;

namespace Store.API.Extentions
{
    public static class AplicationServicesExtenstions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICasheService, CasheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                .Where(model => model.Value.Errors.Count > 0)
                                .SelectMany(model => model.Value.Errors)
                                .Select(error => error.ErrorMessage).ToList();
                    var errorRespnse = new ValidationErrorRespons
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorRespnse);
                };
            });
            return services;

        }
    }
}
