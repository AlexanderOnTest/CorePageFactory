using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory
{
    public abstract class Page : PageController, IPageData
    {
        protected Page(IWebDriver driver) : base (driver) { }

        public abstract string GetExpectedPageTitle();

        public abstract string GetExpectedUri();
    }
}
