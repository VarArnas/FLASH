﻿using FirstLab.src.back_end.data;
using FirstLab.src.back_end.errorHandling;
using FirstLab.src.back_end.factories;
using FirstLab.XAML;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FirstLab;

public partial class App : Application
{
    public static IHost? AppHost {  get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices ((hostContext, services) => 
            {
                services.AddSingleton<MenuWindow>();
                services.AddSingleton<HomeView>();
                services.AddSingleton<LogsView>();
                services.AddSingleton<FlashcardOptions>();
                services.AddSingleton<DoubleAnimation>();
                services.AddTransient<ArrayList>();
                services.AddTransient<Random>();
                services.AddTransient<BrushConverter>();
                services.AddTransient<IMainFactories, MainFactories>();
                services.AddTransient<ObservableCollection<Flashcard>>();
                services.AddTransient<List<ErrorCode>>();
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
