using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiChatbot.Services
{
    public class AiService
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiKey;

        public AiService(string apiKey)
        {
            _apiKey = apiKey;
            if (_apiKey != "demo")
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
        }

        public async Task<string> ChatWithAi(List<Dictionary<string, string>> chatHistory)
        {
            // DEMO MODE
            if (_apiKey == "demo")
            {
                string lastUserMessage = chatHistory.LastOrDefault()?["content"] ?? "";

                if (lastUserMessage.ToLower().Contains("score"))
                {
                    return @"
Here are 4 suggested scoring formulas:

1. Formula A (Balanced)  
   Score = (Budget * 0.4) + (EmployeeStrength * 0.3) + (NumberOfInstruments * 0.3)  
   Reasoning: Balanced weightage across cost, people, and tools.

2. Formula B (Budget-Heavy)  
   Score = (Budget * 0.6) + (EmployeeStrength * 0.2) + (NumberOfInstruments * 0.2)  
   Reasoning: Prioritizes projects with larger financial backing.

3. Formula C (Complexity Focus)  
   Score = (EmployeeStrength * 0.5) + (NumberOfInstruments * 0.4) + (Budget * 0.1)  
   Reasoning: Highlights larger teams and multiple systems.

4. Formula D (Tech-Driven)  
   Score = (NumberOfInstruments * 0.6) + (EmployeeStrength * 0.3) + (Budget * 0.1)  
   Reasoning: Best for projects where technical systems drive complexity.
";
                }
                else
                {
                    return $"You said: \"{lastUserMessage}\"";
                }
            }

            // REAL API
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = chatHistory
            };

            var response = await _client.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();

        }
    }
}
