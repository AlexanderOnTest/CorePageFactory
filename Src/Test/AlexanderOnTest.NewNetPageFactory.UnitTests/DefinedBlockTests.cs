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
    public class DefinedBlockTests
    {
        static readonly object[] SubAtomicByCases =
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

        static readonly object[] NonAtomicByCases =
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
        private Dictionary<LocatorType, Func<AtomicDefinedBlock, IWebElement>> findMethodLookup;

        private IWebDriver driver;
        private IWebElement elementReturnedByDriver;
        private IWebElement elementReturnedByFoundElement;
        private const string AtomicMessage = "This was an Atomic call";
        private const string ChainedMessage = "This was a chained call";

         [OneTimeSetUp]
        public void Setup()
        {
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


            this.findMethodLookup = new Dictionary<LocatorType, Func<AtomicDefinedBlock, IWebElement >>
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

        #region ConstructorTests

        [Test]
        [Category("Unit")]
        public void DefinedBlockFromString_PreferAtomicValueIsCorrect()
        {
            AtomicDefinedBlock block = new AtomicDefinedBlock("cssSelector", driver);

            (block.PreferAtomic).Should().Be(true);
        }

        [TestCaseSource("SubAtomicByCases")]
        [Category("Unit")]
        public void DefinedBlockFromBy_PreferAtomicValueIsCorrect(LocatorType rootLocatorType, bool expectedPreferAtomic)
        {
            AtomicDefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            (block.PreferAtomic).Should().Be(expectedPreferAtomic);
        }

        #endregion

        [Test]
        [Category("Unit")]
        public void AnAtomicCallIsMadeWhenBothLocatorsAreSubAtomic(
            [Values] LocatorType rootLocatorType,
            [Values] LocatorType childLocatorType
            )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            AtomicDefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findMethodLookup.TryGetValue(childLocatorType, out Func< AtomicDefinedBlock, IWebElement> method);

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
        public void TheCorrectCallIsMadeFromTheDriver(
           [Values] LocatorType rootLocatorType,
           [Values] LocatorType childLocatorType
           )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            AtomicDefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);
            this.findMethodLookup.TryGetValue(childLocatorType, out Func<AtomicDefinedBlock, IWebElement> method);

            // Act
            IWebElement element = method(block);

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
                driverCalls.Count().Should().Be(1);
                if (expectedAtomicCall)
                {
                    driverCalls[0].ArgumentsAfterCall[0].ToString().Should().StartWith("By.CssSelector");
                }
            }
        }

        [Test]
        [Category("Unit")]
        public void ACallIsMadeFromTheReturnedElementOnlyOnNonAtomicCalls(
            [Values] LocatorType rootLocatorType,
            [Values] LocatorType childLocatorType
            )
        {
            // Arrange
            bool expectedAtomicCall = rootLocatorType.IsSubAtomic() && childLocatorType.IsSubAtomic();
            AtomicDefinedBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            this.findMethodLookup.TryGetValue(childLocatorType, out Func<AtomicDefinedBlock, IWebElement> method);

            // Act
            IWebElement element = method(block);

            // Log
            var returnedElementCalls = Fake.GetCalls(this.elementReturnedByDriver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
            TestContext.WriteLine($"There were {returnedElementCalls.Count} call(s) to the returned Element.");
            foreach (var call in returnedElementCalls)
            {
                TestContext.WriteLine($"The Call made to the returnedElement was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
            }

            // Assert
            returnedElementCalls.Count().Should().Be(expectedAtomicCall ? 0 : 1);
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(this.driver);
            Fake.ClearRecordedCalls(this.elementReturnedByDriver);
        }

        private AtomicDefinedBlock GetBlockDefinedByLocatorType(LocatorType rootLocatorType)
        {
            this.byLookup.TryGetValue(rootLocatorType, out By by);
            if (by != null)
            {
                return new AtomicDefinedBlock(by, driver);
            } else
                return new AtomicDefinedBlock("css", driver);
        }



        private void AssertCorrectTypeOfCallWasMade(IWebElement element, bool expectedAtomicCall)
        {
            using (new AssertionScope())
            {
                TestContext.WriteLine(element.Text);
                element.Should().Be(expectedAtomicCall ? this.elementReturnedByDriver : this.elementReturnedByFoundElement,
                    "the wrong IWebElement was returned.");
                element.Text.Should().Be(expectedAtomicCall ? AtomicMessage : ChainedMessage);


                var dcList = Fake.GetCalls(this.driver).ToList();
                var driverCalls = Fake.GetCalls(this.driver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
                TestContext.WriteLine($"The Call made to the driver was {driverCalls[0].Method.Name}({driverCalls[0].ArgumentsAfterCall[0]})");


                var recList = Fake.GetCalls(this.elementReturnedByDriver).ToList();
                var returnedElementCalls = Fake.GetCalls(this.elementReturnedByDriver).Where((c) => c.Method.Name.StartsWith("FindElement")).ToList();
                TestContext.WriteLine($"There were {returnedElementCalls.Count} call(s) to the returned Element.");
                foreach (var call in returnedElementCalls)
                {
                    TestContext.WriteLine($"The Call made to the returnedElement was {call.Method.Name}({call.ArgumentsAfterCall[0]})");
                }

                driverCalls.Count().Should().Be(1);

                if (expectedAtomicCall)
                {
                    driverCalls[0].ArgumentsAfterCall[0].ToString().Should().StartWith("By.CssSelector");
                    returnedElementCalls.Count().Should().Be(0);
                }
                else
                {
                    //driverCalls[0].ArgumentsAfterCall[0].ToString().Should().NotStartWith("By.CssSelector");
                    returnedElementCalls.Count().Should().Be(1);
                }
            }
        }
    }
}
