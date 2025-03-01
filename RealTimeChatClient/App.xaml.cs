using Microsoft.Extensions.DependencyInjection;
using RealTimeChatClient.MVVM.View_Models;
using RealTimeChatClient.MVVM.Views;
using RealTimeChatClient.Services;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace RealTimeChatClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        var services = new ServiceCollection();

        services.AddSingleton<HttpClient>();
        services.AddSingleton<AuthService>();
        services.AddSingleton<SignalRClient>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<AuthViewModel>();

        ServiceProvider = services.BuildServiceProvider();
    }

   
}

