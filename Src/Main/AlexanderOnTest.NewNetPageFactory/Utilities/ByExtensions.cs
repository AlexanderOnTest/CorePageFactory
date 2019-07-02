using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.Utilities
{
    public static class ByExtensions
    {
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
    }
}
