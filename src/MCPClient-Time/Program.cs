using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

Console.WriteLine("Hello, official MCP csharp-sdk and MCP Server!");

var message = "What is the current (CET) time in Paris, France?";
Console.WriteLine(message);

McpClientOptions options = new()
{
    ClientInfo = new() { Name = "Time Client", Version = "1.0.0"},
};

using var factory =
    LoggerFactory.Create(builder => builder.AddConsole()
        .SetMinimumLevel(LogLevel.Trace));

var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Time",
    Command =  @"..\..\..\..\MCPServer-Time\bin\Debug\net8.0\MCPServer-Time.exe"
});

await using var mcpClient = 
    await McpClientFactory.CreateAsync(clientTransport, options, loggerFactory: factory);
    
var ollamaChatClient =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2:3b-instruct-fp16");
var client = new ChatClientBuilder(ollamaChatClient)
    .UseLogging(factory)
    .UseFunctionInvocation()
    .Build();
    
IList<ChatMessage> messages =
[
    new(ChatRole.System, """
                         You are a helpful assistant delivering time in one sentence
                         in a short format, like 'It is 10:08 in Paris, France.'
                         """),
    new(ChatRole.User, message)
];

var mcpTools = await mcpClient.ListToolsAsync();
foreach (var tool in mcpTools)
{
    Console.WriteLine($"Tool: {tool.Name}, Description: {tool.Description}");
}

var response =
    await client.GetResponseAsync(
        messages,
        new ChatOptions { Tools = [.. mcpTools] });
        
Console.WriteLine(response);        