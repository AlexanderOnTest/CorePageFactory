using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    public class TableBlock : Block
    {
        private const string CustomerTableRootElementCssSelector = "#customers";

        public TableBlock(IWebDriver driver, TimeSpan shortWaitTimeSpan = default, TimeSpan longWaitTimeSpan = default) : base(CustomerTableRootElementCssSelector, driver, shortWaitTimeSpan, longWaitTimeSpan)
        {
        }

        public TableBlock(By rootElementBy, IWebDriver driver) : base(rootElementBy, driver)
        {
        }

        public IReadOnlyCollection<IWebElement> WaitForMinimumRowsToLoadAndReturn(int minimumNumberOfRowsRequired, bool useLongWait = false)
        {
            return FindElementsWithWaitForMinimumElements(By.TagName("tr"), minimumNumberOfRowsRequired, useLongWait);
        }

        public IReadOnlyCollection<IWebElement> WaitForMaximumRowsToLoadAndReturn(int maximumNumberOfRowsRequired, bool useLongWait = false)
        {
            return FindElementsWithWaitForMaximumElements(By.TagName("tr"), maximumNumberOfRowsRequired, useLongWait);
        }
    }
}
