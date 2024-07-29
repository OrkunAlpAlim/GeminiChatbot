using System;
using System.IO;
using System.Threading.Tasks;
using GenerativeAI.Classes;
using GenerativeAI.Methods;
using GenerativeAI.Models;
using GenerativeAI.Services;
using GenerativeAI.Types;

namespace GeminiAIDemoConsole
{
    class Program
    {
        private static ChatSession chatSession;
        private static readonly string apiKey = "YOUR_API_KEY"; // Replace with your API key

        static async Task Main(string[] args)
        {
            Console.WriteLine("System: Starting Gemini Chatbot...");
            var model = new GenerativeModel(apiKey);
            chatSession = model.StartChat(new StartChatParams());
            Console.WriteLine("System: Chat session started. You can now send messages. Type 'exit' to end the program.");
            Console.WriteLine("System: Type 'upload <path_to_image>' to upload an image.");

            while (true)
            {
                Console.Write("Me: ");
                var message = Console.ReadLine()?.Trim();
                if (message?.ToLower() == "exit")
                {
                    break;
                }
                else if (!string.IsNullOrEmpty(message))
                {
                    if (message.StartsWith("upload "))
                    {
                        await HandleImageUploadAsync(message);
                    }
                    else
                    {
                        await SendMessageAsync(message);
                    }
                }
            }
            Console.WriteLine("System: Chat session ended...");
        }

        private static async Task HandleImageUploadAsync(string message)
        {
            Console.WriteLine("System: Waiting for response...");
            var imgPath = message.Substring(7);
            var imageBytes = await File.ReadAllBytesAsync(imgPath);
            Console.WriteLine("Gemini AI: Image read. What do you want to do with this image?");
            Console.Write("Me: ");
            var prompt = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine("System: Waiting for response...");
                var visionModel = new Gemini15Pro(apiKey);
                var result = await visionModel.GenerateContentAsync(prompt, new FileObject(imageBytes, imgPath));
                Console.WriteLine(result.Text());
            }
        }

        private static async Task SendMessageAsync(string message)
        {
            Console.WriteLine("System: Waiting for response...");
            var result = await chatSession.SendMessageAsync(message);
            Console.WriteLine($"Gemini AI: {result}\n");
        }
    }
}