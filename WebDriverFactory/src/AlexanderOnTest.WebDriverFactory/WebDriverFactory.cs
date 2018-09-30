using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.WebDriverFactory
{
    public static class WebDriverFactory
    {        private static string DriverPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(new FirefoxOptions(), WindowSize.Hd, headless);

                case Browser.Chrome:
                    return GetLocalWebDriver(new ChromeOptions(), WindowSize.Hd, headless);

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(new InternetExplorerOptions());

                case Browser.Edge:
                    return GetLocalWebDriver(new EdgeOptions());

                case Browser.Safari:
                    return GetLocalWebDriver(new SafariOptions());

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }


        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd,
            bool headless = false)
        {
            if (headless)
            {
                options.AddArgument("--headless");
            }

            IWebDriver driver = new ChromeDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd,
            bool headless = false)
        {
            if (headless)
            {
                options.AddArgument("--headless");
            }

            IWebDriver driver = new FirefoxDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
            {
                throw new PlatformNotSupportedException("Microsoft Edge is only available on Microsoft Windows.");
            }

            IWebDriver driver = new EdgeDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
            {
                throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
            }

            IWebDriver driver = new InternetExplorerDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.Mac))
            {
                throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
            }
            
            // I suspect that the SafariDriver is already on the path as it is within the Safari executable.
            // I currently have no means to test this
            IWebDriver driver = new SafariDriver(options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="gridUrl"></param>
        /// <param name="options"></param>
        /// <param name="platformType"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetRemoteWebDriver(Uri gridUrl, DriverOptions options,
            PlatformType platformType = PlatformType.Any, WindowSize windowSize = WindowSize.Hd)
        {
            IWebDriver driver = new RemoteWebDriver(gridUrl, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="gridUrl"></param>
        /// <param name="browser"></param>
        /// <returns></returns>
        public static IWebDriver GetRemoteWebDriver(Uri gridUrl, Browser browser, PlatformType platformType = PlatformType.Any)
        {
            //todo configure options with platform type
            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(gridUrl, new FirefoxOptions());

                case Browser.Chrome:
                    return GetRemoteWebDriver(gridUrl, new ChromeOptions());

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(gridUrl, new InternetExplorerOptions());

                case Browser.Edge:
                    return GetRemoteWebDriver(gridUrl, new EdgeOptions());

                case Browser.Safari:
                    return GetRemoteWebDriver(gridUrl, new SafariOptions());

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Convenience method for setting the Window Size of a WebDriver to common values. (768P, 1080P and fullscreen)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            switch (windowSize)
            {
                case WindowSize.Maximise:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Fhd:
                    driver.Manage().Window.Position = Point.Empty;
                    driver.Manage().Window.Size = new Size(1920, 1080);
                    return driver;

                default:
                    driver.Manage().Window.Position = Point.Empty;
                    driver.Manage().Window.Size = new Size(1366, 768);
                    return driver;
            }
        }
    }
}
