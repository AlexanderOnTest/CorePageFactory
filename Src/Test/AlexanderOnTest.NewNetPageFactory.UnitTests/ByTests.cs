using System;
using System.Collections.Generic;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
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
        private Dictionary<LocatorType, By> cases;
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

            cases = new Dictionary<LocatorType, By>
            {
                {LocatorType.CssSelector, By.CssSelector(CssSelector)},
                {LocatorType.Id, By.Id(Id)},
                {LocatorType.ClassName, By.ClassName(ClassName)},
                {LocatorType.TagName, By.TagName(TagName)},
                {LocatorType.Name, By.Name(Name)},
                {LocatorType.LinkText, By.LinkText(LinkText)},
                {LocatorType.PartialLinkText, By.PartialLinkText(PartialLinkText)},
                {LocatorType.XPath, By.XPath(XPath)}
            };
        }

        [TestCase(LocatorType.CssSelector, true)]
        [TestCase(LocatorType.Id, true)]
        [TestCase(LocatorType.ClassName, true)]
        [TestCase(LocatorType.TagName, true)]
        [TestCase(LocatorType.LinkText, false)]
        [TestCase(LocatorType.PartialLinkText, false)]
        [TestCase(LocatorType.XPath, false)]
        [TestCase(LocatorType.Name, true)]
        [Category("Unit")]
        public void LocatorTypeCorrectlyIdentifiesConvertibility(LocatorType locatorType, bool isSubAtomic)
        {
            locatorType.IsSubAtomic().Should().Be(isSubAtomic);
        }

        [TestCase(LocatorType.CssSelector, CssSelector)]
        [TestCase(LocatorType.Id, Id)]
        [TestCase(LocatorType.ClassName, ClassName)]
        [TestCase(LocatorType.TagName, TagName)]
        [TestCase(LocatorType.LinkText, LinkText)]
        [TestCase(LocatorType.PartialLinkText, PartialLinkText)]
        [TestCase(LocatorType.XPath, XPath)]
        [TestCase(LocatorType.Name, Name)]
        [Category("Unit")]
        public void LocatorDetailsAreParsedCorrectlyFromBy(LocatorType locatorType, string locatorValue)
        {
            cases.TryGetValue(locatorType, out By by);

            by.GetLocatorDetail()
                .Should()
                .BeEquivalentTo((locatorType, locatorValue));
        }

        [TestCase(LocatorType.CssSelector, CssSelector, CssSelector)]
        [TestCase(LocatorType.Id, Id, CssSelector)]
        [TestCase(LocatorType.ClassName, ClassName, ".mainbody")]
        [TestCase(LocatorType.TagName, TagName, TagName)]
        [TestCase(LocatorType.Name, Name, "*[name=\"but2\"]")]
        public void SelectorConverterCanCorrectlyConvertToCssSelector(LocatorType locatorType, string locatorValue, string expectedCssSelector)
        {
            cases.TryGetValue(locatorType, out By by);

            ByExtensions.GetEquivalentCssLocator(by.GetLocatorDetail()).Should().Be(expectedCssSelector);
        }

        [TestCase(LocatorType.LinkText)]
        [TestCase(LocatorType.PartialLinkText)]
        [TestCase(LocatorType.XPath)]
        public void SelectorConverterThrowsForIncorrectLocatorType(LocatorType locatorType)
        {
            cases.TryGetValue(locatorType, out By by);

            Action conversion = () => ByExtensions.GetEquivalentCssLocator(by.GetLocatorDetail());
            conversion.Should().ThrowExactly<ArgumentException>()
                .WithMessage($"'By's of type {locatorType} cannot be converted to use a CssSelector.");
        }

        [TestCase(LocatorType.CssSelector, CssSelector, CssSelector, true)]
        [TestCase(LocatorType.Id, Id, CssSelector, true)]
        [TestCase(LocatorType.ClassName, ClassName, ".mainbody", true)]
        [TestCase(LocatorType.TagName, TagName, TagName, true)]
        [TestCase(LocatorType.Name, Name, "*[name=\"but2\"]", true)]
        [TestCase(LocatorType.LinkText, LinkText, null, false)]
        [TestCase(LocatorType.PartialLinkText, PartialLinkText, null, false)]
        [TestCase(LocatorType.XPath, XPath, null, false)]
        public void ByDataObjectHasCorrectValues(
            LocatorType locatorType, 
            string providedLocator, 
            string expectedCssSelector,
            bool expectedIsSubAtomic)
        {
            cases.TryGetValue(locatorType, out By by);

            ByData byData = new ByData(by);

            using (new AssertionScope())
            {
                byData.By.Should().BeEquivalentTo(by);
                byData.LocatorType.Should().Be(locatorType);
                byData.OriginalLocator.Should().Be(providedLocator);
                byData.IsSubAtomic.Should().Be(expectedIsSubAtomic);
                byData.CssLocator.Should().Be(expectedCssSelector);
            }

        }

        [TearDown]
        public void TearDown()
        {
            this.driverManager?.Quit();
        }
    }
}