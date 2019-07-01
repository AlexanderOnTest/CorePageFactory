using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        private Dictionary<string, By> cases;
        private const string CssSelector = "#q";
        private const string Id = "q";
        private const string ClassName = "mainbody";
        private const string TagName = "input";
        private const string Name = "but2";
        private const string LinkText = "Chapter 8";
        private const string PartialLinkText = "8";
        private const string XPath = "/html/body/div[2]/ul/li[3]/a";

        private IServiceProvider serviceProvider;
        private IWebDriverManager driverManager;
        private IWebDriver driver;


        [OneTimeSetUp]
        public void Setup()
        {
            //IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(true, WebDriverSettings.WebDriverConfiguration);

            //serviceProvider = serviceCollection.BuildServiceProvider();
            //this.driverManager = serviceProvider.GetService<IWebDriverManager>();

            cases = new Dictionary<string, By>();
            cases.Add("CssSelector", By.CssSelector(CssSelector));
            cases.Add("Id", By.Id(Id));
            cases.Add("ClassName", By.ClassName(ClassName));
            cases.Add("TagName", By.TagName(TagName));
            cases.Add("Name", By.Name(Name));
            cases.Add("LinkText", By.LinkText(LinkText));
            cases.Add("PartialLinkText", By.PartialLinkText(PartialLinkText));
            cases.Add("XPath", By.XPath(XPath));
        }

        //http://book.theautomatedtester.co.uk/
        [TestCase("CssSelector", CssSelector)]
        [TestCase("Id", Id)]
        [TestCase("ClassName", ClassName)]
        [TestCase("TagName", TagName)]
        [TestCase("LinkText", LinkText)]
        [TestCase("PartialLinkText", PartialLinkText)]
        [TestCase("XPath", XPath)]
        //http://book.theautomatedtester.co.uk/chapter2
        [TestCase("Name", Name)]
        //http://book.theautomatedtester.co.uk/
        public void LocatorValuesCanBeParsed(string locatorType, string locatorValue)
        {
            cases.TryGetValue(locatorType, out By by);
            string locatorDescription = (locatorType != "ClassName") ? locatorType : $"{locatorType}[Contains]";
            using (new AssertionScope())
            {
                by.ToString().Should().Be($"By.{locatorDescription}: {locatorValue}");
                GetLocatorValueByRegex(by).Should().Be(locatorValue);
                GetLocatorValueBySubString(by).Should().Be(locatorValue);

                GetLocatorTypeByRegex(by).Should().Be(locatorType);
                GetLocatorTypeBySubString(by).Should().Be(locatorType);
            }
        }

        //http://book.theautomatedtester.co.uk/
        [TestCase("CssSelector", CssSelector)]
        [TestCase("Id", Id)]
        [TestCase("ClassName", ClassName)]
        [TestCase("TagName", TagName)]
        [TestCase("LinkText", LinkText)]
        [TestCase("PartialLinkText", PartialLinkText)]
        [TestCase("XPath", XPath)]
        //http://book.theautomatedtester.co.uk/chapter2
        [TestCase("Name", Name)]
        //http://book.theautomatedtester.co.uk/
        public void LocatorValuesCanBeParsedInOne(string locatorType, string locatorValue)
        {
            cases.TryGetValue(locatorType, out By by);
            using (new AssertionScope())
            {
                (string locatorType, string locatorValue) byDetails = GetLocatorDetails(by);

                byDetails.locatorValue.Should().Be(locatorValue);
                byDetails.locatorType.Should().Be(locatorType);
            }
        }

        [TearDown]
        public void TearDown()
        {
            //this.driverManager?.Quit();
        }

        private string GetLocatorValueByRegex(By by)
        {
            return Regex.Match(by.ToString(), "[^ ]* (.*)").Groups[1].Value;
        }

        private string GetLocatorValueBySubString(By by)
        {
            string toString = by.ToString();
            return toString.Substring(toString.IndexOf(' ') + 1); ;
        }

        private string GetLocatorTypeByRegex(By by)
        {
            return Regex.Match(by.ToString(), "[^By.][A-Z][^:]").Groups[1].Value;
        }

        private string GetLocatorTypeBySubString(By by)
        {
            string toString = by.ToString();
            return toString.Substring(3, toString.IndexOf(':') - 3)
                .Replace("[Contains]", string.Empty); 
        }

        private (string locatorType, string locatorValue) GetLocatorDetails(By by)
        {
            string toString = by.ToString();
            string cleanToString = toString.Replace("[Contains]", string.Empty);
            var tokens = cleanToString.Split(": ");
            return (tokens[0].Substring(3), tokens[1]);
        } 
    }
}