using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AlexanderOnTest.NewNetPageFactory
{
    public abstract class Page
    {
        private readonly IWebDriver driver;

        protected Page(IWebDriver driver)
        {
            this.driver = driver;
        }

        public abstract string GetExpectedPageTitle();

        public abstract Uri GetExpectedUriPath();

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
