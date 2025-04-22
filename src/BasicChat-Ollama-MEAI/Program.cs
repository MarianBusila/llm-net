using Microsoft.Extensions.AI;
using System.Text;

IChatClient client = new OllamaChatClient(new Uri("http://localhost:11434"), "mistral");

// build the prompt
var prompt = new StringBuilder();
prompt.AppendLine("You will analyze the sentiment of the following product reviews. Each line is its own review. Output the sentiment of each review in a bulleted list including the original text and the sentiment, and then provide a general sentiment of all reviews. ");
prompt.AppendLine("I bought this product and it's amazing. I love it!");
prompt.AppendLine("This product is terrible. I hate it.");
prompt.AppendLine("I'm not sure about this product. It's okay.");
prompt.AppendLine("I found this product based on the other reviews. It worked for a bit, and then it didn't.");

// send the prompt to the model
var response = await client.GetResponseAsync(prompt.ToString());
Console.WriteLine(response.Text);
