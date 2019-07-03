using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.Utilities
{
    public static class ByExtensions
    {
        private static readonly IDictionary<string, (string, string)> PrefixSuffixLookup;
        private static readonly IDictionary<string, Func<string, string>> SelectorConverterLookup;

        static ByExtensions()
        {
            PrefixSuffixLookup = new Dictionary<string, (string, string)>()
            {
                {"CssSelector", (string.Empty, string.Empty)},
                {"Id", ("#", string.Empty)},
                {"ClassName", (".", string.Empty) },
                {"TagName", (string.Empty, string.Empty) },
                {"Name", ("*[name=\"", "\"]") }
            };


            SelectorConverterLookup = new Dictionary<string, Func<string, string>>()
            {
                {"CssSelector", sel => sel},
                {"Id", sel => $"#{sel}"},
                {"ClassName", sel => $".{sel}" },
                {"TagName", sel => sel },
                {"Name", sel => $"*[name=\"{sel}\"]" }
            };
        }

        public static (string locatorType, string locatorValue, bool isAtomic) GetLocatorDetails(this By by)
        {
            (string locatorType, string locatorValue, bool isAtomic) byDetails;

            string toString = by.ToString();
            string cleanToString = toString.Replace("[Contains]", string.Empty);
            var tokens = cleanToString.Split(':');
            if (tokens.Length > 1)
            {
                byDetails = (tokens[0].Substring(3), tokens[1].TrimStart(), false);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"By format ({by.ToString()}) is not recognized.");
            }

            byDetails.isAtomic = (byDetails.locatorType.Equals("Id") |
                                  byDetails.locatorType.Equals("CssSelector") |
                                  byDetails.locatorType.Contains("Name"));

            return byDetails;
        }

        public static string GetAtomicCssLocator((string locatorType, string locatorValue, bool isAtomic) byDetails)
        {
            if (!byDetails.isAtomic)
            {
                throw new ArgumentException($"'By's of type {byDetails.locatorType} cannot be converted to use a CssSelector.");
            }
            else
            {
                SelectorConverterLookup.TryGetValue(byDetails.locatorType, out Func<string, string> conversionFunction);
                return conversionFunction(byDetails.locatorValue);
            }
        }
    }
}
