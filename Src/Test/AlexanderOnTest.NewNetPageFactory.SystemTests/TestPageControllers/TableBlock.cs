using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    /// <summary>
    /// BlockController implementation for the TableBlock of TestPage.html for testing BlockController methods
    /// </summary>
    public class TableBlock : BlockController
    {
        internal const string CustomerTableRootElementCssSelector = "#customers";

        /// <summary>
        /// Default TableBlock constructor calling the BlockController(string rootCssSelector...) constructor
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="shortWaitTimeSpan"></param>
        /// <param name="longWaitTimeSpan"></param>
        public TableBlock(IWebDriver driver, TimeSpan shortWaitTimeSpan = default, TimeSpan longWaitTimeSpan = default) : base(CustomerTableRootElementCssSelector, driver, shortWaitTimeSpan, longWaitTimeSpan)
        {
        }

        /// <summary>
        /// TableBlock constructor calling the BlockController(By.....) constructor
        /// </summary>
        /// <param name="rootElementBy"></param>
        /// <param name="driver"></param>
        public TableBlock(By rootElementBy, IWebDriver driver) : base(rootElementBy, driver)
        {
        }
        
        /// <summary>
        /// TableBlock constructor calling the BlockController(IWebDriver.....) constructor
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="driver"></param>
        public TableBlock(IWebElement rootElement, IWebDriver driver) : base(rootElement)
        {
        }

        /// <summary>
        /// Wait for a minimum number rows to be displayed and return them.
        /// </summary>
        /// <param name="minimumNumberOfRowsRequired"></param>
        /// <param name="useLongWait"></param>
        /// <returns></returns>
        public IReadOnlyCollection<IWebElement> WaitForMinimumRowsToLoadAndReturn(int minimumNumberOfRowsRequired, bool useLongWait = false)
        {
            return FindElementsWithWaitForMinimumElements(By.TagName("tr"), minimumNumberOfRowsRequired, useLongWait);
        }

        /// <summary>
        /// Wait for a maximum number rows to be displayed and return them.
        /// </summary>
        /// <param name="maximumNumberOfRowsRequired"></param>
        /// <param name="useLongWait"></param>
        /// <returns></returns>
        public IReadOnlyCollection<IWebElement> WaitForMaximumRowsToLoadAndReturn(int maximumNumberOfRowsRequired, bool useLongWait = false)
        {
            return FindElementsWithWaitForMaximumElements(By.TagName("tr"), maximumNumberOfRowsRequired, useLongWait);
        }
    }
}
