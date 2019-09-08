using System;
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using Scrutor;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class WaitTests
    {
        private string TestPageUri =>
            "file:///C:/src/CorePageFactory/Src/Test/AlexanderOnTest.NewNetPageFactory.SystemTests/TestPages/TestPage.html";

        private string TestPageTitle => "AlexanderOnTest - PageFactory Test Page";

        private IWebDriver Driver { get; set; }

        private IWebDriverFactory WebDriverFactory { get; set; }

        private IWebDriverManager DriverManager { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private TestPage TestPage { get; set; }

        [TestCase(false, 5)]
        [TestCase(true, 30)]
        public void DefaultWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using default timeouts
            TestPage.NonExistentBlock = new NonExistentBlock(Driver);

            Action act = () => TestPage.TimeoutFailingToFindNonExistentElement(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds} seconds*");
        }

        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void DefinedWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.NonExistentBlock = new NonExistentBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            Action act = () => TestPage.TimeoutFailingToFindNonExistentElement(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds} seconds*");
        }

        #region SetUpTearDown

        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Force local Browser running for local file
            IWebDriverConfiguration driverConfig =
                WebDriverConfigurationBuilder.Start()
                    .RunHeadless()
                    .WithBrowser(Browser.Chrome)
                    .WithWindowSize(WindowSize.Fhd)
                    .Build();
            
            DriverManager = ServiceCollectionFactory.GetDefaultServiceCollection(true, driverConfig)
                .BuildServiceProvider()
                .GetRequiredService<IWebDriverManager>();

            
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IWebDriver>(DriverManager.Get());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<PageTests>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithSingletonLifetime());

            ServiceProvider = serviceCollection.BuildServiceProvider();

            Driver = ServiceProvider.GetRequiredService<IWebDriver>();
            TestPage = ServiceProvider.GetRequiredService<TestPage>();
        }

        [SetUp]
        public void SetUp()
        {
            Driver = DriverManager.Get();
            Driver.Url = TestPageUri;
        }

        [TearDown]
        public void Teardown()
        {
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DriverManager.Quit();
            DriverManager?.Dispose();
            WebDriverFactory?.Dispose();
        }

        #endregion

    }
}