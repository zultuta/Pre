using Microsoft.OpenApi.Models;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Services;
using System.Reflection;

namespace Pre.UserProjectManager.API.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void ResolveSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pre User Project Manager API",
                    Version = "v1",
                    Description = @"An API that manages projects for users",
                    Contact = new OpenApiContact
                    {
                        Name = "Zulqornain Gambari",
                        Email = "zulqornaingambari@gmail.com"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ResolveCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IProjectManagementService, ProjectManagementService>();
        }
    }
}
