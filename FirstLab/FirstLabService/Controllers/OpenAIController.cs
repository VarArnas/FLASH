using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using System.Threading.Tasks;

namespace ChatGPT_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        [HttpGet]
        [Route("UseChatGPT")]
        public async Task<IActionResult> UseChatGPT(string query)
        {
            string outputResult = "";
            var openai = new OpenAIAPI("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");
            ChatRequest chatRequest = new ChatRequest();
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.Content = query;
            chatRequest.Messages = new List<ChatMessage>();
            chatRequest.Messages.Add(chatMessage); //why is this grayed out????????????????
            chatRequest.MaxTokens = 1024;
            chatRequest.Model = "gpt-3.5-turbo-16k";

            var result = await openai.Chat.CreateChatCompletionAsync(chatRequest);

            outputResult = result.ToString();

            return Ok(outputResult);
        }
    }
}
