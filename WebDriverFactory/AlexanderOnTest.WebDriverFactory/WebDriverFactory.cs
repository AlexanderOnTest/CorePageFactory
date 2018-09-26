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

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, bool headless = false)
        {
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(browser, new FirefoxOptions(), WindowSize.hd, headless);

                case Browser.Chrome:
                    return GetLocalWebDriver(browser, new ChromeOptions(), WindowSize.hd, headless);

                case Browser.InternetExplorer:
                    if (headless)
                    {
                        throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
                    }
                    return GetLocalWebDriver(browser, new InternetExplorerOptions());

                case Browser.Edge:
                    if (headless)
                    {
                        throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
                    }
                    return GetLocalWebDriver(browser, new EdgeOptions());

                case Browser.Safari:
                    if (headless)
                    {
                        throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
                    }
                    return GetLocalWebDriver(browser, new SafariOptions());

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, ChromeOptions options, WindowSize windowSize = WindowSize.hd, bool headless = false)
        {
            if (browser != Browser.Chrome)
            {
                throw new ArgumentException("Options Mismatch: ChromeDriver can only launch with ChromeOptions");
            }
            IWebDriver driver = new ChromeDriver(DriverPath, headless ? options : AddHeadlessOption(options));
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, FirefoxOptions options, WindowSize windowSize = WindowSize.hd, bool headless = false)
        {
            if (browser != Browser.Firefox)
            {
                throw new ArgumentException("Options Mismatch: FirefoxDriver can only launch with FirefoxOptions");
            }

            IWebDriver driver = new FirefoxDriver(DriverPath, headless ? options : AddHeadlessOption(options));
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, EdgeOptions options, WindowSize windowSize = WindowSize.hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
            {
                throw new PlatformNotSupportedException("Microsoft Edge is only available on Microsoft Windows.");
            }
            if (browser != Browser.Edge)
            {
                throw new ArgumentException("Options Mismatch: EdgeDriver can only launch with EdgeOptions");
            }

            IWebDriver driver = new EdgeDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, InternetExplorerOptions options, WindowSize windowSize = WindowSize.hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.WinNT))
            {
                throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
            }
            if (browser != Browser.InternetExplorer)
            {
                throw new ArgumentException("Options Mismatch: InternetExplorerDriver can only launch with InternetExplorerOptions");
            }

            IWebDriver driver = new InternetExplorerDriver(DriverPath, options);
            return SetWindowSize(driver, windowSize);
        }
        
        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, SafariOptions options, WindowSize windowSize = WindowSize.hd)
        {
            if (!Platform.CurrentPlatform.IsPlatformType(PlatformType.Mac))
            {
                throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
            }
            if (browser != Browser.Safari)
            {
                throw new ArgumentException("Options Mismatch: SafariDriver can only launch with SafariOptions");
            }
            // I suspect that the SafariDriver is already on the path as it is within the Safari executable.
            // I currently have no means to test this

            IWebDriver driver = new SafariDriver(options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Convenience method for setiing the Window Size to common values. (768P, 1080P and fullscreen)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            switch (windowSize)
            {
                case WindowSize.fullScreen:
                    driver.Manage().Window.FullScreen();
                    return driver;

                case WindowSize.fhd:
                    driver.Manage().Window.Position = Point.Empty;
                    driver.Manage().Window.Size = new Size(1920, 1080);
                    return driver;

                default:
                    driver.Manage().Window.Position = Point.Empty;
                    driver.Manage().Window.Size = new Size(1366, 768);
                    return driver;
            }
        }

        private static T AddHeadlessOption<T>(T options)
        {
            if (options is ChromeOptions)
            {
                ChromeOptions chromeOptions = options as ChromeOptions;
                chromeOptions.AddArguments("--headless");
                return options;
            }

            if (options is FirefoxOptions)
            {
                FirefoxOptions firefoxOptions = options as FirefoxOptions;
                firefoxOptions.AddArguments("--headless");
                return options;
            }

            throw new ArgumentException($"Headless mode is not currently supported for the requested browser.");
        }
    }
}
