using FirstLab.src.back_end.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab;

public partial class HomeView : UserControl
{
    public FlashcardOptions flashcardOptionsView;

    public HomeView(FlashcardOptions flashcardOptionsView)
    {
        InitializeComponent();
        this.flashcardOptionsView = flashcardOptionsView;
    }

    private void Flashcards_Clicked(object sender, RoutedEventArgs e)
    {
        ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsView);
    }
}
