using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    /// <summary>
    /// <para> Abstract class with methods to support PageBlocks whose Root IWebElement can be defined.</para>
    /// <para> Uses atomic WebDriver calls where possible </para>
    /// </summary>
    public abstract class AbstractPageBlock : IBlockController
    {
        protected readonly bool PreferAtomic;
        private readonly IWebElement rootElement;

        /// <summary>
        /// <para> Create a BlockController with a previously found IWebElement as its root.</para>
        /// <para> Note: A DOM update can cause the rootElement to become stale.</para>
        /// </summary>
        /// <param name="rootElement"></param>
        protected AbstractPageBlock(IWebElement rootElement)
        {
            Driver = null;
            this.PreferAtomic = false;
            this.rootElement = rootElement;
        }

        /// <summary>
        /// Create a BlockController using a CssLocator string to define the IWebElement at its root.
        /// </summary>
        /// <param name="rootElementCssSelector"></param>
        /// <param name="driver"></param>
        protected AbstractPageBlock(string rootElementCssSelector, IWebDriver driver)
        {
            Driver = driver;
            this.PreferAtomic = true;
            this.RootElementCssSelector = rootElementCssSelector;
        }


        /// <summary>
        /// Create a BlockController using a By locator to define the IWebElement at its root.
        /// </summary>
        protected AbstractPageBlock(By rootElementBy, IWebDriver driver)
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
            return this.rootElement?? Driver.FindElement(RootElementBy);
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
