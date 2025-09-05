namespace AiChatbot.Services
{
    public class GlassdoorService
    {
        private readonly string _apiKey;

        public GlassdoorService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetCompanyReviewsAsync(string company)
        {
            if (_apiKey == "demo")
            {
                return $"{company} Reviews: Rating=4.1, Pros='Great culture', Cons='Work-life balance issues'.";
            }

            // REAL GLASSDOOR API CALL (via SerpAPI)
            // string url = $"https://serpapi.com/search.json?engine=glassdoor&q={Uri.EscapeDataString(company)}&api_key={_apiKey}";
            // var response = await new HttpClient().GetAsync(url);
            // var json = await response.Content.ReadAsStringAsync();
            // return $"[Glassdoor API Response] {json}";
            return "[Glassdoor API Integration Not Implemented]";
        }
    }
}
