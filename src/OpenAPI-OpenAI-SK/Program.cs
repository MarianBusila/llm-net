using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.OpenApi;

#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0040

Console.WriteLine("Hello, Semantic Kernel, OpenAI and OpenAPI!");

DotNetEnv.Env.Load();
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "");
builder.Services.AddLogging(logging =>
    logging.AddConsole().SetMinimumLevel(LogLevel.Trace));
var kernel = builder.Build();

HttpClient httpClient = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(5)
};

var operationsToInclude = new List<string>
{
    "alerts_active_count",      // /alerts/active/count
    "icons_summary",          // /icons
};

static async Task<object?> ReadLdJsonAsync(
    HttpResponseContentReaderContext context,
    CancellationToken cancellationToken)
{
    var mediaType = context.Response.Content.Headers.ContentType?.MediaType;
    // 2. Detect JSON-LD and read as string
    if (string.Equals(mediaType, "application/ld+json", StringComparison.OrdinalIgnoreCase))
    {
        return await context.Response.Content
            .ReadAsStringAsync(cancellationToken);
    }
    // 3. Fallback to default for everything else
    return null;
}

var executionParams = new OpenApiFunctionExecutionParameters
{
    HttpClient = httpClient,
    EnablePayloadNamespacing = true,
    OperationSelectionPredicate = (context) => operationsToInclude.Contains(context.Id!),
    HttpResponseContentReader = ReadLdJsonAsync // need a custom reader for application/ld+json
};

await kernel.ImportPluginFromOpenApiAsync(
    pluginName: "weatherforecast",
    uri: new Uri("https://api.weather.gov/openapi.json"),
    executionParameters: executionParams);

// list the functions
foreach (var plugin in kernel.Plugins)
{
    Console.WriteLine($"Plugin: {plugin.Name}");
    foreach (var function in plugin)
    {
        Console.WriteLine($"  Function: {function.Name}, {function.Description}");
    }
}


var chatService = kernel.GetRequiredService<IChatCompletionService>();
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

ChatHistory chat = new("""
                       You are an AI assistant that provides weather alerts information using tool calling.
                       You reply with a short message. If you cannot call the tool to get the information,
                       you should reply with a message that explains why you cannot.
                       """);

// chat.AddUserMessage("Can you give me a description for all possible weather icons?");
chat.AddUserMessage("Can you give me the number of active alerts in Florida?");
var messages = await chatService.GetChatMessageContentsAsync(chat, settings, kernel);

foreach (var message in messages)
{
    Console.Write(message);
}

Console.WriteLine();
