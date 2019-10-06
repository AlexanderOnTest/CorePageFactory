using System;
using System.IO;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public static class TestSettings
    {
        static TestSettings()
        {
            TestPagesDirectoryUri = new Uri(
                $"{new Uri(AppDomain.CurrentDomain.BaseDirectory)}TestPages{Path.AltDirectorySeparatorChar}");
            
            Uri TestPageUri = new Uri(TestPagesDirectoryUri, $"TestPage.html");
            TestPageUriString = TestPageUri.ToString();
        }

        // A Uri for the TestPages Directory for locating test pages 
        private static readonly Uri TestPagesDirectoryUri;

        /// <summary>
        /// Return the String value of the TestPage Uri for use with WebDriver methods.
        /// </summary>
        internal static readonly string TestPageUriString;
    }
}