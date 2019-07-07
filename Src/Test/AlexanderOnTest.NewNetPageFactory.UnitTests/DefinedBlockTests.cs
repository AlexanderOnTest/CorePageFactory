using System;
using System.Collections.Generic;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public class DefinedBlockTests
    {
        static object[] SubAtomicChildByCases =
        {
            new object[] { LocatorType.CssSelector, true },
            new object[] { LocatorType.Id, true },
            new object[] { LocatorType.ClassName, true },
            new object[] { LocatorType.TagName, true },
            new object[] { LocatorType.Name, true },
            new object[] { LocatorType.LinkText, false },
            new object[] { LocatorType.PartialLinkText, false },
            new object[] { LocatorType.XPath, false },
        };

        //http://book.theautomatedtester.co.uk/
        private Dictionary<LocatorType, By> cases;
        private Dictionary<LocatorType, Func<string, By>> byConstructorsDictionary;

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
        private IWebElement firstFakeElement;
        private IWebElement secondFakeElement;
        private const string AtomicMessage = "This was an Atomic call";
        private const string ChainedMessage = "This was a chained call";

         [OneTimeSetUp]
        public void Setup()
        {
            IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(true, WebDriverSettings.WebDriverConfiguration);

            serviceProvider = serviceCollection.BuildServiceProvider();
            this.driverManager = serviceProvider.GetService<IWebDriverManager>();

            this.driver = A.Fake<IWebDriver>();
            IWebElement fakeElement = A.Fake<IWebElement>();
            A.CallTo(this.driver).WithReturnType<IWebElement>().Returns(fakeElement);
            A.CallTo(fakeElement).WithReturnType<IWebElement>().Returns(A.Fake<IWebElement>());

            this.firstFakeElement = A.Fake<IWebElement>();
            this.secondFakeElement = A.Fake<IWebElement>();
            A.CallTo(this.driver).WithReturnType<IWebElement>().Returns(firstFakeElement);
            A.CallTo(firstFakeElement).WithReturnType<IWebElement>().Returns(secondFakeElement);

            A.CallTo(() => firstFakeElement.Text).Returns(AtomicMessage);
            A.CallTo(() => secondFakeElement.Text).Returns(ChainedMessage);

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



            byConstructorsDictionary = new Dictionary<LocatorType, Func<string, By>>
            {
                {LocatorType.CssSelector, By.CssSelector},
                {LocatorType.Id, By.Id},
                {LocatorType.ClassName, By.ClassName},
                {LocatorType.TagName, By.TagName},
                {LocatorType.Name, By.Name},
                {LocatorType.LinkText, By.LinkText},
                {LocatorType.PartialLinkText, By.PartialLinkText},
                {LocatorType.XPath, By.XPath}
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
        public void DefinedBlockUseByIsCorrect(LocatorType locatorType, bool expectedPreferAtomic)
        {
            cases.TryGetValue(locatorType, out By by);

            DefinedBlock block = new DefinedBlock(by, driver);

            (block.preferAtomic).Should().Be(expectedPreferAtomic);
        }

        [TestCaseSource("SubAtomicChildByCases")]
        [Category("Unit")]
        public void CssSelectorChildGeneratesAtomicCallWhenExpected(
            LocatorType locatorType, 
            bool expectedAtomicCall)
        {
            cases.TryGetValue(locatorType, out By by);

            DefinedBlock block = new DefinedBlock(by, driver);

            IWebElement element = block.CssSelectorElement();

            using (new AssertionScope())
            {
                TestContext.WriteLine(element.Text);
                element.Should().Be(expectedAtomicCall ? this.firstFakeElement : this.secondFakeElement);
                element.Text.Should().Be(expectedAtomicCall ? AtomicMessage : ChainedMessage);
            }
        }


        [TestCaseSource("SubAtomicChildByCases")]
        [Category("Unit")]
        public void NameChildGeneratesAtomicCallWhenExpected(
            LocatorType locatorType,
            bool expectedAtomicCall)
        {
            cases.TryGetValue(locatorType, out By by);

            DefinedBlock block = new DefinedBlock(by, driver);

            IWebElement element = block.NameElement();

            using (new AssertionScope())
            {
                TestContext.WriteLine(element.Text);
                element.Should().Be(expectedAtomicCall ? this.firstFakeElement : this.secondFakeElement);
                element.Text.Should().Be(expectedAtomicCall ? AtomicMessage : ChainedMessage);
            }
        }

        [TearDown]
        public void TearDown()
        {
            this.driverManager?.Quit();
        }
    }
}
