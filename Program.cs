﻿using AiChatbot.Services;
using AIChatbot.Models;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("AI Job Evaluation Chatbot 🤖");
        Console.WriteLine("Type 'exit' to quit.\n");

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string openAiKey = config["OpenAIKey"];
        string googleApiKey = config["GoogleApiKey"];
        string googleCx = config["GoogleCx"];
        string linkedInToken = config["LinkedInToken"];
        string serpApiKey = config["SerpApiKey"];

        var aiService = new AiService(openAiKey);
        var googleService = new GoogleService(googleApiKey, googleCx);
        var linkedInService = new LinkedInService(linkedInToken);
        var glassdoorService = new GlassdoorService(serpApiKey);

        var chatHistory = new List<Dictionary<string, string>>();

        while (true)
        {
            Console.Write("You: ");
            string userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput)) continue;
            if (userInput.ToLower() == "exit") break;

            chatHistory.Add(new Dictionary<string, string>
            {
                { "role", "user" },
                { "content", userInput }
            });

            string response = "";

            if (userInput.StartsWith("google", StringComparison.OrdinalIgnoreCase))
            {
                var query = userInput.Substring(6).Trim();
                response = await googleService.SearchCompanyAsync(query);
            }
            else if (userInput.StartsWith("linkedin", StringComparison.OrdinalIgnoreCase))
            {
                var query = userInput.Substring(8).Trim();
                response = await linkedInService.GetCompanyProfileAsync(query);
            }
            else if (userInput.StartsWith("glassdoor", StringComparison.OrdinalIgnoreCase))
            {
                var query = userInput.Substring(9).Trim();
                response = await glassdoorService.GetCompanyReviewsAsync(query);
            }
            else if (userInput.StartsWith("score project", StringComparison.OrdinalIgnoreCase))
            {
                var project = new ProjectInput
                {
                    Budget = 100000,
                    EmployeeStrength = 50,
                    Instruments = new List<string> { "CRM", "ERP" }
                };

                response = await aiService.ChatWithAi(chatHistory);
            }
            else
            {
                response = await aiService.ChatWithAi(chatHistory);
            }

            Console.WriteLine($"\nAI: {response}\n");

            chatHistory.Add(new Dictionary<string, string>
            {
                { "role", "assistant" },
                { "content", response }
            });
        }
    }
}
