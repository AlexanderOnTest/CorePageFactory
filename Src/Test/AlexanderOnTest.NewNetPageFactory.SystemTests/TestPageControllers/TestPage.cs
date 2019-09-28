using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    public class TestPage : Page
    {
        public TestPage(IWebDriver driver, 
            NonExistentBlock nonExistentBlock,
            TableBlock tableBlock) : base(driver)
        {
            this.NonExistentBlock = nonExistentBlock;
            this.TableBlock = tableBlock;
        }

        public NonExistentBlock NonExistentBlock { get; set; }

        public TableBlock TableBlock { get; set; }

        public override string GetExpectedPageTitle()
        {
            return "AlexanderOnTest - PageFactory Test Page";
        }

        public override string GetExpectedUri()
        {
            return TestSettings.TestPageAddress;
        }

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
