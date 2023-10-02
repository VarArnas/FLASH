using FirstLab.src.back_end.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab
{
    public partial class HomeView : UserControl
    {
        private MenuWindow menuWindowReference;

        private FlashcardOptions flashcardOptionsView;

        public HomeView(MenuWindow menuWindowReference)
        {
            InitializeComponent();
            this.menuWindowReference = menuWindowReference;
        }

        private void Flashcards_Clicked(object sender, RoutedEventArgs e)
        {
            flashcardOptionsView = new FlashcardOptions(menuWindowReference);
            ControllerUtils.ChangeWindow(menuWindowReference, "Flashcards", flashcardOptionsView);
        }
    }
}
