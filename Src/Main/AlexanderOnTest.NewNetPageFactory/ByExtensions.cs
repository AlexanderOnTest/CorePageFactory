using System;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory
{
    internal static class ByExtensions
    {
        internal static (LocatorType locatorType, string locatorValue) GetLocatorDetail(this By by)
        {
            (string locatorType, string locatorValue) byDetail;

            string toString = by.ToString();
            string cleanToString = toString.Replace("[Contains]", string.Empty);
            var tokens = cleanToString.Split(':');
            if (tokens.Length > 1)
            {
                byDetail = (tokens[0].Substring(3), tokens[1].TrimStart());
            }
            else
            {
                throw new ArgumentOutOfRangeException($"By format ({by.ToString()}) is not recognized.");
            }

            _ = Enum.TryParse(byDetail.locatorType, out LocatorType locatorType);

            return (locatorType, byDetail.locatorValue);
        }

        internal static string GetEquivalentCssLocator((LocatorType locatorType, string locatorValue) byDetails)
        {
            (LocatorType locatorType, var locatorValue) = byDetails;
            var conversionFunc = locatorType.ConvertToCssSelectorFunc();
            if (conversionFunc == null)
            {
                throw new ArgumentException($"'By's of type {locatorType.ToString()} cannot be converted to use a CssSelector.");
            }
            else
            {
                return conversionFunc(locatorValue);
            }
        }
    }
}
