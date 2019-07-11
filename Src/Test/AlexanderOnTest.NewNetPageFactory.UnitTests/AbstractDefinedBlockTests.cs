using System.Collections.Generic;
using AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks;
using FakeItEasy;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public abstract class AbstractDefinedBlockTests
    {
        protected Dictionary<LocatorType, By> byLookup;
        protected readonly string AtomicMessage = "This was an Atomic call";
        protected readonly string ChainedMessage = "This was a chained call";

        protected IWebDriver driver;
        protected IWebElement elementReturnedByDriver;

        [OneTimeSetUp]
        public void SuperSetup()
        {
            this.driver = A.Fake<IWebDriver>();

            this.elementReturnedByDriver = A.Fake<IWebElement>();
            A.CallTo(this.driver).WithReturnType<IWebElement>().Returns(elementReturnedByDriver);

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

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(this.driver);
            Fake.ClearRecordedCalls(this.elementReturnedByDriver);
        }

        protected AtomicDefinedBlock GetBlockDefinedByLocatorType(LocatorType rootLocatorType)
        {
            if (rootLocatorType == LocatorType.String)
            {
                return new AtomicDefinedBlock("css", driver);
            }
            else
            {
                this.byLookup.TryGetValue(rootLocatorType, out By by);
                return new AtomicDefinedBlock(by, driver);
            }
        }
    }
}
