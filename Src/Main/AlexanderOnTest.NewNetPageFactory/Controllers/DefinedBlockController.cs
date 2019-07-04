using OpenQA.Selenium;
using System;
using AlexanderOnTest.NewNetPageFactory.Utilities;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    public abstract class DefinedBlockController
    {
        private readonly bool UseBy;

        protected DefinedBlockController(string rootElementCssSelector)
        {
            this.UseBy = false;
            this.RootElementCssSelector = rootElementCssSelector;
        }

        protected DefinedBlockController(By rootElementBy)
        {
            (LocatorType locatorType, var locatorValue) = rootElementBy.GetLocatorDetail();
            Func<string, string> conversionFunc = locatorType.ConvertToCssSelectorFunc();
            if (conversionFunc != null)
            {
                this.UseBy = false;
                this.RootElementCssSelector = conversionFunc(locatorValue);
                this.RootElementBy = By.CssSelector(RootElementCssSelector);
            }
            else
            {
                this.UseBy = true;
                this.RootElementBy = rootElementBy;
            }
        }

        protected IWebDriver Driver { get; } 

        protected string RootElementCssSelector { get; }
        
        protected By RootElementBy { get;  }

        public IWebElement GetRootElement()
        {
            return Driver.FindElement(RootElementBy);
        }
    }
}
