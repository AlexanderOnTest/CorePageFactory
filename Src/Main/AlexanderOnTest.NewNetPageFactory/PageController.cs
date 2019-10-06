using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory
{
    /// <summary>
    /// A PageController using Selenium WebDriver to control the Browser.
    /// </summary>
    public class PageController : IPageController
    {
        private readonly IWebDriver driver;
        
        /// <summary>
        /// Constructor for a general WebDriver PageController.
        /// </summary>
        /// <param name="driver">The IWebDriver instance</param>
        public PageController(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string GetActualPageTitle()
        {
            return driver.Title;
        }

        public string GetActualUri()
        {
            return driver.Url;
        }
    }
}