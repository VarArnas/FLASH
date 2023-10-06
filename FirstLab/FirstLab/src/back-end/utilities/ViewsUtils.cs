using FirstLab.XAML;

namespace FirstLab.src.back_end.utilities
{
    internal class ViewsUtils
    {
        public static void ChangeWindow(MenuWindow menuWindowReference, string headerText, FlashcardOptions? flashcardOptionsView = null, FlashcardCustomization? flashcardCustomizationView = null, HomeView? homeView = null, PlayWindow? playWindowReference = null, LogsView? logsView = null)
        {
            menuWindowReference.ViewsName.Text = headerText;

            if (flashcardOptionsView != null)
            {
                menuWindowReference.contentControl.Content = flashcardOptionsView;
            }
            else if (flashcardCustomizationView != null)
            {
                menuWindowReference.contentControl.Content = flashcardCustomizationView;
            }
            else if (homeView != null)
            {
                menuWindowReference.contentControl.Content = homeView;
            }
            else if (playWindowReference != null)
            {
                menuWindowReference.contentControl.Content = playWindowReference;
            }
            else if (logsView != null)
            {
                menuWindowReference.contentControl.Content = logsView;
            }
        }
    }
}
