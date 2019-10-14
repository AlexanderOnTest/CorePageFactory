using System;
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using Scrutor;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public static class ConfigurationModule
    {
        public static IServiceProvider GetServiceProvider(bool headless)
        {
            // Force local Browser running for local file
            IWebDriverConfiguration driverConfig =
                WebDriverConfigurationBuilder.Start()
                    .WithHeadless(headless)
                    .WithBrowser(Browser.Firefox)
                    .WithWindowSize(WindowSize.Hd)
                    .Build();
            
            IWebDriverManager driverManager = ServiceCollectionFactory.GetDefaultServiceCollection(true, driverConfig)
                .BuildServiceProvider()
                .GetRequiredService<IWebDriverManager>();

            IServiceCollection serviceCollection = new ServiceCollection();
            
            serviceCollection.AddSingleton<IWebDriverManager>(driverManager);
            serviceCollection.AddSingleton<IWebDriver>(driverManager.Get());
            
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<PageTests>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithSingletonLifetime());

            return serviceCollection.BuildServiceProvider();
        }    
    }
}