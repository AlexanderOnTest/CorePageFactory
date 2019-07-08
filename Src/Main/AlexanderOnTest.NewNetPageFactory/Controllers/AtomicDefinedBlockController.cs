using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    public abstract class AtomicDefinedBlockController
    {
        protected readonly bool preferAtomic;

        protected AtomicDefinedBlockController(string rootElementCssSelector, IWebDriver driver)
        {
            Driver = driver;
            this.preferAtomic = true;
            this.RootElementCssSelector = rootElementCssSelector;
        }

        protected AtomicDefinedBlockController(By rootElementBy, IWebDriver driver)
        {
            Driver = driver;
            (LocatorType locatorType, var locatorValue) = rootElementBy.GetLocatorDetail();
            Func<string, string> conversionFunc = locatorType.ConvertToCssSelectorFunc();
            if (conversionFunc != null)
            {
                this.preferAtomic = true;
                this.RootElementCssSelector = conversionFunc(locatorValue);
                this.RootElementBy = By.CssSelector(RootElementCssSelector);
            }
            else
            {
                this.preferAtomic = false;
                this.RootElementBy = rootElementBy;
            }
        }

        protected IWebDriver Driver { get; } 

        protected string RootElementCssSelector { get; }
        
        protected By RootElementBy { get;  }

        public async Task<IWebElement> GetRootElement()
        {
            return Driver.FindElement(RootElementBy);
        }

        protected IWebElement FindElement(string relativeCssSelector)
        {
            return (preferAtomic)
                ? Driver.FindElement(By.CssSelector($"{RootElementCssSelector} {relativeCssSelector}")) 
                : Driver.FindElement(RootElementBy).FindElement(By.CssSelector(relativeCssSelector));
        }

        protected IWebElement FindElement(By relativeBy)
        {
            var relativeByData = new ByData(relativeBy);
            return (preferAtomic && relativeByData.IsSubAtomic)
                ? Driver.FindElement(By.CssSelector($"{RootElementCssSelector} {relativeByData.CssLocator}"))
                : Driver.FindElement(RootElementBy).FindElement(relativeBy);
        }
    }
}
