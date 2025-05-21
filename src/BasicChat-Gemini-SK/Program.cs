// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

#pragma warning disable SKEXP0070

DotNetEnv.Env.Load();

var builder = Kernel.CreateBuilder();
builder.AddGoogleAIGeminiChatCompletion(modelId: "gemini-2.0-flash", apiKey: Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? "");
var kernel = builder.Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory("You are an expert in the tool shop. You can help me to find the right tool for my needs.");
chatHistory.AddUserMessage("Hi, I'm looking for new power tools, any suggestion?");
await MessageOutputAsync(chatHistory);

var reply = await chat.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(reply);
await MessageOutputAsync(chatHistory);

chatHistory.AddUserMessage("I'm looking for a drill, a screwdriver and a hammer.");
await MessageOutputAsync(chatHistory);

reply = await chat.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(reply);
await MessageOutputAsync(chatHistory);


Task MessageOutputAsync(ChatHistory ch)
{
    var message = ch.Last();

    Console.WriteLine($"{message.Role}: {message.Content}");
    Console.WriteLine("------------------------");

    return Task.CompletedTask;
}