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
- [Functions-Ollama-MEAI](./src/Functions-Ollama-MEAI/) - Sample for function calling using MEAI
- [RAG-InMemory-Ollama-MEAI](./src/RAG-InMemory-Ollama-MEAI/) - Sample for RAG with InMemory Vector Store using MEAI
- [RAG-InMemory-Ollama-SK](./src/RAG-InMemory-Ollama-SK/) - Sample for RAG with InMemory Vector Store using SK
- [RAG-Qdrant-Ollama-MEAI](./src/RAG-Qdrant-Ollama-MEAI/) - Sample for RAG with Qdrant Vector Store using MEAI
