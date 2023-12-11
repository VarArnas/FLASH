using Castle.DynamicProxy;
using FirstLab.src.controllers;
using FirstLab.src.models;
using FirstLab.src.utilities;
using System.IO;
using System.Linq;
using System.Text;

namespace FirstLab.src.interceptors;

public class LoggingInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
      
        invocation.Proceed();


        if (invocation.Method.Name == "CheckErrorsAndSaveFlashcard" && ViewsUtils.menuWindowReference!.contentControl.Content is FlashcardOptions)
        {
            var flashcardSet = invocation.Arguments[0] as FlashcardSet;

            var colorsBuilder = new StringBuilder();
            colorsBuilder.Append("The ")
                         .Append(flashcardSet!.FlashcardSetName)
                         .Append(" flashcardSet had these colors selected: ");

            if (flashcardSet.Flashcards != null)
            {
                foreach (var flashcard in flashcardSet.Flashcards)
                {
                    if (flashcard.FlashcardColor != null)
                    {
                        colorsBuilder.Append(flashcard.FlashcardColor).Append(", ");
                    }
                }

                if (flashcardSet.Flashcards.Any())
                {
                    colorsBuilder.Length -= 2;
                }

                colorsBuilder.AppendLine(".");
            }

            string baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName;
            string projectDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\bin"));
            string logFilePath = Path.Combine(projectDirectory, "src\\interceptors\\LogsOfColors.txt");
            File.AppendAllText(logFilePath, colorsBuilder.ToString());
        }
    }
}
