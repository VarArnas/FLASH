using FirstLab.src.back_end;
using FirstLab.src.back_end.utilities;
using FirstLab.XAML;
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

        private LogsView logsView;

        private DateTime playWindowEndTime;

        public MenuWindow()
        {
            InitializeComponent();
            flashcardSets = new ObservableCollection<FlashcardSet>(DataManager.LoadAllFlashcardSets());
            homeView = new HomeView(this);
            contentControl.Content = homeView;
            logsView = new LogsView();
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
           if(contentControl.Content is PlayWindow)
           {
                playWindowEndTime = DateTime.Now;
                logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
           }

           ViewsUtils.ChangeWindow(this, "Menu", homeView: homeView);
        }

        private void ExitProgram(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes?", "save changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                DataManager.SaveAllFlashcardSets(flashcardSets);
                DataManager.SaveLogs(logsView.flashcardSetsLogs);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AccessLogs_Click(object sender, RoutedEventArgs e)
        {
            if (contentControl.Content is PlayWindow)
            {
                playWindowEndTime = DateTime.Now;
                logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
            }

            ViewsUtils.ChangeWindow(this, "Logs", logsView: logsView);
        }
    }
}
