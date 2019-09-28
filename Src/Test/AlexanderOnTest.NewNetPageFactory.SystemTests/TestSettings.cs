using System;
using System.IO;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public static class TestSettings
    {
        static TestSettings()
        {
            TestPagesDirectoryUri = new Uri($"file:///{AppDomain.CurrentDomain.BaseDirectory}");
            TestPageAddress = new Uri(
                TestSettings.TestPagesDirectoryUri, 
                $"TestPages{Path.DirectorySeparatorChar}TestPage.html").ToString();
        }

        internal static readonly Uri TestPagesDirectoryUri;

        internal static readonly string TestPageAddress;
    }
}