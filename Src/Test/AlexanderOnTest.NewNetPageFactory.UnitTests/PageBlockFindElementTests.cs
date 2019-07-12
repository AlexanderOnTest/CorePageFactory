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
    public class PageBlockFindElementTests : AbstractPageBlockTests
    {
        private Dictionary<LocatorType, Func<TestBlock, IWebElement>> findElementMethodLookup;
        private IWebElement elementReturnedByFoundElement;

        [OneTimeSetUp]
        public void Setup()
        {
            this.elementReturnedByFoundElement = A.Fake<IWebElement>();
            A.CallTo(elementReturnedByDriver).WithReturnType<IWebElement>().Returns(elementReturnedByFoundElement);

            A.CallTo(() => this.elementReturnedByDriver.Text).Returns(AtomicMessage);
            A.CallTo(() => this.elementReturnedByFoundElement.Text).Returns(ChainedMessage);

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
        public void UsingFindElement_AnAtomicCallIsMadeWhenBothLocatorsAreSubAtomic(
            [Values] LocatorType rootLocatorType, 
            [Values] LocatorType childLocatorType)
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            IWebElement element = method(block);

            // Log
            TestContext.WriteLine(element.Text);

            // Assert
            using (new AssertionScope())
            {
                element.Should().Be(expectedAtomicCall ?
                        this.elementReturnedByDriver :
                        this.elementReturnedByFoundElement,
                    "the wrong IWebElement was returned.");
            }
        }

        [Test]
        [Category("Unit")]
        public void UsingFindElement_TheCorrectCallIsMadeFromTheDriver(
           [Values] LocatorType rootLocatorType,
           [Values] LocatorType childLocatorType)
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            _ = method(block);

            // Log
            var driverCalls = Fake.GetCalls(this.driver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {driverCalls.Count} call(s) to the IWebDriver.");
            foreach (var call in driverCalls)
            {
                TestContext.WriteLine($"The method called on the IWebDriver was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            using (new AssertionScope())
            {
                driverCalls.Count.Should().Be(1);
                if (expectedAtomicCall)
                {
                    driverCalls[0].ArgumentsAfterCall[0].ToString().Should().StartWith("By.CssSelector");
                }
            }
        }

        [Test]
        [Category("Unit")]
        public void UsingFindElement_ACallIsMadeFromTheReturnedElementOnlyOnNonAtomicCalls(
            [Values] LocatorType rootLocatorType,
            [Values] LocatorType childLocatorType
            )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findElementMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, IWebElement> method);

            // Act
            _ = method(block);

            // Log
            var returnedElementCalls = Fake.GetCalls(this.elementReturnedByDriver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {returnedElementCalls.Count} call(s) to the returned Element.");
            foreach (var call in returnedElementCalls)
            {
                TestContext.WriteLine($"The Call made to the returnedElement was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            returnedElementCalls.Count.Should().Be(expectedAtomicCall ? 0 : 1);
        }
    }
}
