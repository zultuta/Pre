
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pre.UserProjectManager.API.Extensions;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Infrastructure.Extension;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// we can typically add the required logger to the host builder. Serilog is a good logging library for .Net
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ResolveCoreServices();
builder.Services.ResolveSwagger();

builder.Services.ResolveInfrastructureServices(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"));

var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(x =>
 {
     x.RequireHttpsMetadata = false;
     x.SaveToken = true;
     x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = false,
         ValidateAudience = false
     };
 });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.ConfigurSwagger();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
