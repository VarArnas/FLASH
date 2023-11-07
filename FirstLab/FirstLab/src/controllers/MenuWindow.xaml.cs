using FirstLab.src.interfaces;
using FirstLab.src.utilities;
using FirstLab.XAML;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FirstLab;

public partial class MenuWindow : Window
{
    private HomeView homeView;

    private LogsView logsView;

    private DateTime playWindowEndTime;

    public MenuWindow(HomeView homeView, LogsView logsView, IFactoryContainer factoryContainer)
    {
        InitializeComponent();
        InitializeAnimation();
        InitializeMenuFields(homeView,logsView, factoryContainer);
    }

    private void InitializeAnimation()
    {
        DoubleAnimation opacityAnimation = new DoubleAnimation();
        opacityAnimation.From = 1.0;
        opacityAnimation.To = 0.1;
        opacityAnimation.Duration = TimeSpan.FromSeconds(2);
        opacityAnimation.AutoReverse = true;
        opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;
        breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, opacityAnimation);
    }

    private void InitializeMenuFields(HomeView homeView, LogsView logsView, IFactoryContainer factoryContainer)
    {
        this.homeView = homeView;
        contentControl.Content = homeView;
        this.logsView = logsView;
        ViewsUtils.menuWindowReference = this;
    }

    private void MovingWindow(object sender, MouseButtonEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void ReturnToHomeView_Click(object sender, RoutedEventArgs e)
    {
       if(contentControl.Content is PlayWindow)
       {
            playWindowEndTime = DateTime.Now;
            logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
       } 
       else if (contentControl.Content is FlashcardCustomization) 
       {
            ShowMessage("There are unsaved changes!!");
            return;
       }
       ViewsUtils.ChangeWindow("Menu", homeView);
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
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
        else if (contentControl.Content is FlashcardCustomization)
        {
            ShowMessage("There are unsaved changes!!");
            return;
        }
        ViewsUtils.ChangeWindow("Logs", logsView);
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
