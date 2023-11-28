using FirstLab.src.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab.src.controllers;

public partial class HomeView : UserControl
{

    public FlashcardOptions flashcardOptionsView;

    public FlashcardEvaluator flashcardEvaluatorView;

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

    private void Evaluator_Clicked(object sender, RoutedEventArgs e)
    {
        ViewsUtils.ChangeWindow("Answer evaluator", flashcardEvaluatorView);
    }
}
