using FirstLab.src.data;
using FirstLab.src.factories;
using FirstLab.src.interfaces;
using FirstLab.src.mappers;
using FirstLab.src.services;
using FirstLab.src.utilities;
using FirstLab.XAML;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

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
                services.AddSingleton<FlashcardEvaluator>();
                services.AddSingleton<IFactoryContainer, FactoryContainer>();
                services.AddSingleton<IPlayWindowService, PlayWindowService>();
                services.AddSingleton<IFlashcardOptionsService, FlashcardOptionsService>();
                services.AddSingleton<ILogsViewService, LogsViewService>();
                services.AddSingleton<IFlashcardCustomizationService, FlashcardCustomizationService>();
                services.AddTransient<DataContext>();
                services.AddSingleton<IFlashcardSetLogMapper,FlashcardSetLogMapper>();
                services.AddSingleton<IFlashcardSetMapper,FlashcardSetMapper>();
                services.AddSingleton<IMenuWindowService, MenuWindowService>();
                services.AddSingleton<IDatabaseRepository, DatabaseRepository>();
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
