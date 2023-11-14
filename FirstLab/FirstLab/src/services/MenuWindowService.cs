using FirstLab.src.interfaces;
using FirstLab.src.utilities;
using FirstLab.XAML;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace FirstLab.src.services;

public class MenuWindowService : IMenuWindowService
{
    public MenuWindowService() { }

    public DoubleAnimation CreateElipseAnimation()
    {
        DoubleAnimation opacityAnimation = new()
        {
            From = 1.0,
            To = 0.1,
            Duration = TimeSpan.FromSeconds(2),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };
        return opacityAnimation;
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }

    public void CheckSenderOfTheButtonAndChangeView<T>(T someView, LogsView logsView, HomeView homeView, string nameOfView)
    {
        if (someView is PlayWindow)
        {
            DateTime playWindowEndTime = DateTime.Now;
            logsView.CalculateAndCreateLog(homeView.flashcardOptionsView.playWindowStartTime, playWindowEndTime, homeView.flashcardOptionsView.flashcardSet);
            ViewsUtils.ChangeWindow(nameOfView, homeView);
            return;
        }
        else if (ViewsUtils.menuWindowReference!.contentControl.Content is FlashcardCustomization)
        {
            ShowMessage("There are unsaved changes!!");
            return;
        }

        if (nameOfView == "Logs")
        {
            ViewsUtils.ChangeWindow(nameOfView, logsView);
            return;
        }
        ViewsUtils.ChangeWindow(nameOfView, homeView);
    }

    public void InitializeViewsUtils(MenuWindow menuWindow)
    {
        ViewsUtils.menuWindowReference = menuWindow;
    }
}
