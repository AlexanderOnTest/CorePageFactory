using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    public class TestPage : Page
    {
        public TestPage(IWebDriver driver, NonExistentBlock nonExistentBlock) : base(driver)
        {
            this.NonExistentBlock = nonExistentBlock;
        }

        public NonExistentBlock NonExistentBlock { get; set; }

        public override string GetExpectedPageTitle()
        {
            return "AlexanderOnTest - PageFactory Test Page";
        }

        public override string GetExpectedUri()
        {
            return "file:///C:/src/CorePageFactory/Src/Test/AlexanderOnTest.NewNetPageFactory.SystemTests/TestPages/TestPage.html";
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
    }
}
