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

builder.AddOllamaChatCompletion("llama3:8b-instruct-q4_1", httpClient);

var kernel = builder.Build();

ChatCompletionAgent agent = new()
{
    Instructions = "Answer questions about C# and .NET.",
    Name = "C# Agent",
    Kernel = kernel
};

ChatHistory chat = [
    new ChatMessageContent(AuthorRole.User, "What is the difference between a class and  a record?")
];

await foreach (var response in agent.InvokeAsync(chat))
{
    chat.Add(response);
    Console.WriteLine(response.Content);
}