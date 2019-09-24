using System;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers
{
    public class NonExistentBlock : Block
    {
        private const string rootElementCssSelector = ".ThisClassDoesNotExist";

        public NonExistentBlock(IWebDriver driver, TimeSpan shortWaitTimeSpan = default, TimeSpan longWaitTimeSpan = default) : 
            base(rootElementCssSelector, driver, shortWaitTimeSpan, longWaitTimeSpan)
        {
        }
    }
}
