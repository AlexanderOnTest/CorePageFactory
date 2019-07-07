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

    /// <summary>
    /// Extension methods for the LocatorType Enum to simplify 'By' conversion.
    /// </summary>
    public static class LocatorTypeExtension
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

        /// <summary>
        /// Return the function to convert the locator for this LocatorType to a CssSelector; or null if not possible;
        /// </summary>
        /// <param name="locatorType"></param>
        /// <returns></returns>
        public static Func<string, string> ConvertToCssSelectorFunc(this LocatorType locatorType)
        {

            LocatorConversionFunctions.TryGetValue(locatorType, out Func<string, string> locatorConversionFunc);
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
