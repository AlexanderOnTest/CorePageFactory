using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class WaitTests
    {
        private string TestPageTitle => "AlexanderOnTest - PageFactory Test Page";

        private IWebDriver Driver { get; set; }

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

        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MinimumElementWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMinRowsToLoad(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds} seconds: Less than 15 of By By.TagName: tr were returned - Wait Condition not met");
        }
        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MaximumElementWaitTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMaxRowsToLoad(useLongWait);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds} seconds: More than 2 of By By.TagName: tr were returned - Wait Condition not met");
        }

        #region SetUpTearDown

        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServiceProvider = ConfigurationModule.GetServiceProvider(true);
            DriverManager = ServiceProvider.GetRequiredService<IWebDriverManager>();
            Driver = ServiceProvider.GetRequiredService<IWebDriver>();
            TestPage = ServiceProvider.GetRequiredService<TestPage>();
            Driver.Url = TestSettings.TestPageUriString;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DriverManager.Quit();
            DriverManager?.Dispose();
        }

        #endregion

    }
}