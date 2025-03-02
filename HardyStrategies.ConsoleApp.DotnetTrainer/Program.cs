using Bogus;
using CliFx;
using HardyStrategies.ConsoleApp.DotnetTrainer.Commands;
using Microsoft.Extensions.DependencyInjection;
// See https://aka.ms/new-console-template for more information
var builder = new CliApplicationBuilder();

builder.UseTypeActivator(type =>
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddScoped<Faker>();
    
    // Register commands
    serviceCollection.AddScoped<HelloWorldCommand>();
    serviceCollection.AddScoped<IsNullTestCommand>();
    
    return serviceCollection.BuildServiceProvider();
});
builder.AddCommand<HelloWorldCommand>();
builder.AddCommand<IsNullTestCommand>();

await builder.Build().RunAsync();