using System.ComponentModel;
using Microsoft.Extensions.AI;

IChatClient client = new OllamaChatClient(endpoint:"http://localhost:11434", modelId: "llama3.2:3b-instruct-fp16")
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();
    
ChatOptions options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetTheWeather)]
};

var question = "Solve 2 + 2. Provide an accurate and short answer.";
Console.WriteLine($"question: {question}");
var response = await client.GetResponseAsync(question, options);
Console.WriteLine($"response: {response}");

Console.WriteLine();

question = "Do I need an umbrella today?. Provide an accurate and short answer.";
Console.WriteLine($"question: {question}");
response = await client.GetResponseAsync(question, options);
Console.WriteLine($"response: {response}");

[Description("Get the weather")]
static string GetTheWeather()
{
    Console.WriteLine("\tGet the weather function called");
    var temperature = Random.Shared.Next(5, 20);
    var conditions = Random.Shared.Next(0, 2) == 0 ? "sunny" : "rainy";
    var weather = $"The weather is {temperature} degrees and {conditions}.";
    Console.WriteLine($"\tGet the weather function returned: {weather}");
    return weather;
}