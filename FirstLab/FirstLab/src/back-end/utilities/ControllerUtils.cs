using FirstLab.XAML;
using System.Windows.Controls;

namespace FirstLab.src.back_end.utilities
{
    public static class ControllerUtils
    {
        public static void setDefaultText(TextBox textBox, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = defaultText;
            }

        }

        public static void setEmptyText(TextBox textBox, string defaultText)
        {
            if(textBox.Text == defaultText)
            {
                textBox.Text = string.Empty;
            }
        }

        public static void ChangeWindow(MenuWindow menuWindowReference, string headerText, FlashcardOptions? flashcardOptionsView = null, FlashcardCustomization? flashcardCustomizationView = null, HomeView? homeView = null, PlayWindow? playWindowReference = null)
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
            else if(playWindowReference != null)
            {
                menuWindowReference.contentControl.Content = playWindowReference;
            }
        }
    }
}
