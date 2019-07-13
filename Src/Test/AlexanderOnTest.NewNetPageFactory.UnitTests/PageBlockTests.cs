using AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks;
using FluentAssertions;
using NUnit.Framework;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [Category("Unit")]
    [TestFixture]
    public class PageBlockTests : BlockTests
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

        [TestCaseSource(nameof(SubAtomicByCases))]
        public void DefinedBlockPreferAtomicValueIsCorrect(LocatorType rootLocatorType, bool expectedPreferAtomic)
        {
            TestBlock block = GetBlockDefinedByLocatorType(rootLocatorType);

            (block.PreferAtomic).Should().Be(expectedPreferAtomic);
        }
    }
}
