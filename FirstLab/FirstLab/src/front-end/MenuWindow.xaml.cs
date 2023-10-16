using FirstLab.src.back_end;
using FirstLab.src.back_end.utilities;
using FirstLab.XAML;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Thread moveWindowThread = new Thread(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        DragMove();
                    });
                });

                moveWindowThread.Start();
            }
        }

        private void ReturnToHomeView(object sender, RoutedEventArgs e)
        {
            if (contentControl.Content is PlayWindow)
            {
                DateTime playWindowEndTime = DateTime.Now;
                PlayWindow playWindow = contentControl.Content as PlayWindow;

                Thread returnToHomeViewThread = new Thread(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
                        ViewsUtils.ChangeWindow(this, "Menu", homeView);
                    });
                });

                returnToHomeViewThread.Start();
            }
            else
            {
                ViewsUtils.ChangeWindow(this, "Menu", homeView);
            }
        }

        private void ExitProgram(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes?", "Save Changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Thread exitProgramThread = new Thread(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        DataManager.SaveAllFlashcardSets(flashcardSets);
                        DataManager.SaveLogs(logsView.flashcardSetsLogs);
                    });
                });

                exitProgramThread.Start();
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
                DateTime playWindowEndTime = DateTime.Now;

                Thread accessLogsThread = new Thread(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
                        ViewsUtils.ChangeWindow(this, "Logs", logsView);
                    });
                });

                accessLogsThread.Start();
            }
            else
            {
                ViewsUtils.ChangeWindow(this, "Logs", logsView);
            }
        }
    }
}
