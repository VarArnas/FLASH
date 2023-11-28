using FirstLab.src.controllers;
using System.Windows.Media.Animation;

namespace FirstLab.src.interfaces
{
    public interface IMenuWindowService
    {
        void CheckSenderOfTheButtonAndChangeView<T>(T someView, LogsView logsView, HomeView homeView, string nameOfView);

        DoubleAnimation CreateElipseAnimation();

        void ShowMessage(string message);

        void InitializeViewsUtils(MenuWindow menuWindow);
    }
}