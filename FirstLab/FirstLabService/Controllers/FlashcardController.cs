using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/flashcards")]
public class FlashcardController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpPost("evaluate")]
    public IActionResult EvaluateFlashcard([FromBody] FlashcardEvaluationRequest request)
    {
        var evaluationResult = _flashcardService.EvaluateFlashcard(request.Question, request.Answer);
        return Ok(new { EvaluationResult = evaluationResult });
    }
}
