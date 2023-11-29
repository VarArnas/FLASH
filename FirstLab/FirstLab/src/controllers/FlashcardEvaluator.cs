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
                ". Is this students answer similar/correct to the actual answer or is it partly correct or is it incorrect. If it is correct then write \"1\", if it is partly correct write \"0.5\" and if its it incorrect write \"0\" AND DONT WRITE ANY OTHER SYMBOLS OR CHARACTERS." +
                "ALSO take the question into consideration itself and evaluate if the student's answer ir correct, partly correct or incorrect based not only on the actual answer but also the question. REMEMBER ONLY WRITE 1 FOR CORRECT 0.5 FOR PARTLY AND 0 FOR INCORRECT DONT WRITE ANY OTHER SYMBOLS OR ANYTHING";
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
