using System;
using System.Collections.Generic;

namespace AlexanderOnTest.NewNetPageFactory
{
    public enum LocatorType
    {
        CssSelector,
        TagName,
        ClassName,
        Name,
        Id,
        XPath,
        LinkText,
        PartialLinkText,
        String
    }
    
    internal static class LocatorTypeExtension
    {
        private static readonly Dictionary<LocatorType, Func<string, string>> LocatorConversionFunctions
            = new Dictionary<LocatorType, Func<string, string>>();

        static LocatorTypeExtension()
        {
            LocatorConversionFunctions.Add(LocatorType.CssSelector, (sel => sel));
            LocatorConversionFunctions.Add(LocatorType.TagName, (sel => sel));
            LocatorConversionFunctions.Add(LocatorType.ClassName, (sel => $".{sel}"));
            LocatorConversionFunctions.Add(LocatorType.Name, (sel => $"*[name=\"{sel}\"]"));
            LocatorConversionFunctions.Add(LocatorType.Id, (sel => $"#{sel}"));
            LocatorConversionFunctions.Add(LocatorType.XPath, null);
            LocatorConversionFunctions.Add(LocatorType.LinkText, null);
            LocatorConversionFunctions.Add(LocatorType.PartialLinkText, null);
            LocatorConversionFunctions.Add(LocatorType.String, (sel => sel));
        }

        internal static Func<string, string> ConvertToCssSelectorFunc(this LocatorType locatorType)
        {

            LocatorConversionFunctions.TryGetValue(locatorType, out Func<string, string> locatorConversionFunc);
            return locatorConversionFunc;
        }

        internal static bool IsSubAtomic(this LocatorType locatorType)
        {
            return locatorType.ConvertToCssSelectorFunc() != null;
        }
    }
}
