using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstLab
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
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
            flashcardOptionsView = new FlashcardOptions(menuWindowReference.flashcardSets, menuWindowReference); //send the Obersavble Collection to the view

            menuWindowReference.UpdateHeaderText("Flashcards");
          
            if (menuWindowReference != null)
            {
                menuWindowReference.contentControl.Content = flashcardOptionsView;
            }
        }
    }
}
