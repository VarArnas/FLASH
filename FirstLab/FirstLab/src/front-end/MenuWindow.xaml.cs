using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace FirstLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private HomeView homeView;

        public ObservableCollection<FlashcardSet> flashcardSets;

        public MenuWindow()
        {
            InitializeComponent();

            flashcardSets = new ObservableCollection<FlashcardSet> //create a new set of flashcards for demonstration
            {
                new FlashcardSet { SetName = "Set 1" },
                new FlashcardSet { SetName = "Set 2" },
                new FlashcardSet { SetName = "Set 3" },
                new FlashcardSet { SetName = "Set 4" },
                new FlashcardSet { SetName = "Set 5" },
                new FlashcardSet { SetName = "Set 6" },
                new FlashcardSet { SetName = "Set 7" },
                new FlashcardSet { SetName = "Set 8" },
                new FlashcardSet { SetName = "Set 9" },
                new FlashcardSet { SetName = "Set 10" },
                new FlashcardSet { SetName = "Set 11" },
                new FlashcardSet { SetName = "Set 12" }
            };
        }

        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 1.0;
            opacityAnimation.To = 0.1;
            opacityAnimation.Duration = TimeSpan.FromSeconds(2);
            opacityAnimation.AutoReverse = true;
            opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;
            breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, opacityAnimation);

            //ElipseAnimation^^^

            homeView = new HomeView(this);

            contentControl.Content = homeView;
        }

        private void MovingWindow(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ReturnToHomeView(object sender, RoutedEventArgs e)
        {
            // Set the Content of the ContentControl to the existing HomeView
            UpdateHeaderText("Menu");
            contentControl.Content = homeView;
        }

        public void UpdateHeaderText(string newText)
        {
            ViewsName.Text = newText;
        }
    }
}
