using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Agents_GroupChatWithAIManager;

#pragma warning disable SKEXP0110
#pragma warning disable SKEXP0070

public sealed class AIGroupChatManager : GroupChatManager
{
    private readonly string _topic;
    private readonly IChatCompletionService _chatCompletion;
    
    private static class Prompts
    {
        public static string Termination(string topic) =>
            $"""
             You are mediator that guides a discussion on the topic of '{topic}'. 
             You need to determine if the discussion has reached a conclusion. 
             If you would like to end the discussion, please respond with True. Otherwise, respond with False.
             """;

        public static string Selection(string topic, string participants) =>
            $"""
             You are mediator that guides a discussion on the topic of '{topic}'. 
             You need to select the next participant to speak. 
             Here are the names and descriptions of the participants: 
             {participants}\n
             Please respond with only the name of the participant you would like to select.
             """;

        public static string Filter(string topic) =>
            $"""
             You are mediator that guides a discussion on the topic of '{topic}'. 
             You have just concluded the discussion. 
             Please summarize the discussion and provide a closing statement.
             """;
    }
    
    public AIGroupChatManager(string topic,
        IChatCompletionService chatCompletion)
    {
        _topic = topic;
        _chatCompletion = chatCompletion;
    }
    
    public override ValueTask<GroupChatManagerResult<string>> FilterResults(ChatHistory history, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.Write("[FILTER RESULTS]: ");
        return this.GetResponseAsync<string>(history, Prompts.Filter(_topic), cancellationToken);
    }

    public override ValueTask<GroupChatManagerResult<string>> SelectNextAgent(ChatHistory history, GroupChatTeam team,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Console.Write("[SELECT NEXT AGENT]: ");
        return this.GetResponseAsync<string>(history, Prompts.Selection(_topic, team.FormatList()), cancellationToken);
    }

    public override ValueTask<GroupChatManagerResult<bool>> ShouldRequestUserInput(ChatHistory history, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine("[SHOULD REQUEST USER INPUT]");
        return ValueTask.FromResult(new GroupChatManagerResult<bool>(false) {Reason = "The AI group chat manager does not request user input."});
    }
    
    public override async ValueTask<GroupChatManagerResult<bool>> ShouldTerminate(ChatHistory history, CancellationToken cancellationToken = default)
    {
        Console.Write("[SHOULD TERMINATE]: ");
        GroupChatManagerResult<bool> result = await base.ShouldTerminate(history, cancellationToken);
        if (!result.Value)
        {
            result = await this.GetResponseAsync<bool>(history, Prompts.Termination(_topic), cancellationToken);
        }
        return result;
    }
    
    private async ValueTask<GroupChatManagerResult<TValue>> GetResponseAsync<TValue>(ChatHistory history, string prompt, CancellationToken cancellationToken = default)
    {
        /*
        var executionSettings = new GeminiPromptExecutionSettings
        {
            ResponseMimeType = "application/json",
            ResponseSchema = typeof(GroupChatManagerResult<TValue>)
        };
        if (history.Count == 0)
        {
            // gemini requires a user message to start the conversation
            history.AddUserMessage("As a mediator, you are responsible for guiding the discussion.");
        }
        */
        OpenAIPromptExecutionSettings executionSettings = new() { ResponseFormat = typeof(GroupChatManagerResult<TValue>) };
        ChatHistory request = [.. history, new ChatMessageContent(AuthorRole.System, prompt)];
        //int sleepSeconds = 20;
        //Console.WriteLine($"Waiting {sleepSeconds} seconds before sending request to avoid Gemini free quota limits...");
        //Thread.Sleep(TimeSpan.FromSeconds(sleepSeconds));
        ChatMessageContent response = await _chatCompletion.GetChatMessageContentAsync(request, executionSettings, kernel: null, cancellationToken);
        string responseText = response.ToString();
        Console.WriteLine(responseText);
        return
            JsonSerializer.Deserialize<GroupChatManagerResult<TValue>>(responseText) ??
            throw new InvalidOperationException($"Failed to parse response: {responseText}");
    }    
}