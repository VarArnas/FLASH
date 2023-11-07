using FirstLab.src.controllers.services;
using FirstLab.src.data;
using FirstLab.src.factories;
using FirstLab.src.interfaces;
using FirstLab.XAML;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace FirstLab;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MenuWindow>();
                services.AddSingleton<HomeView>();
                services.AddSingleton<LogsView>();
                services.AddSingleton<FlashcardOptions>();
                services.AddSingleton<IFactoryContainer, FactoryContainer>();
                services.AddSingleton<IPlayWindowService, PlayWindowService>();
                services.AddSingleton<IFlashcardOptionsService, FlashcardOptionsService>();
                services.AddSingleton<ILogsViewService, LogsViewService>();
                services.AddSingleton<IFlashcardCustomizationService, FlashcardCustomizationService>();
                services.AddTransient<DataContext>();

            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

    
        var startupWindow = AppHost.Services.GetRequiredService<MenuWindow>();
        startupWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}
