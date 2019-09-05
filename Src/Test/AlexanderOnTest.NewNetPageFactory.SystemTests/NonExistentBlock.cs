using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
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
