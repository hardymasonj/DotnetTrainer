using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace HardyStrategies.ConsoleApp.DotnetTrainer.Commands;

[Command("hello-world",  Description = "Prints Hello World like a noob")]
public class HelloWorldCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        console.Output.WriteLine("Hello World!");
        return default;
    }
}