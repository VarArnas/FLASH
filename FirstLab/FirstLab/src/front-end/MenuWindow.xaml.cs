using FirstLab.src.back_end.utilities;
using FirstLab.XAML;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FirstLab
{
    public partial class MenuWindow : Window
    {
        private HomeView homeView;

        private LogsView logsView;

        private DateTime playWindowEndTime;

        public MenuWindow()
        {
            InitializeComponent();
            InitializeMenuFields();
        }

        private void InitializeMenuFields()
        {
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
           else if (contentControl.Content is FlashcardCustomization) 
           {
                MessageBox.Show("There are unsaved changes!!");
                return;
           }
           ViewsUtils.ChangeWindow(this, "Menu", homeView);
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
            ViewsUtils.ChangeWindow(this, "Logs", logsView);
        }
    }
}
