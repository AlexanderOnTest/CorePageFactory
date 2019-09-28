using System;
using System.Reflection;
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
    public class PageTests
    {
        internal static readonly string TestPageUri;
        
        private string TestPageTitle => "AlexanderOnTest - PageFactory Test Page";

        static PageTests()
        {
            TestPageUri = $"file://{AppDomain.CurrentDomain.BaseDirectory}TestPages/TestPage.html";
        }

        private IWebDriver Driver { get; set; }

        private IWebDriverFactory WebDriverFactory { get; set; }

        private IWebDriverManager DriverManager { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private TestPage TestPage { get; set; }

        [Test]
        public void ExpectedTitleIsReturnedCorrectly()
        {
            TestPage.GetExpectedPageTitle().Should().Be(TestPageTitle);
        }
        
        [Test]
        public void TitleIsReturnedCorrectly()
        {
            TestPage.GetActualPageTitle().Should().Be(TestPageTitle);
        }
        
        [Test]
        public void UriStringIsReturnedCorrectly()
        {
            TestPage.GetActualUri().Should().Be(TestPageUri);
        }
        
        [Test]
        public void ExpectedUriStringIsReturnedCorrectly()
        {
            TestPage.GetActualUri().Should().Be(TestPage.GetExpectedUri());
        }

        #region SetUpTearDown

        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //this.TestPageUri = $"file://{AppDomain.CurrentDomain.BaseDirectory}TestPages/TestPage.html";
            
            // Force local Browser running for local file
            IWebDriverConfiguration driverConfig =
                WebDriverConfigurationBuilder.Start()
                    .RunHeadless()
                    .WithBrowser(Browser.Firefox)
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