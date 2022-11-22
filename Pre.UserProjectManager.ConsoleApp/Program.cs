using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pre.UserProjectManager.ConsoleApp;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Services;
using Pre.UserProjectManager.Infrastructure.Extension;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggin =>
                {
                    loggin.ClearProviders();
                })
                .ConfigureServices((_, services) =>
                    services.AddScoped<IAuthenticationService, AuthenticationService>()
                    .AddScoped<IProjectManagementService, ProjectManagementService>()
                    .ResolveInfrastructureServices(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")))
                .Build();

var _authService = ActivatorUtilities.CreateInstance<AuthenticationService>(host.Services);
var _projectService = ActivatorUtilities.CreateInstance<ProjectManagementService>(host.Services);

Console.WriteLine("Welcome. Please provide your login details to Proceed");
Worker worker = new();
await worker.LoginAsync(_authService);
await worker.GetAssignedProjectsAsync(_projectService);
