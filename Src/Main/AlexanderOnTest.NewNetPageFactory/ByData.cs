using System;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory
{
    internal class ByData
    {
        public ByData(By by)
        {
            By = by;
            (LocatorType locatorType, string locatorValue) = by.GetLocatorDetail();
            LocatorType = locatorType;
            OriginalLocator = locatorValue;
            Func<string, string> converterFunc = LocatorType.ConvertToCssSelectorFunc();
            IsSubAtomic = converterFunc != null;
            CssLocator = IsSubAtomic ? converterFunc(OriginalLocator) : null;
        }

        internal By By { get; }

        internal LocatorType LocatorType { get; }

        internal string OriginalLocator { get; }

        internal bool IsSubAtomic { get; }

        internal string CssLocator { get; }
    }
}
