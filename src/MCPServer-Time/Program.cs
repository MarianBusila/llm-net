global using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(settings:null);
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
    
await builder.Build().RunAsync();

[McpServerToolType]
public static class TimeTool
{
    [McpServerTool, Description("Get the current time for a city")]
    public static string GetCurrentTime(string city)
    {
        return $"It is {DateTime.Now.Hour}:{DateTime.Now.Minute} in {city}.";
    }
}