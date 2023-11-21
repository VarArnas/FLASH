using OpenAI_API.Moderation;
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
        public FlashcardEvaluator()
        {
            InitializeComponent();
            PossibilityTextBox.Text = result;
        }

        public async void Evaluator_Loaded(object sender, RoutedEventArgs e)
        {
            QuestionTextBox.Focus();
            AnswerTextBox.Focus();

            string query = "Hows it going?";
            result = await CallOpenAIController(query);
            PossibilityTextBox.Text = result;
        }

        private async Task<string> CallOpenAIController(string query)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "http://localhost:7124/api/OpenAI/UseChatGPT?query=" + query;

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
