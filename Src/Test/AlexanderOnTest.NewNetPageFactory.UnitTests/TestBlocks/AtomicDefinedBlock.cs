using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AlexanderOnTest.NewNetPageFactory.Controllers;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests.TestBlocks
{
    class AtomicDefinedBlock : AtomicDefinedBlockController
    {
        internal new bool PreferAtomic;

        public AtomicDefinedBlock(string rootElementCssSelector, IWebDriver driver) : base(rootElementCssSelector, driver)
        {
            this.PreferAtomic = base.PreferAtomic;
        }

        public AtomicDefinedBlock(By rootElementBy, IWebDriver driver) : base(rootElementBy, driver)
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
    }
}
