using System;
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using Scrutor;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class FirstTests
    {
        private const string TestPageUri =
            "file:///C:/src/Table-ControlObject/TableControlObjectDemo/Page/Table_ControlObject_FriendlyTester.html";

        public FirstTests()
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
                .FromAssemblyOf<FirstTests>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithSingletonLifetime());

            ServiceProvider = serviceCollection.BuildServiceProvider();

            Driver = ServiceProvider.GetRequiredService<IWebDriver>();
            TableControllerPostPage = ServiceProvider.GetRequiredService<TableControllerPostPage>();
        }

        private IWebDriver Driver { get; set; }

        private IWebDriverFactory WebDriverFactory { get; set; }

        private IWebDriverManager DriverManager { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private TableControllerPostPage TableControllerPostPage { get; set; }
        
        [Test]
        public void TitleIsReturnedCorrectly()
        {
            TableControllerPostPage.GetActualPageTitle().Should().Be("Table ControlObject | Friendly Tester");
        }
        
        [Test]
        public void UrlIsReturnedCorrectly()
        {
            TableControllerPostPage.GetActualUri().Should().Be(TestPageUri);
        }

        [TestCase(false, 5)]
        [TestCase(true, 30)]
        public void DefaultWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            Action act = () => TableControllerPostPage.TimeoutFailingToFindNonExistentElement(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds} seconds*");
        }

        

        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void DefinedWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            TableControllerPostPage.NonExistentBlock = new NonExistentBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            Action act = () => TableControllerPostPage.TimeoutFailingToFindNonExistentElement(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds} seconds*");
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
    }
}