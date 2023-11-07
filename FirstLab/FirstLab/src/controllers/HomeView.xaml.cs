using FirstLab.src.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab;

public partial class HomeView : UserControl
{

    public FlashcardOptions flashcardOptionsView;

    public HomeView(FlashcardOptions flashcardOptionsView)
    {
        InitializeComponent();
        InitializeHomeFields(flashcardOptionsView);
    }

    private void InitializeHomeFields(FlashcardOptions flashcardOptionsView)
    {
        this.flashcardOptionsView = flashcardOptionsView;
    }

    private void Flashcards_Clicked(object sender, RoutedEventArgs e)
    {
        ViewsUtils.ChangeWindow("Flashcards", flashcardOptionsView);
    }
}
