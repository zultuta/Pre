namespace Pre.UserProjectManager.API.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder ConfigurSwagger(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseSwagger();

            appBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Pre User Project Manager API");
                c.RoutePrefix = string.Empty;
            });

            return appBuilder;
        }
    }
}
