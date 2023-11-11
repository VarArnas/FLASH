public class FlashcardAppService
{
    private readonly HttpClient _httpClient;

    public FlashcardAppService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> EvaluateFlashcard(string question, string answer)
    {
        var requestBody = new { Question = question, Answer = answer };
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/flashcards/evaluate", requestBody);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<FlashcardEvaluationResult>();
            return result.EvaluationResult;
        }

        // Handle error scenarios
        return "Error occurred";
    }
}
