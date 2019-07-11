using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    public abstract class AtomicDefinedBlockController
    {
        protected readonly bool PreferAtomic;

        protected AtomicDefinedBlockController(string rootElementCssSelector, IWebDriver driver)
        {
            Driver = driver;
            this.PreferAtomic = true;
            this.RootElementCssSelector = rootElementCssSelector;
        }

        protected AtomicDefinedBlockController(By rootElementBy, IWebDriver driver)
        {
            Driver = driver;
            (LocatorType locatorType, var locatorValue) = rootElementBy.GetLocatorDetail();
            Func<string, string> conversionFunc = locatorType.ConvertToCssSelectorFunc();
            if (conversionFunc != null)
            {
                this.PreferAtomic = true;
                this.RootElementCssSelector = conversionFunc(locatorValue);
                this.RootElementBy = By.CssSelector(RootElementCssSelector);
            }
            else
            {
                this.PreferAtomic = false;
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

        protected IWebElement FindElement(string relativeCssSelector)
        {
            return (PreferAtomic)
                ? Driver.FindElement(By.CssSelector($"{RootElementCssSelector} {relativeCssSelector}")) 
                : this.GetRootElement().FindElement(By.CssSelector(relativeCssSelector));
        }

        protected IWebElement FindElement(By relativeBy)
        {
            var relativeByData = new ByData(relativeBy);
            return (PreferAtomic && relativeByData.IsSubAtomic)
                ? Driver.FindElement(By.CssSelector($"{RootElementCssSelector} {relativeByData.CssLocator}"))
                : this.GetRootElement().FindElement(relativeBy);
        }

        protected ReadOnlyCollection<IWebElement> FindElements(string relativeCssSelector)
        {
            return (PreferAtomic)
                ? Driver.FindElements(By.CssSelector($"{RootElementCssSelector} {relativeCssSelector}"))
                : this.GetRootElement().FindElements(By.CssSelector(relativeCssSelector));
        }

        protected ReadOnlyCollection<IWebElement> FindElements(By relativeBy)
        {
            var relativeByData = new ByData(relativeBy);
            return (PreferAtomic && relativeByData.IsSubAtomic)
                ? Driver.FindElements(By.CssSelector($"{RootElementCssSelector} {relativeByData.CssLocator}"))
                : this.GetRootElement().FindElements(relativeBy);
        }
    }
}
