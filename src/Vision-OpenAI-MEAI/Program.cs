using Microsoft.Extensions.AI;
using OpenAI;

DotNetEnv.Env.Load();

string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"); 
IChatClient chatClient = new OpenAIClient(apiKey).AsChatClient("gpt-4o-mini");

//images
string imgRunningShoes = "running-shoes.jpg";

string imagePath = Path.Combine("../../../images",  imgRunningShoes);
AIContent aic = new DataContent(File.ReadAllBytes(imagePath), "image/jpeg");

string systemPrompt = "You are a useful assistant that describes images using a direct style.";
var describePrompt = "Describe the image";
var analyzePrompt = "How many red shoes are in the picture? and what other shoes colors are there?";
var userPrompt = describePrompt;

List<ChatMessage> messages = [
    new ChatMessage(ChatRole.System, systemPrompt),
    new ChatMessage(ChatRole.User, userPrompt),
    new ChatMessage(ChatRole.User, [aic])
];

var imageAnalysis = await chatClient.GetResponseAsync(messages);
Console.WriteLine($"Response: {imageAnalysis.Text}");