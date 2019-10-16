using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace AlexanderOnTest.NewNetPageFactory
{
    /// <summary>
    /// <para> Abstract class with methods to support PageBlocks whose Root IWebElement can be defined.</para>
    /// <para> Uses atomic WebDriver calls where possible </para>
    /// </summary>
    public abstract class BlockController : IBlockController
    {
        protected readonly bool PreferAtomic;
        private readonly IWebElement rootElement;
        private IWebDriver driver;
        
        private readonly TimeSpan shortWaitTimeSpan;
        private readonly TimeSpan longWaitTimeSpan;
        private readonly Lazy<IWait<IWebDriver>> shortWait;
        private readonly Lazy<IWait<IWebDriver>> longWait;
        private readonly string parentClassName;

        private IWait<IWebDriver> InitShortWait()
        {
            return new ImmutableWait(this.Driver, this.shortWaitTimeSpan, this.parentClassName);
        }

        private IWait<IWebDriver> InitLongWait()
        {
            return new ImmutableWait(this.Driver, this.longWaitTimeSpan, this.parentClassName);
        }

        private IWait<IWebDriver> ShortWait => 
            shortWait.Value;

        private IWait<IWebDriver> LongWait => 
            longWait.Value;

        /// <summary>
        /// <para> Create a BlockController with a previously found IWebElement as its root.</para>
        /// <para> Note: A DOM update can cause the rootElement to become stale.</para>
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="shortWaitTimeSpan"></param>
        /// <param name="longWaitTimeSpan"></param>
        /// <param name="parentClassName"></param>
        protected BlockController(
            IWebElement rootElement, 
            TimeSpan shortWaitTimeSpan = default, 
            TimeSpan longWaitTimeSpan = default, 
            [CallerFilePath] string parentClassName = null)
        {
            this.PreferAtomic = false;
            this.rootElement = rootElement;

            this.shortWaitTimeSpan = (shortWaitTimeSpan == default) ? TimeSpan.FromSeconds(5) : shortWaitTimeSpan;
            this.longWaitTimeSpan = (longWaitTimeSpan == default) ? TimeSpan.FromSeconds(30) : longWaitTimeSpan;
            
            this.parentClassName = parentClassName;

            this.shortWait = new Lazy<IWait<IWebDriver>>(InitShortWait);
            this.longWait = new Lazy<IWait<IWebDriver>>(InitLongWait);
        }

        /// <summary>
        /// Create a BlockController using a CssLocator string to define the IWebElement at its root.
        /// </summary>
        /// <param name="rootElementCssSelector"></param>
        /// <param name="driver"></param>
        /// <param name="shortWaitTimeSpan"></param>
        /// <param name="longWaitTimeSpan"></param>
        /// <param name="parentClassName"></param>
        protected BlockController(
            string rootElementCssSelector, 
            IWebDriver driver, 
            TimeSpan shortWaitTimeSpan = default, 
            TimeSpan longWaitTimeSpan = default,
            [CallerFilePath] string parentClassName = null)
        {
            this.driver = driver;
            this.PreferAtomic = true;
            this.RootElementCssSelector = rootElementCssSelector;

            this.shortWaitTimeSpan = (shortWaitTimeSpan == default) ? TimeSpan.FromSeconds(5) : shortWaitTimeSpan;
            this.longWaitTimeSpan = (longWaitTimeSpan == default) ? TimeSpan.FromSeconds(30) : longWaitTimeSpan;
            
            this.parentClassName = parentClassName;

            this.shortWait = new Lazy<IWait<IWebDriver>>(InitShortWait);
            this.longWait = new Lazy<IWait<IWebDriver>>(InitLongWait);
        }

        /// <summary>
        /// Create a BlockController using a By locator to define the IWebElement at its root.
        /// </summary>
        protected BlockController(
            By rootElementBy, 
            IWebDriver driver, 
            TimeSpan shortWaitTimeSpan = default, 
            TimeSpan longWaitTimeSpan = default,
            [CallerFilePath] string parentClassName = null)
        {
            this.driver = driver;
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

            this.shortWaitTimeSpan = (shortWaitTimeSpan == default) ? TimeSpan.FromSeconds(5) : shortWaitTimeSpan;
            this.longWaitTimeSpan = (longWaitTimeSpan == default) ? TimeSpan.FromSeconds(30) : longWaitTimeSpan;
            
            this.parentClassName = parentClassName;

            this.shortWait = new Lazy<IWait<IWebDriver>>(InitShortWait);
            this.longWait = new Lazy<IWait<IWebDriver>>(InitLongWait);
        }

        protected IWebDriver Driver
            => driver ?? (driver = ((IWrapsDriver) this.rootElement).WrappedDriver);

        protected string RootElementCssSelector { get; }
        
        protected By RootElementBy { get;  }

        public IWebElement GetRootElement()
        {
            return this.rootElement?? Driver.FindElement(RootElementBy?? By.CssSelector(RootElementCssSelector));
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

        public IWebElement WaitToGetRootElement(bool useLongWait = false)
        {
            var wait = useLongWait ? LongWait : ShortWait;
            return wait.Until(d => GetRootElement());
        }

        protected IWebElement FindElementWithWait(string relativeCssSelector, bool useLongWait = false)
        {
            var wait = useLongWait ? LongWait : ShortWait;
            return wait.Until(d => FindElement(relativeCssSelector));
        }

        protected IWebElement FindElementWithWait(By relativeBy, bool useLongWait = false)
        {
            var wait = useLongWait ? LongWait : ShortWait;
            return wait.Until(d => FindElement(relativeBy));
        }

        protected ReadOnlyCollection<IWebElement> FindElementsWithWaitForMinimumElements(
            string relativeCssSelector,
            int minimumElements = 1, 
            bool useLongWait = false)
        {
            var wait = useLongWait ? LongWait : ShortWait;

            Func<ReadOnlyCollection<IWebElement>> findFunction = () => FindElements(relativeCssSelector);

            Func<ReadOnlyCollection<IWebElement>, bool> conditionFunction =
                (returnedElements) => returnedElements.Count < minimumElements;

            string failMessage = $"Less than {minimumElements.ToString()} (By.CssSelector: {relativeCssSelector})";
            
            return FindElementsWithWait(
                wait, 
                findFunction, 
                conditionFunction, 
                failMessage);
        }

        protected ReadOnlyCollection<IWebElement> FindElementsWithWaitForMinimumElements(
            By relativeBy,
            int minimumElements = 1, 
            bool useLongWait = false)
        {
            IWait<IWebDriver> wait = useLongWait ? LongWait : ShortWait;

            Func<ReadOnlyCollection<IWebElement>> findFunction = () => FindElements(relativeBy);

            Func<ReadOnlyCollection<IWebElement>, bool> conditionFunction =
                (returnedElements) => returnedElements.Count < minimumElements;

            string failMessage = $"Less than {minimumElements.ToString()} ({relativeBy})";

            return FindElementsWithWait(
                wait, 
                findFunction, 
                conditionFunction, 
                failMessage);
        }
        
        protected ReadOnlyCollection<IWebElement> FindElementsWithWaitForMaximumElements(
            string relativeCssSelector,
            int maximumElements = 1, 
            bool useLongWait = false)
        {
            IWait<IWebDriver> wait = useLongWait ? LongWait : ShortWait;

            Func<ReadOnlyCollection<IWebElement>> findFunction = () => FindElements(relativeCssSelector);

            Func<ReadOnlyCollection<IWebElement>, bool> conditionFunction =
                (returnedElements) => returnedElements.Count > maximumElements;

            string failMessage = $"More than {maximumElements.ToString()} (By.CssSelector: {relativeCssSelector})";

            return FindElementsWithWait(
                wait, 
                findFunction, 
                conditionFunction, 
                failMessage);
        }

        protected ReadOnlyCollection<IWebElement> FindElementsWithWaitForMaximumElements(
            By relativeBy,
            int maximumElements = 1, 
            bool useLongWait = false)
        {
            IWait<IWebDriver> wait = useLongWait ? LongWait : ShortWait;

            Func<ReadOnlyCollection<IWebElement>> findFunction = () => FindElements(relativeBy);

            Func<ReadOnlyCollection<IWebElement>, bool> conditionFunction =
                (returnedElements) => returnedElements.Count > maximumElements;

            string failMessage = $"More than {maximumElements.ToString()} ({relativeBy})";

            return FindElementsWithWait(
                wait, 
                findFunction, 
                conditionFunction, 
                failMessage);
        }

        /// <summary>
        /// General Purpose Wait for FindElements to meet a given condition.
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="findFunction"></param>
        /// <param name="conditionFunction"></param>
        /// <param name="conditionDescription"></param>
        /// <returns></returns>
        /// <exception cref="NoSuchElementException"></exception>
        /// <exception cref="WebDriverTimeoutException"></exception>
        protected ReadOnlyCollection<IWebElement> FindElementsWithWait(
            IWait<IWebDriver> wait, 
            Func<ReadOnlyCollection<IWebElement>> findFunction,
            Func<ReadOnlyCollection<IWebElement>, bool> conditionFunction,
            string conditionDescription)
        {
            try
            {
                return wait.Until(d => {
                    ReadOnlyCollection<IWebElement> returnedElements = findFunction.Invoke();
                    
                    if (conditionFunction(returnedElements))
                    {
                        throw new NoSuchElementException();
                    }

                    return returnedElements;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException(
                    $"{ex.Message}: {conditionDescription} were found in the searched context.", ex);
            }
        }
    }
}
