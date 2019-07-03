using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NewNetPageFactory.Utilities;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public class ByTests
    {
        //http://book.theautomatedtester.co.uk/
        private Dictionary<string, By> cases;
        private const string CssSelector = "#q";
        private const string Id = "q";
        private const string ClassName = "mainbody";
        private const string TagName = "input";
        private const string LinkText = "Chapter 8";
        private const string PartialLinkText = "8";
        private const string XPath = "/html/body/div[2]/ul/li[3]/a";
        //http://book.theautomatedtester.co.uk/chapter2
        private const string Name = "but2";

        private IServiceProvider serviceProvider;
        private IWebDriverManager driverManager;
        private IWebDriver driver;


        [OneTimeSetUp]
        public void Setup()
        {
            IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(true, WebDriverSettings.WebDriverConfiguration);

            serviceProvider = serviceCollection.BuildServiceProvider();
            this.driverManager = serviceProvider.GetService<IWebDriverManager>();

            cases = new Dictionary<string, By>
            {
                {"CssSelector", By.CssSelector(CssSelector)},
                {"Id", By.Id(Id)},
                {"ClassName", By.ClassName(ClassName)},
                {"TagName", By.TagName(TagName)},
                {"Name", By.Name(Name)},
                {"LinkText", By.LinkText(LinkText)},
                {"PartialLinkText", By.PartialLinkText(PartialLinkText)},
                {"XPath", By.XPath(XPath)}
            };
        }

        [TestCase("CssSelector", CssSelector, true)]
        [TestCase("Id", Id, true)]
        [TestCase("ClassName", ClassName, true)]
        [TestCase("TagName", TagName, true)]
        [TestCase("LinkText", LinkText, false)]
        [TestCase("PartialLinkText", PartialLinkText, false)]
        [TestCase("XPath", XPath, false)]
        [TestCase("Name", Name, true)]
        [Category("Unit")]
        public void LocatorDetailsAreParsedCorrectlyFromBy(string locatorType, string locatorValue, bool isAtomic)
        {
            cases.TryGetValue(locatorType, out By by);

            by.GetLocatorDetails()
                .Should()
                .BeEquivalentTo((locatorType, locatorValue, isAtomic));
        }

        [TestCase("CssSelector", CssSelector, CssSelector)]
        [TestCase("Id", Id, CssSelector)]
        [TestCase("ClassName", ClassName, ".mainbody")]
        [TestCase("TagName", TagName, TagName)]
        [TestCase("Name", Name, "*[name=\"but2\"]")]
        public void CanCorrectlyConvertToCssSelector(string locatorType, string locatorValue, string expectedCssSelector)
        {
            cases.TryGetValue(locatorType, out By by);

            ByExtensions.GetAtomicCssLocator(by.GetLocatorDetails()).Should().Be(expectedCssSelector);
        }


        [TestCase("LinkText")]
        [TestCase("PartialLinkText")]
        [TestCase("XPath")]
        public void SelectorConverterThrowsForIncorrectLocatorType(string locatorType)
        {

            cases.TryGetValue(locatorType, out By by);

            Action conversion = () => ByExtensions.GetAtomicCssLocator(by.GetLocatorDetails());
            conversion.Should().ThrowExactly<ArgumentException>()
                .WithMessage($"'By's of type {locatorType} cannot be converted to use a CssSelector.");
        }

        [TearDown]
        public void TearDown()
        {
            this.driverManager?.Quit();
        }
    }
}