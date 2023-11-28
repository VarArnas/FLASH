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

        string useranswer;
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
            useranswer = UserAnswerTextBox.Text;

            string query = "You are a teacher who is evaluating a students answer to this question: " + question
                + " the actual answer is this: " + answer + ". And the student wrote this: " + useranswer + 
                ". Is this students answer similar to the actual answer. With one word say YES or NO.";
            result = await CallOpenAIController(query);
            PossibilityTextBox.Text = result;
        }
        
        private async Task<string> CallOpenAIController(string query)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7124/api/OpenAI/UseChatGPT";
                   
                    string fullUrl = $"{apiUrl}?query={query}";

                    MessageBox.Show(fullUrl);

                    var response = await httpClient.GetAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return "Error communicating with the API";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return "An error occurred";
            }
        }

    }
}
