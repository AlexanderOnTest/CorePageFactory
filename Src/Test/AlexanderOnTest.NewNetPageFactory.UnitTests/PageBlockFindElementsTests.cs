using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class PageBlockFindElementsTests : BlockTests
    {
        private Dictionary<LocatorType, Func<TestBlock, ReadOnlyCollection<IWebElement>>>
            findElementsMethodLookup;

        private ReadOnlyCollection<IWebElement> elementsReturnedByDriver;
        private IWebElement elementInElementsReturnedByDriver;
        private ReadOnlyCollection<IWebElement> elementsReturnedByFoundElement;
        private IWebElement elementInElementsReturnedByFoundElement;

        [OneTimeSetUp]
        public void Setup()
        {
            elementInElementsReturnedByDriver = A.Fake<IWebElement>();
            A.CallTo(() => elementInElementsReturnedByDriver.Text).Returns(AtomicMessage);

            this.elementsReturnedByDriver = new ReadOnlyCollection<IWebElement>(new List<IWebElement>
                    {elementInElementsReturnedByDriver});

            elementInElementsReturnedByFoundElement = A.Fake<IWebElement>();
            A.CallTo(() => elementInElementsReturnedByFoundElement.Text).Returns(ChainedMessage);

            this.elementsReturnedByFoundElement = new ReadOnlyCollection<IWebElement>(new List<IWebElement>
                    {elementInElementsReturnedByFoundElement});

            A.CallTo(elementReturnedByDriver).WithReturnType<ReadOnlyCollection<IWebElement>>()
                .Returns(elementsReturnedByFoundElement);

            A.CallTo(driver).WithReturnType<ReadOnlyCollection<IWebElement>>()
                .Returns(elementsReturnedByDriver);

            this.findElementsMethodLookup =
                new Dictionary<LocatorType, Func<TestBlock, ReadOnlyCollection<IWebElement>>>
                {
                        {LocatorType.CssSelector, definedBlock => definedBlock.CssSelectorElements()},
                        {LocatorType.Id, definedBlock => definedBlock.IdElements()},
                        {LocatorType.ClassName, definedBlock => definedBlock.ClassNameElements()},
                        {LocatorType.TagName, definedBlock => definedBlock.TagNameElements()},
                        {LocatorType.Name, definedBlock => definedBlock.NameElements()},
                        {LocatorType.LinkText, definedBlock => definedBlock.LinkTextElements()},
                        {LocatorType.PartialLinkText, definedBlock => definedBlock.PartialLinkTextElements()},
                        {LocatorType.XPath, definedBlock => definedBlock.XPathElements()},
                        {LocatorType.String, definedBlock => definedBlock.StringElements()}
                };
        }

        [Test]
        public void UsingFindElements_AnAtomicCallIsMadeWhenBothLocatorsAreSubAtomic(
            [Values] LocatorType rootLocatorType,
            [Values] LocatorType childLocatorType)
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findElementsMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, ReadOnlyCollection<IWebElement>> method);

            // Act
            ReadOnlyCollection<IWebElement> elements = method(block);

            // Log
            TestContext.WriteLine(elements[0]?.Text);

            // Assert
            using (new AssertionScope())
            {
                elements.Count.Should().Be(1);
                elements[0].Should().Be(expectedAtomicCall ?
                        this.elementInElementsReturnedByDriver :
                        this.elementInElementsReturnedByFoundElement,
                    "the wrong IWebElement was returned.");
            }
        }

        [Test]
        public void UsingFindElements_TheCorrectCallIsMadeFromTheDriver(
           [Values] LocatorType rootLocatorType,
           [Values] LocatorType childLocatorType
           )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findElementsMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, ReadOnlyCollection<IWebElement>> method);

            // Act
            ReadOnlyCollection<IWebElement> elements = method(block);

            // Log
            var driverCalls = Fake.GetCalls(this.driver).Where(c => c.Method.Name.StartsWith("FindElement")).ToList();
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
                    driverCalls[0].Method.Name.Should().Be("FindElements");
                    driverCalls[0].ArgumentsAfterCall[0].ToString().Should().StartWith("By.CssSelector");
                }
                else
                {
                    driverCalls[0].Method.Name.Should().Be("FindElement");
                }
            }
        }

        [Test]
        public void UsingFindElements_AFindElementsCallIsMadeFromTheReturnedElementOnlyOnNonAtomicCalls(
            [Values] LocatorType rootLocatorType,
            [Values] LocatorType childLocatorType
            )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findElementsMethodLookup.TryGetValue(childLocatorType, out Func<TestBlock, ReadOnlyCollection<IWebElement>> method);

            // Act
            ReadOnlyCollection<IWebElement> elements = method(block);

            // Log
            var returnedElementCalls = Fake.GetCalls(this.elementReturnedByDriver).Where(c => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {returnedElementCalls.Count} call(s) to the returned Element.");
            foreach (var call in returnedElementCalls)
            {
                TestContext.WriteLine($"The Call made to the returnedElement was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            using (new AssertionScope())
            {
                if (expectedAtomicCall)
                {
                    returnedElementCalls.Count.Should().Be(0);
                }
                else
                {
                    returnedElementCalls.Count.Should().Be(1);
                    returnedElementCalls[0].Method.Name.Should().Be("FindElements");
                }

                
            }
        }

    }
}
