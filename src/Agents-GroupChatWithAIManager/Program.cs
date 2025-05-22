// See https://aka.ms/new-console-template for more information

using Agents_GroupChatWithAIManager;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;

#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0110
#pragma warning disable SKEXP0001

DotNetEnv.Env.Load();

var builder = Kernel.CreateBuilder();
//builder.AddGoogleAIGeminiChatCompletion(modelId: "gemini-2.0-flash", apiKey: Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? "");
builder.AddOpenAIChatCompletion(modelId: "gpt-4o-mini", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "");

var kernel = builder.Build();

// define the agents
ChatCompletionAgent farmer = new ChatCompletionAgent
{
    Name = "Farmer",
    Description = "A rural farmer from Southeast Asia.",
    Instructions =
    """
    You're a farmer from Southeast Asia. 
    Your life is deeply connected to land and family. 
    You value tradition and sustainability. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
};
    
ChatCompletionAgent developer = new ChatCompletionAgent
{
    Name = "Developer",
    Description = "An urban software developer from the United States.",
    Instructions =
    """
    You're a software developer from the United States. 
    Your life is fast-paced and technology-driven. 
    You value innovation, freedom, and work-life balance. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
}; 
    
ChatCompletionAgent teacher = new ChatCompletionAgent
{
    Name = "Teacher",
    Description = "A retired history teacher from Eastern Europe",
    Instructions =
    """
    You're a retired history teacher from Eastern Europe. 
    You bring historical and philosophical perspectives to discussions. 
    You value legacy, learning, and cultural continuity. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
};
    
ChatCompletionAgent activist = new ChatCompletionAgent
{
    Name = "Activist",
    Description = "A young activist from South America.",
    Instructions =
    """
    You're a young activist from South America. 
    You focus on social justice, environmental rights, and generational change. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
};    
    
ChatCompletionAgent immigrant = new ChatCompletionAgent
{
    Name = "Immigrant",
    Description = "An immigrant entrepreneur from Asia living in Canada.",
    Instructions = 
    """
    You're an immigrant entrepreneur from Asia living in Canada. 
    You balance trandition with adaption. 
    You focus on family success, risk, and opportunity. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
};

ChatCompletionAgent doctor = new ChatCompletionAgent
{
    Name = "Doctor",
    Description = "A doctor from the United Kingdom.",
    Instructions =
    """
    You're a doctor from the United Kingdom. 
    You focus on health, well-being, and community service. 
    You are in a debate. Feel free to challenge the other participants with respect.
    """,
    Kernel = kernel,
};

const string topic = "What does a good life mean to you personally?";
var chatManager = new AIGroupChatManager(topic, kernel.GetRequiredService<IChatCompletionService>())
{
    MaximumInvocationCount = 3,
};

GroupChatOrchestration orchestration = new GroupChatOrchestration(
    chatManager,
    farmer,
    developer,
    teacher,
    activist,
    immigrant,
    doctor
)
{
    ResponseCallback = (result) =>
    {
        Console.WriteLine($"[{result.AuthorName}]: {result.Content}");
        return ValueTask.CompletedTask;
    }
};

// Start the runtime
InProcessRuntime runtime = new();
await runtime.StartAsync();  

// Run the orchestration
Console.WriteLine($"\n# INPUT: {topic}\n");
OrchestrationResult<string> result = await orchestration.InvokeAsync(topic, runtime);

string text = await result.GetValueAsync(TimeSpan.FromSeconds(600));
Console.WriteLine($"\n# RESULT: {text}");

await  runtime.RunUntilIdleAsync();

