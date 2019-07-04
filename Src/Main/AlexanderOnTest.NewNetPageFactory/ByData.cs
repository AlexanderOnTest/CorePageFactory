using System;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory
{
    public class ByData
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

        public By By { get; }

        public LocatorType LocatorType { get; }

        public string OriginalLocator { get; }

        public bool IsSubAtomic { get; }

        public string CssLocator { get; }
    }
}
