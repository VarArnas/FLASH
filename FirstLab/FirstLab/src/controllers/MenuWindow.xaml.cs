using FirstLab.src.interfaces;
using FirstLab.XAML;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace FirstLab;

public partial class MenuWindow : Window
{
    private HomeView homeView;

    private LogsView logsView;

    private IMenuWindowService _ifMenuWindowService;

    public MenuWindow(HomeView homeView, LogsView logsView, IFactoryContainer factoryContainer, IMenuWindowService ifMenuWindowService)
    {
        InitializeComponent();
        InitializeMenuFields(homeView,logsView, factoryContainer, ifMenuWindowService);
        InitializeAnimation();
    }

    private void InitializeMenuFields(HomeView homeView, LogsView logsView, IFactoryContainer factoryContainer,
        IMenuWindowService ifMenuWindowService)
    {
        this.homeView = homeView;
        _ifMenuWindowService = ifMenuWindowService;
        contentControl.Content = homeView;
        this.logsView = logsView;
        _ifMenuWindowService.InitializeViewsUtils(this);
    }

    private void InitializeAnimation()
    {
        breathingEllipse.BeginAnimation(Ellipse.OpacityProperty, _ifMenuWindowService.CreateElipseAnimation());
    }

    private void MovingWindow(object sender, MouseButtonEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    public void ReturnToHomeView_Click(object? sender = null, RoutedEventArgs? e = null)
    {
        _ifMenuWindowService.CheckSenderOfTheButtonAndChangeView(sender, logsView, homeView, "Menu");
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AccessLogs_Click(object sender, RoutedEventArgs e)
    {
        _ifMenuWindowService.CheckSenderOfTheButtonAndChangeView(sender, logsView, homeView, "Logs");
    }
}
