using FirstLab.src.back_end;
using FirstLab.src.back_end.utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            flashcardSets = new ObservableCollection<FlashcardSet>(DataManager.LoadAllFlashcardSets());
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
            UpdateHeaderText("Menu");
            contentControl.Content = homeView;
        }

        public void UpdateHeaderText(string newText)
        {
            ViewsName.Text = newText;
        }

        private void ExitProgram(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes?", "save changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                DataManager.SaveAllFlashcardSets(flashcardSets);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
