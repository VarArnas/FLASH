using FirstLab.src.back_end.utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FirstLab
{
    public partial class MenuWindow : Window
    {
        private HomeView homeView;

        public ObservableCollection<FlashcardSet> flashcardSets;

        public MenuWindow()
        {
            InitializeComponent();

            flashcardSets = new ObservableCollection<FlashcardSet>
            {
                new FlashcardSet { FlashcardSetName = "Set 1" },
                new FlashcardSet { FlashcardSetName = "Set 2" },
                new FlashcardSet { FlashcardSetName = "Set 3" },
                new FlashcardSet { FlashcardSetName = "Set 4" },
                new FlashcardSet { FlashcardSetName = "Set 5" },
                new FlashcardSet { FlashcardSetName = "Set 6" },
                new FlashcardSet { FlashcardSetName = "Set 7" },
                new FlashcardSet { FlashcardSetName = "Set 8" },
                new FlashcardSet { FlashcardSetName = "Set 9" },
                new FlashcardSet { FlashcardSetName = "Set 10" },
                new FlashcardSet { FlashcardSetName = "Set 11" },
                new FlashcardSet { FlashcardSetName = "Set 12" }
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

            homeView = new HomeView(this);
            contentControl.Content = homeView;

            string relativePath = "DataFiles\\AppName.txt";
            string? appName = FileUtility.ReadAppNameFromFile(AppDomain.CurrentDomain.BaseDirectory + relativePath);
            if (appName != null)
            {
                NameOFApp.Text = appName.ExtractCapLetters();
            }
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
           ControllerUtils.ChangeWindow(this, "Menu", homeView: homeView);
        }
    }
}
