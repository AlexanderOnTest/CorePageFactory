using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    /// <summary>
    /// Controller for the entire TestPage.html.
    /// </summary>
    public class TestPage : Page
    {
        /// <summary>
        /// Construct a TestPage controller for TestPage.html
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="nonExistentBlock"></param>
        /// <param name="tableBlock"></param>
        public TestPage(IWebDriver driver, 
            NonExistentBlock nonExistentBlock,
            TableBlock tableBlock) : base(driver)
        {
            this.NonExistentBlock = nonExistentBlock;
            this.TableBlock = tableBlock;
        }

        /// <summary>
        /// Return the controller for a block that does not exist in the DOM.
        /// </summary>
        public NonExistentBlock NonExistentBlock { get; set; }

        /// <summary>
        /// Return the controller for a block controller for the table block.
        /// </summary>
        public TableBlock TableBlock { get; set; }

        public override string GetExpectedPageTitle()
        {
            return "AlexanderOnTest - PageFactory Test Page";
        }

        public override string GetExpectedUri()
        {
            return TestSettings.TestPageUriString;
        }

        /// <summary>
        /// Test timeout duration by waiting for the non existent block to load.
        /// </summary>
        /// <param name="useLongWait"></param>
        /// <exception cref="Exception"></exception>
        public void TimeoutFailingToFindNonExistentElement(bool useLongWait)
        {
            DateTime start = DateTime.Now;
            try
            {
                NonExistentBlock.WaitToGetRootElement(useLongWait);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Wait timed out after {DateTime.Now.Subtract(start).TotalSeconds} seconds");
                throw ex;
            }
        }

        /// <summary>
        /// Test minimum elements wait timeout duration by waiting for more rows to load than are in the DOM.
        /// </summary>
        /// <param name="useLongWait"></param>
        /// <exception cref="Exception"></exception>
        public void TimeoutFailingToWaitForMinRowsToLoad(bool useLongWait)
        {
            DateTime start = DateTime.Now;
            try
            {
                TableBlock.WaitForMinimumRowsToLoadAndReturn(15, useLongWait);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Wait timed out after {DateTime.Now.Subtract(start).TotalSeconds} seconds");
                throw ex;
            }
        }

        /// <summary>
        /// Test maximum elements wait timeout duration by waiting for more rows to load than are in the DOM.
        /// </summary>
        /// <param name="useLongWait"></param>
        /// <exception cref="Exception"></exception>
        public void TimeoutFailingToWaitForMaxRowsToLoad(bool useLongWait)
        {
            DateTime start = DateTime.Now;
            try
            {
                TableBlock.WaitForMaximumRowsToLoadAndReturn(2, useLongWait);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Wait timed out after {DateTime.Now.Subtract(start).TotalSeconds} seconds");
                throw ex;
            }
        }
    }
}
