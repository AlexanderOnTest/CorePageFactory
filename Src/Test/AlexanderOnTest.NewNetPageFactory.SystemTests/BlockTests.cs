using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class BlockTests
    {
        private IWebDriver Driver { get; set; }

        private IWebDriverManager DriverManager { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        [Test]
        public void GetRootElementReturnsCorrectElementInCssSelectorDefinedBlock()
        {
            By rootElementBy = By.CssSelector(TableBlock.CustomerTableRootElementCssSelector);
            
            WebDriverWait wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(5));
            IWebElement expectedRootElement = wait.Until(
                driver => driver.FindElement(rootElementBy));
            
            // TableBlock(IWebDriver.....) constructor calls the BlockController(string rootCssSelector....) constructor
            TableBlock tableBlock = 
                new TableBlock(this.Driver);
            
            tableBlock.GetRootElement().Should().Be(expectedRootElement);
        }
        
        [Test]
        public void GetRootElementReturnsCorrectElementInByDefinedBlock()
        {
            By rootElementBy = By.CssSelector(TableBlock.CustomerTableRootElementCssSelector);
            
            WebDriverWait wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(5));
            IWebElement expectedRootElement = wait.Until(
                driver => driver.FindElement(rootElementBy));
            
            TableBlock tableBlock = 
                new TableBlock(rootElementBy, this.Driver);
            
            tableBlock.GetRootElement().Should().Be(expectedRootElement);
        }

        [Test]
        public void GetRootElementReturnsCorrectElementInRootElementDefinedBlock()
        {
            WebDriverWait wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(5));
            IWebElement rootElement = wait.Until(
                driver => driver.FindElement(By.CssSelector(TableBlock.CustomerTableRootElementCssSelector)));
            TableBlock tableBlock = new TableBlock(rootElement, this.Driver);

            tableBlock.GetRootElement().Should().Be(rootElement);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void FindElementsWithWaitForMinimumElementsReturnsCorrectList(bool useBy)
        {
            TableBlock tableBlock = new TableBlock(this.Driver);

            tableBlock.WaitForMinimumRowsToLoadAndReturn(2, false, useBy).Should().HaveCount(13);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void FindElementsWithWaitForMaximumElementsReturnsCorrectList(bool useBy)
        {
            TableBlock tableBlock = new TableBlock(this.Driver);

            tableBlock.WaitForMaximumRowsToLoadAndReturn(14, false, useBy).Should().HaveCount(13);
        }
        
        #region SetUpTearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServiceProvider = ConfigurationModule.GetServiceProvider(true);
            DriverManager = ServiceProvider.GetRequiredService<IWebDriverManager>();
            Driver = ServiceProvider.GetRequiredService<IWebDriver>();
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