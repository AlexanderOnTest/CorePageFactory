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
        PartialLinkText
    }

    /// <summary>
    /// Extension methods for the LocatorType Enum to simplify 'By' conversion.
    /// </summary>
    public static class LocatorTypeExtension
    {
        private static readonly Dictionary<LocatorType, (bool isConvertible, Func<string, string> locatorConversionFunc)> LocatorConvertibilityDetails 
            = new Dictionary<LocatorType, (bool isConvertible, Func<string, string> locatorConversionFunc)>();

        private static readonly Dictionary<LocatorType, Func<string, string>> LocatorConversionFuncs
            = new Dictionary<LocatorType, Func<string, string>>();

        static LocatorTypeExtension()
        {
            LocatorConversionFuncs.Add(LocatorType.CssSelector, (sel => sel));
            LocatorConversionFuncs.Add(LocatorType.TagName, (sel => sel));
            LocatorConversionFuncs.Add(LocatorType.ClassName, (sel => $".{sel}"));
            LocatorConversionFuncs.Add(LocatorType.Name, (sel => $"*[name=\"{sel}\"]"));
            LocatorConversionFuncs.Add(LocatorType.Id, (sel => $"#{sel}"));
            LocatorConversionFuncs.Add(LocatorType.XPath, (null));
            LocatorConversionFuncs.Add(LocatorType.LinkText, (null));
            LocatorConversionFuncs.Add(LocatorType.PartialLinkText, (null));
        }

        /// <summary>
        /// Return the function to convert the locator for this LocatorType to a CssSelector; or null if not possible;
        /// </summary>
        /// <param name="locatorType"></param>
        /// <returns></returns>
        public static Func<string, string> ConvertToCssSelectorFunc(this LocatorType locatorType)
        {

            LocatorConversionFuncs.TryGetValue(locatorType, out Func<string, string> locatorConversionFunc);
            return locatorConversionFunc;
        }

        /// <summary>
        /// <para>Return true if a 'By' of this LocatorType can be combined into Atomic WebDriver calls.</para>
        /// <para>(In other words - the 'By' uses or can be converted to use a CssSelector)</para>
        /// </summary>
        /// <param name="locatorType"></param>
        /// <returns></returns>
        public static bool IsSubAtomic(this LocatorType locatorType)
        {
            return locatorType.ConvertToCssSelectorFunc() != null;
        }
    }
}
