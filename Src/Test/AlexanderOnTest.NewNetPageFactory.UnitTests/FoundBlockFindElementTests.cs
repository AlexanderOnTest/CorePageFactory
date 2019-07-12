using System;
using System.Collections.Generic;
using System.Linq;
using AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public class FoundBlockFindElementTests : AbstractPageBlockTests
    {
        private Dictionary<LocatorType, Func<TestBlock, IWebElement>> findElementMethodLookup;
        private IWebElement elementReturnedByRootElement;
        private IWebElement rootElement;

        [OneTimeSetUp]
        public void Setup()
        {
            rootElement = A.Fake<IWebElement>();
            this.elementReturnedByRootElement = A.Fake<IWebElement>();
            A.CallTo(rootElement).WithReturnType<IWebElement>().Returns(elementReturnedByRootElement);

            A.CallTo(() => this.rootElement.Text).Returns("This is The found rootElement");
            A.CallTo(() => this.elementReturnedByDriver.Text).Returns(AtomicMessage);
            A.CallTo(() => this.elementReturnedByRootElement.Text).Returns(ChainedMessage);

            this.findElementMethodLookup = new Dictionary<LocatorType, Func<TestBlock, IWebElement>>
            {
                {LocatorType.CssSelector, (definedBlock) => definedBlock.CssSelectorElement()},
                {LocatorType.Id, (definedBlock) => definedBlock.IdElement()},
                {LocatorType.ClassName, (definedBlock) => definedBlock.ClassNameElement()},
                {LocatorType.TagName, (definedBlock) => definedBlock.TagNameElement()},
                {LocatorType.Name, (definedBlock) => definedBlock.NameElement()},
                {LocatorType.LinkText, (definedBlock) => definedBlock.LinkTextElement()},
                {LocatorType.PartialLinkText, (definedBlock) => definedBlock.PartialLinkTextElement()},
                {LocatorType.XPath, (definedBlock) => definedBlock.XPathElement()},
                {LocatorType.String, (definedBlock) => definedBlock.StringElement()}
            };
        }

        [Test]
        [Category("Unit")]
        public void UsingFindElement_CallsAreAlwaysChainedWithAFoundRootElement(
            [Values] LocatorType childLocatorType)
        {
            // Arrange
            TestBlock block = new TestBlock(this.rootElement);

            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            IWebElement element = method(block);

            // Log
            TestContext.WriteLine(element.Text);

            // Assert
            using (new AssertionScope())
            {
                element.Should().Be(this.elementReturnedByRootElement, "the wrong IWebElement was returned.");
            }
        }

        [Test]
        [Category("Unit")]
        public void UsingFindElement_NoCallIsMadeFromTheDriverWithAFoundRootElement(
           [Values] LocatorType childLocatorType)
        {
            // Arrange
            TestBlock block = new TestBlock(this.rootElement);

            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            IWebElement element = method(block);

            // Log
            TestContext.WriteLine(element.Text);
            var driverCalls = Fake.GetCalls(this.driver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {driverCalls.Count} call(s) to the IWebDriver.");
            foreach (var call in driverCalls)
            {
                TestContext.WriteLine($"The method called on the IWebDriver was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            using (new AssertionScope())
            {
                driverCalls.Count.Should().Be(0);
            }
        }

        [Test]
        [Category("Unit")]
        public void UsingFindElement_ACallIsAlwaysMadeFromTheRootElementWithAFoundRootElement(
            [Values] LocatorType childLocatorType
            )
        {
            // Arrange
            TestBlock block = new TestBlock(this.rootElement);

            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            _ = method(block);

            // Log
            var rootElementCalls = Fake.GetCalls(this.rootElement).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {rootElementCalls.Count} call(s) to the returned Element.");
            foreach (var call in rootElementCalls)
            {
                TestContext.WriteLine($"The Call made to the RootElement was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            rootElementCalls.Count.Should().Be(1);
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(this.rootElement);
        }
    }
}
