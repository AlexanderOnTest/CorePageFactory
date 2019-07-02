using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

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

            this.UseBy = true;
            this.RootElementBy = rootElementBy;
        }

        protected IWebDriver Driver { get; } 

        protected string RootElementCssSelector { get; }
        
        protected By RootElementBy { get;  }



        public IWebElement GetRootElement()
        {
            if (this.UseBy)
            {
                return Driver.FindElement(RootElementBy);
            }
            else
            {
                return Driver.FindElement(By.CssSelector(RootElementCssSelector));
            }

        }
    }
}
