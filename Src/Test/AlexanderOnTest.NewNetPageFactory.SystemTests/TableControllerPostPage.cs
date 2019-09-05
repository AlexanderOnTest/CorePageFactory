using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class TableControllerPostPage : Page
    {
        //private NonExistentBlock nonExistentBlock;

        public TableControllerPostPage(IWebDriver driver, NonExistentBlock nonExistentBlock) : base(driver)
        {
            this.NonExistentBlock = nonExistentBlock;
        }

        public NonExistentBlock NonExistentBlock { get; set; }

        public override string GetExpectedPageTitle()
        {
            return "";
        }

        public override Uri GetExpectedUriPath()
        {
            return null;
        }

        public void TimeoutFailingToFindNonExistentElement(bool useLongWait)
        {
            DateTime start = DateTime.Now;
            double delay;
            try
            {
                NonExistentBlock.WaitToGetRootElement(useLongWait);
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Time until failure : ${DateTime.Now.Subtract(start).TotalSeconds} seconds");
                throw ex;
            }
        }
    }
}
