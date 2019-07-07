using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AlexanderOnTest.NewNetPageFactory.Controllers;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks
{
    class DefinedBlock : DefinedBlockController
    {
        internal new bool preferAtomic;

        public DefinedBlock(string rootElementCssSelector, IWebDriver driver) : base(rootElementCssSelector, driver)
        {
            this.preferAtomic = base.preferAtomic;
        }

        public DefinedBlock(By rootElementBy, IWebDriver driver) : base(rootElementBy, driver)
        {
            this.preferAtomic = base.preferAtomic;
        }

        public IWebElement CssSelectorElement() => this.FindElement(By.CssSelector("css"));

        public IWebElement ClassNameElement() => this.FindElement(By.ClassName("class"));

        public IWebElement NameElement() => this.FindElement(By.Name("name"));

        public IWebElement TagNameElement() => this.FindElement(By.TagName("tagName"));

        public IWebElement IdElement() => this.FindElement(By.Id("id"));

        public IWebElement XPathElement() => this.FindElement(By.XPath("xpath"));

        public IWebElement LinkTextElement() => this.FindElement(By.LinkText("linkText"));

        public IWebElement PartialLinkTextElement() => this.FindElement(By.PartialLinkText("partialLinkText"));
    }
}
