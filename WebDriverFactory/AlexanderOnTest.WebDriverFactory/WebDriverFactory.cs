using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.WebDriverFactory
{
    public static class WebDriverFactory
    {
        private static string DriverPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static IWebDriver GetLocalWindowsWebDriver(Browser browser)
        {
            switch (browser)
            {
                case Browser.Firefox:
                    return new FirefoxDriver(DriverPath);

                case Browser.Chrome:
                    return new ChromeDriver(DriverPath);

                case Browser.InternetExplorer:
                    if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
                    {
                        throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
                    }
                    return new InternetExplorerDriver(DriverPath);

                case Browser.Edge:
                    if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
                    {
                        throw new PlatformNotSupportedException("Microsoft Edge is only available on Microsoft Windows.");
                    }
                    return new EdgeDriver(DriverPath);

                case Browser.Safari:
                    if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.Mac))
                    {
                        throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
                    }
                    // I suspect that the SafariDriver is alredy on the path as it is Within Safari.
                    // I currently have no means to test.
                    return new SafariDriver();

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static IWebDriver GetHdLocalWindowsWebDriver(Browser browser)
        {
            return GetLocalWindowsWebDriver(browser, new Size(1366, 768));
        }

        public static IWebDriver GetFhdLocalWindowsWebDriver(Browser browser)
        {
            return GetLocalWindowsWebDriver(browser, new Size(1920, 1080));
        }
        
        public static IWebDriver GetFullScreenLocalWindowsWebDriver(Browser browser)
        {
            IWebDriver newWebDriver = GetLocalWindowsWebDriver(browser);
            newWebDriver.Manage().Window.FullScreen(); 
            return newWebDriver;
        }

        public static IWebDriver GetLocalWindowsWebDriver(Browser browser, Size windowSize)
        {
            IWebDriver newWebDriver = GetLocalWindowsWebDriver(browser);
            newWebDriver.Manage().Window.Position = Point.Empty;
            newWebDriver.Manage().Window.Size = windowSize;
            return newWebDriver;
        }
    }
}
