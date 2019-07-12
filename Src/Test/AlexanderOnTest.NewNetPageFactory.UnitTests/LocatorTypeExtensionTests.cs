using FluentAssertions;
using NUnit.Framework;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public class LocatorTypeExtensionTests
    {
        [TestCase(LocatorType.CssSelector, true)]
        [TestCase(LocatorType.ClassName, true)]
        [TestCase(LocatorType.Id, true)]
        [TestCase(LocatorType.LinkText, false)]
        [TestCase(LocatorType.Name, true)]
        [TestCase(LocatorType.PartialLinkText, false)]
        [TestCase(LocatorType.String, true)]
        [TestCase(LocatorType.TagName, true)]
        [TestCase(LocatorType.XPath, false)]
        public void IsSubAtomicReturnsExpectedValue(LocatorType locatorType, bool isSubatomic)
        {
            locatorType.IsSubAtomic().Should().Be(isSubatomic);
        }

        [TestCase(LocatorType.CssSelector, "locator")]
        [TestCase(LocatorType.ClassName, ".locator")]
        [TestCase(LocatorType.Id, "#locator")]
        [TestCase(LocatorType.Name, "*[name=\"locator\"]")]
        [TestCase(LocatorType.String, "locator")]
        [TestCase(LocatorType.TagName, "locator")]
        public void ConvertToCssSelectorFuncReturnGeneratesValidCssSelector(LocatorType locatorType, string expectedCssLocator)
        {
            var conversion = locatorType.ConvertToCssSelectorFunc();
                conversion("locator").Should().Be(expectedCssLocator);
        }

        [TestCase(LocatorType.LinkText)]
        [TestCase(LocatorType.PartialLinkText)]
        [TestCase(LocatorType.XPath)]
        public void ConvertToCssSelectorFuncReturnsNullForInvalidLocatorTypes(LocatorType locatorType)
        {
            locatorType.ConvertToCssSelectorFunc().Should().BeNull();
        }
    }
}
