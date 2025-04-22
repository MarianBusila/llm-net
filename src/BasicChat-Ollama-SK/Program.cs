using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

#pragma warning disable SKEXP0001

var modelId = "mistral";
var uri = "http://localhost:11434/";

var chat = new OllamaApiClient(uri, modelId).AsChatCompletionService();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply in a funny way. Use emojis if possible.");

while (true)
{
    Console.Write("Q: ");
    var userQuestion = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userQuestion))
    {
        break;
    }
    history.AddUserMessage(userQuestion);
    var sb = new StringBuilder();
    var result = chat.GetStreamingChatMessageContentsAsync(history);
    Console.Write($"AI [{modelId}]: ");
    await foreach(var item in result)
    {
        sb.Append(item);
        Console.Write(item);
    }
    Console.WriteLine();
    history.AddAssistantMessage(sb.ToString());
}