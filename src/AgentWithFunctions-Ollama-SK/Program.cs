using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

#pragma warning disable SKEXP0070
var builder = Kernel.CreateBuilder();

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434"),
    Timeout = TimeSpan.FromMinutes(5)
};

builder.AddOllamaChatCompletion("llama3.2:3b-instruct-fp16", httpClient);

var kernel = builder.Build();

ChatCompletionAgent agent = new()
{
    Instructions = """
                   Answer questions about different locations.
                   For France, use the time format HH:MM.
                   HH goes from 00 to 23, MM goes from 00 to 59.
                   """,
    Name = "Location Agent",
    Kernel = kernel,
    Arguments = new KernelArguments(new PromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    })
};

// define a time tool plugin
var plugin = KernelPluginFactory.CreateFromFunctions(
    "Time", 
    "Get the current time for a city",
    [KernelFunctionFactory.CreateFromMethod(GetCurrentTime)]);
agent.Kernel.Plugins.Add(plugin);

ChatHistory chat = [
    new ChatMessageContent(AuthorRole.User, "What time is it in Paris, France?")
];

await foreach (var response in agent.InvokeAsync(chat))
{
    chat.Add(response);
    Console.WriteLine(response.Content);
}

[Description("Get the current time for a city")]
string GetCurrentTime(string city) =>
    $"It is {DateTime.Now.Hour}:{DateTime.Now.Minute} in {city}.";