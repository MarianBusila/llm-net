# llm-net
GenAI exploration in .NET using Ollama.
The samples below use Microsoft.Extensions.AI package(__MEAI__) and SemanticKernel(__SK__)


## Setup
- download and install [Ollama](https://ollama.com/download)
- pull models locally. For example:
```
ollama pull all-minilm
ollama pull mistral
```

## Samples

- [BasicChat-Ollama-MEAI](./src/BasicChat-Ollama-MEAI/) - Sample for a chat, using MEAI
- [BasicChat-Ollama-SK](./src/BasicChat-Ollama-SK/) - Sample for a chat, using SK
- [BasicChat-Gemini-SK](./src/BasicChat-Gemini-SK/) - Sample for a chat, using SK and Google Gemini
- [Functions-Ollama-MEAI](./src/Functions-Ollama-MEAI/) - Sample for function calling using MEAI
- [RAG-InMemory-Ollama-MEAI](./src/RAG-InMemory-Ollama-MEAI/) - Sample for RAG with InMemory Vector Store using MEAI
- [RAG-InMemory-Ollama-SK](./src/RAG-InMemory-Ollama-SK/) - Sample for RAG with InMemory Vector Store using SK
- [RAG-Qdrant-Ollama-MEAI](./src/RAG-Qdrant-Ollama-MEAI/) - Sample for RAG with Qdrant Vector Store using MEAI
- [Vision-Ollama-MEAI](./src/Vision-Ollama-MEAI/) - Sample for image analysis using MEAI and Ollama
- [Vision-OpenAI-MEAI](./src/Vision-OpenAP-MEAI/) - Sample for image analysis using MEAI and OpenAI
- [Agent-Ollama-SK](./src/Agent-Ollama-SK/) - Simple agent defined using SK
- [AgentWithFunctions-Ollama-SK](./src/AgentWithFunctions-Ollama-SK/) - SK Agent with function calling
- [MCPServer-Docker-Ollama-MEAI](./src/MCPServer-Docker-Ollama-MEAI/) - Chat client calling mcp server run via docker to get the time
- [MCPServer-Time](./src/MCPServer-Time/) and [MCPClient-Time](./src/MCPClient-Time/) - Chat client calling mcp server to get the time using [MCP csharp-sdk package](https://github.com/modelcontextprotocol/csharp-sdk)
- [OpenAPI-OpenAI-SK](./src/OpenAPI-OpenAI-SK/) - Chat client calling OpenAPI endpoints using SK
- [Agents-GroupChatWithAIManager](./src/Agents-GroupChatWithAIManager/) - Agent orchestration using an AI GroupChatManager using OpenAI. The agents debate on the "What does a good life mean to you?". The chat manager collects several viewpoints and then does a summary of the discussion.
