using FirstLab.src.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab.src.controllers;

public partial class HomeView : UserControl
{

    public FlashcardOptions flashcardOptionsView;

    public FlashcardEvaluator flashcardEvaluatorView;

    public HomeView(FlashcardOptions flashcardOptionsView, FlashcardEvaluator flashcardEvaluatorView)
    {
        InitializeComponent();
        InitializeHomeFields(flashcardOptionsView, flashcardEvaluatorView);
    }

    private void InitializeHomeFields(FlashcardOptions flashcardOptionsView, FlashcardEvaluator flashcardEvaluatorView)
    {
        this.flashcardOptionsView = flashcardOptionsView;
        this.flashcardEvaluatorView = flashcardEvaluatorView;
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
