using System.Collections.Generic;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
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
        static object[] SubAtomicByCases =
        {
            new object[] { LocatorType.CssSelector, true },
            new object[] { LocatorType.Id, true },
            new object[] { LocatorType.ClassName, true },
            new object[] { LocatorType.TagName, true },
            new object[] { LocatorType.Name, true },
            new object[] { LocatorType.LinkText, false },
            new object[] { LocatorType.PartialLinkText, false },
            new object[] { LocatorType.XPath, false },
            new object[] { LocatorType.String, true },
        };

        static object[] NonAtomicByCases =
        {
            new object[] { LocatorType.CssSelector, false },
            new object[] { LocatorType.Id, false },
            new object[] { LocatorType.ClassName, false },
            new object[] { LocatorType.TagName, false },
            new object[] { LocatorType.Name, false },
            new object[] { LocatorType.LinkText, false },
            new object[] { LocatorType.PartialLinkText, false },
            new object[] { LocatorType.XPath, false },
            new object[] { LocatorType.String, false },
        };

        private Dictionary<LocatorType, By> byLookup;

        private IWebDriver driver;
        private IWebElement elementReturnedByDriver;
        private IWebElement elementReturnedByFoundElement;
        private const string AtomicMessage = "This was an Atomic call";
        private const string ChainedMessage = "This was a chained call";

         [OneTimeSetUp]
        public void Setup()
        {
            IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(true, WebDriverSettings.WebDriverConfiguration);

            this.driver = A.Fake<IWebDriver>();

            this.elementReturnedByDriver = A.Fake<IWebElement>();
            this.elementReturnedByFoundElement = A.Fake<IWebElement>();
            A.CallTo(this.driver).WithReturnType<IWebElement>().Returns(elementReturnedByDriver);
            A.CallTo(elementReturnedByDriver).WithReturnType<IWebElement>().Returns(elementReturnedByFoundElement);

            A.CallTo(() => this.elementReturnedByDriver.Text).Returns(AtomicMessage);
            A.CallTo(() => this.elementReturnedByFoundElement.Text).Returns(ChainedMessage);

            this.byLookup = new Dictionary<LocatorType, By>
            {
                {LocatorType.CssSelector, By.CssSelector("CssSelector")},
                {LocatorType.Id, By.Id("Id")},
                {LocatorType.ClassName, By.ClassName("ClassName")},
                {LocatorType.TagName, By.TagName("TagName")},
                {LocatorType.Name, By.Name("Name")},
                {LocatorType.LinkText, By.LinkText("LinkText")},
                {LocatorType.PartialLinkText, By.PartialLinkText("PartialLinkText")},
                {LocatorType.XPath, By.XPath("XPath")},
                {LocatorType.String, null}
            };
        }

        [Test]
        [Category("Unit")]
        public void DefinedBlockFromString_PreferAtomicValueIsCorrect()
        {
            DefinedBlock block = new DefinedBlock("cssSelector", driver);

            (block.preferAtomic).Should().Be(true);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void DefinedBlockFromBy_PreferAtomicValueIsCorrect(LocatorType rootLocatorType, bool expectedPreferAtomic)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            (block.preferAtomic).Should().Be(expectedPreferAtomic);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromCssSelectorChild(
            LocatorType rootLocatorType, 
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.CssSelectorElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromNameChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.NameElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedTagNameChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.TagNameElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromClassNameChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.ClassNameElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromIdChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.IdElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("NonAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromXPathChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.XPathElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("NonAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromLinkTextChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.LinkTextElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        [TestCaseSource("NonAtomicByCases")]
        [Category("Unit")]
        public void AtomicCallIsMadeWhenExpectedFromPartialLinkTextChild(
            LocatorType rootLocatorType,
            bool expectedAtomicCall)
        {
            DefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            IWebElement element = block.PartialLinkTextElement();
            AssertCorrectTypeOfCallWasMade(element, expectedAtomicCall);
        }

        private DefinedBlock GetBlockDefinedByLocatorType(LocatorType rootLocatorType)
        {
            this.byLookup.TryGetValue(rootLocatorType, out By by);
            if (by != null)
            {
                return new DefinedBlock(by, driver);
            } else
                return new DefinedBlock("css", driver);
        }

        private void AssertCorrectTypeOfCallWasMade(IWebElement element, bool expectedAtomicCall)
        {
            using (new AssertionScope())
            {
                TestContext.WriteLine(element.Text);
                element.Should().Be(expectedAtomicCall ? this.elementReturnedByDriver : this.elementReturnedByFoundElement,
                    "the wrong IWebElement was returned.");
                element.Text.Should().Be(expectedAtomicCall ? AtomicMessage : ChainedMessage);
            }
        }
    }
}
