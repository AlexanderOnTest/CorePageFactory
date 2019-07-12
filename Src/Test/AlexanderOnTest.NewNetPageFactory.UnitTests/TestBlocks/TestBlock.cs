using System.Collections.ObjectModel;
using AlexanderOnTest.NewNetPageFactory.Controllers;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks
{
    public class TestBlock : AbstractPageBlock
    {
        internal new bool PreferAtomic;

        public TestBlock(IWebElement rootElement) : base(rootElement) { }

        public TestBlock(string rootElementCssSelector, IWebDriver driver) : base(rootElementCssSelector, driver)
        {
            this.PreferAtomic = base.PreferAtomic;
        }

        public TestBlock(By rootElementBy, IWebDriver driver) : base(rootElementBy, driver)
        {
            this.PreferAtomic = base.PreferAtomic;
        }

        public IWebElement StringElement() => this.FindElement("css");

        public IWebElement CssSelectorElement() => this.FindElement(By.CssSelector("css"));

        public IWebElement ClassNameElement() => this.FindElement(By.ClassName("class"));

        public IWebElement NameElement() => this.FindElement(By.Name("name"));

        public IWebElement TagNameElement() => this.FindElement(By.TagName("tagName"));

        public IWebElement IdElement() => this.FindElement(By.Id("id"));

        public IWebElement XPathElement() => this.FindElement(By.XPath("xpath"));

        public IWebElement LinkTextElement() => this.FindElement(By.LinkText("linkText"));

        public IWebElement PartialLinkTextElement() => this.FindElement(By.PartialLinkText("partialLinkText"));

        public ReadOnlyCollection<IWebElement> StringElements() => this.FindElements("css");

        public ReadOnlyCollection<IWebElement> CssSelectorElements() => this.FindElements(By.CssSelector("css"));

        public ReadOnlyCollection<IWebElement> ClassNameElements() => this.FindElements(By.ClassName("class"));

        public ReadOnlyCollection<IWebElement> NameElements() => this.FindElements(By.Name("name"));

        public ReadOnlyCollection<IWebElement> TagNameElements() => this.FindElements(By.TagName("tagName"));

        public ReadOnlyCollection<IWebElement> IdElements() => this.FindElements(By.Id("id"));

        public ReadOnlyCollection<IWebElement> XPathElements() => this.FindElements(By.XPath("xpath"));

        public ReadOnlyCollection<IWebElement> LinkTextElements() => this.FindElements(By.LinkText("linkText"));

        public ReadOnlyCollection<IWebElement> PartialLinkTextElements() => this.FindElements(By.PartialLinkText("partialLinkText"));
    }
}
