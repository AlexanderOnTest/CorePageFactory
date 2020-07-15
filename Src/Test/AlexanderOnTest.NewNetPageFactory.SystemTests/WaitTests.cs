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
        private IServiceProvider ServiceProvider { get; set; }

        private IWebDriverManager DriverManager => ServiceProvider.GetRequiredService<IWebDriverManager>();

        private IWebDriver Driver => ServiceProvider.GetRequiredService<IWebDriver>();

        private TestPage TestPage => ServiceProvider.GetRequiredService<TestPage>();

        [TestCase(true, 2)]
        [TestCase(false, 1)]
        public void WaitForRootElementWorks(bool useLongWait, int expectedTimeoutInSeconds)
        {
            {
                //Ensure we are using shorter / non default timeouts
                TestPage.NonExistentBlock = new NonExistentBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                Action act = () => TestPage.TimeoutFailingToFindRootElement(useLongWait);
                act
                    .Should().Throw<WebDriverTimeoutException>()
                    .WithMessage($"Timed out after {expectedTimeoutInSeconds.ToString()} seconds*");
            }
        }
        
        [TestCase(false, 5)]
        [TestCase(true, 30)]
        public void DefaultWaitTimesOutAfterExpectedTime(
            bool useLongWait, 
            int expectedTimeoutInSeconds)
        {
            //Ensure we are using default timeouts
            TestPage.NonExistentBlock = new NonExistentBlock(Driver);

            Action act = () => TestPage.TimeoutFailingToFindNonExistentElement(useLongWait, false);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds.ToString()} seconds*");
        }

        [TestCase(false, 1, true)]
        [TestCase(true, 2, true)]
        [TestCase(false, 1, false)]
        [TestCase(true, 2, false)]
        public void DefinedWaitTimesOutAfterExpectedTime(
            bool useLongWait, 
            int expectedTimeoutInSeconds,
            bool useBy)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.NonExistentBlock = new NonExistentBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            Action act = () => TestPage.TimeoutFailingToFindNonExistentElement(useLongWait, useBy);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage($"Timed out after {expectedTimeoutInSeconds.ToString()} seconds*");
        }
        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MinimumElementWaitUsingCssSelectorTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMinRowsToLoad(useLongWait, false);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds.ToString()} seconds: Less than 15 (By.CssSelector: tr) were found in the searched context.");
        }
        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MinimumElementWaitUsingByTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMinRowsToLoad(useLongWait, true);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds.ToString()} seconds: Less than 15 (By.TagName: tr) were found in the searched context.");
        }
        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MaximumElementWaitUsingCssSelectorTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMaxRowsToLoad(useLongWait, false);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds.ToString()} seconds: More than 2 (By.CssSelector: tr) were found in the searched context.");
        }
        
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void MaximumElementWaitUsingByTimesOutAfterExpectedTime(bool useLongWait, int expectedTimeoutInSeconds)
        {
            //Ensure we are using shorter / non default timeouts
            TestPage.TableBlock = new TableBlock(Driver, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            Action act = () => TestPage.TimeoutFailingToWaitForMaxRowsToLoad(useLongWait, true);
            act
                .Should().Throw<WebDriverTimeoutException>()
                .WithMessage(
                    $"Timed out after {expectedTimeoutInSeconds.ToString()} seconds: More than 2 (By.TagName: tr) were found in the searched context.");
        }

        #region SetUpTearDown

        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServiceProvider = ConfigurationModule.GetServiceProvider(true);
            Driver.Url = TestSettings.TestPageUriString;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DriverManager?.Quit();
            DriverManager?.Dispose();
        }

        #endregion

    }
}