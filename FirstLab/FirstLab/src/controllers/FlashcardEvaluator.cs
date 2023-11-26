using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab
{
    public partial class FlashcardEvaluator : UserControl
    {
        string result;

        string question;

        string answer;
        public FlashcardEvaluator()
        {
            InitializeComponent();
        }

        public async void Evaluator_Loaded(object sender, RoutedEventArgs e)
        {
            QuestionTextBox.Focus();
            AnswerTextBox.Focus();
        }

        private async void selectButton(object sender, RoutedEventArgs e)
        {
            question = QuestionTextBox.Text;
            answer = AnswerTextBox.Text;
            string query = "Could the answer: " + answer + " be likely correct or likely incorrect for question: " + question + "?";
            result = await CallOpenAIController(query);
            PossibilityTextBox.Text = result;
        }
        
        private async Task<string> CallOpenAIController(string query)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Update the URL to match the actual URL of your API
                    string apiUrl = "https://localhost:7124/api/OpenAI/UseChatGPT";
                    //https://localhost:7124/api/OpenAI/UseChatGPT?query=heko
                    // Append the query as a parameter
                    string fullUrl = $"{apiUrl}?query={query}";

                    MessageBox.Show(fullUrl);

                    // Make the GET request
                    var response = await httpClient.GetAsync(fullUrl);

                    // Check if the request was successful (HTTP status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        // Log or handle the error
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return "Error communicating with the API";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return "An error occurred";
            }
        }

    }
}
