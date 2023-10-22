using FirstLab.src.back_end.utilities;
using System.Windows;
using System.Windows.Controls;

namespace FirstLab
{
    public partial class HomeView : UserControl
    {
        private MenuWindow menuWindowReference;

        public FlashcardOptions flashcardOptionsView;

        public HomeView(MenuWindow menuWindowReference)
        {
            InitializeComponent();
            InitializeHomeFields(menuWindowReference);
        }

        private void InitializeHomeFields(MenuWindow menuWindowReference)
        {
            this.menuWindowReference = menuWindowReference;
        }

        private void Flashcards_Clicked(object sender, RoutedEventArgs e)
        {
            flashcardOptionsView = new FlashcardOptions(menuWindowReference);
            ViewsUtils.ChangeWindow(menuWindowReference, "Flashcards", flashcardOptionsView);
        }
    }
}
