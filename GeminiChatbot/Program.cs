using System;
using System.Threading.Tasks;
using GenerativeAI.Methods;
using GenerativeAI.Models;
using GenerativeAI.Types;

namespace GeminiAIDemoConsole
{
    class Program
    {
        private static ChatSession chatSession;
        public static string apiKey = "YOUR_API_KEY"; // Replace with your API key

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Gemini AI Console Chat Application...");

            var model = new GenerativeModel(apiKey);
            chatSession = model.StartChat(new StartChatParams());

            Console.WriteLine("Chat session started. You can now send messages. Type 'exit' to end the chat.");

            while (true)
            {
                Console.Write("Me: ");
                var message = Console.ReadLine()?.Trim();

                if (message?.ToLower() == "exit")
                {
                    break;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    await SendMessageAsync(message);
                }
            }

            Console.WriteLine("Chat session ended.");
        }

        private static async Task SendMessageAsync(string message)
        {
            Console.WriteLine("Waiting for response...");

            var result = await chatSession.SendMessageAsync(message);

            Console.WriteLine($"Gemini AI: {result}\n");
        }
    }
}
