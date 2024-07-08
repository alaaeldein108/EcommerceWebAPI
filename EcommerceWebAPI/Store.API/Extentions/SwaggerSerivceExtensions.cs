using Microsoft.OpenApi.Models;

namespace Store.API.Extentions
{
    public static class SwaggerSerivceExtensions
    {
        public static IServiceCollection AddSwaggerDocumention(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme="bearar",
                    Reference=new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="bearar",
                    }

                };
                options.AddSecurityDefinition("bearar", securitySchema);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema,new[]{ "bearar"} }
                });
            });
            return services;
        }
    }
}
