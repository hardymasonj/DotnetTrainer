using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Bogus;
using CliFx.Exceptions;

namespace HardyStrategies.ConsoleApp.DotnetTrainer.Commands;

[Command("is-null-test")]
public class IsNullTestCommand(Faker faker) : ICommand
{
    private record NullTestQuestion
    {
        public required string Prompt { get; init; }
        public required string CorrectAnswer { get; init; }
        public required string Response { get; init; }
    }

    [CommandOption("rounds")] public int Rounds { get; init; } = 10;

    private List<NullTestQuestion> Prompts { get; init; } = [];

    public async ValueTask ExecuteAsync(IConsole console)
    {
        if (Rounds <= 0)
            throw new CliFxException("Invalid input: Rounds must be greater than zero", 400);
        for (var i = 0; i < Rounds; i++)
        {
            console.Clear();
            (string question, string expected) = GetQuestion();
            using (var greenConsole = console.WithForegroundColor(ConsoleColor.Green))
            {
                await console.Output.WriteLineAsync(question);
            }

            var response = await console.Input.ReadLineAsync();
            Prompts.Add(new()
            {
                Prompt = question,
                CorrectAnswer = expected,
                Response = response ?? ""
            });
        }

        var incorrect = Prompts.Where(p => p.Response != p.CorrectAnswer).ToArray();

        await console.Output.WriteLineAsync($"Total correct: {Rounds - incorrect.Length}");
        foreach (var question in incorrect)
        {
            await console.Output.WriteLineAsync();
            using (console.WithForegroundColor(ConsoleColor.Red))
            {
                await console.Output.WriteLineAsync(question.Prompt);
            }
            await console.Output.WriteLineAsync($"Your answer:      {question.Response}");
            await console.Output.WriteLineAsync($"Correct answer:   {question.CorrectAnswer}");
        }
    }

    private (string question, string expected) GetQuestion()
    {
        var variableName = faker.Database.Column();
        string[] operators = ["==", "!=", "is", "is not"];
        var equality = faker.PickRandom(operators);
        if (equality is null)
            throw new CliFxException("Bogus Exception", 500);
        var question = $"{variableName} {equality} null;";
        var expected = $"{variableName} {
            equality switch {
                "==" => "is",
                "!=" => "is not",
                "is" => "==",
                "is not" => "!=",
                _ => throw new CliFxException("Bogus Exception", 500)
            }
        } null;";
        return (question, expected);
    }
}