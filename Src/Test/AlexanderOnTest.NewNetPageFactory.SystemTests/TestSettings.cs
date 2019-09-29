using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public static class TestSettings
    {
        static TestSettings()
        {
            TestPagesDirectoryUri = new Uri(AppDomain.CurrentDomain.BaseDirectory);
            TestPageUri = new Uri(
                TestSettings.TestPagesDirectoryUri,
                $"TestPages{Path.AltDirectorySeparatorChar}TestPage.html");
            TestPageUriString = TestPageUri.ToString();
        }

        internal static readonly Uri TestPagesDirectoryUri;

        internal static readonly Uri TestPageUri;

        internal static readonly string TestPageUriString;
    }
}