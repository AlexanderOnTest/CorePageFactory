using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.WebDriverFactory.LinuxTests
{
    [TestFixture]
    public class WebDriverFactoryTests
    {
        private IWebDriver Driver { get; set; }
        private readonly PlatformType thisPlatformType = PlatformType.Linux;

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => Platform.CurrentPlatform.IsPlatformType(thisPlatformType));
        }

        [Test]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Firefox)]
        public void LocalWebDriverCanBeLaunched(Browser browser)
        {
            Driver = WebDriverFactory.GetLocalWebDriver(browser);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        public void HeadlessBrowserCanBeLaunched(Browser browser)
        {
            Driver = WebDriverFactory.GetLocalWebDriver(browser, true);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            Action act = () => WebDriverFactory.GetLocalWebDriver(browser, true);
            act.Should().ThrowExactly<ArgumentException>($"because headless mode is not supported on {browser}.");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedWebDriverThrowsException(Browser browser)
        {
            Action act = () => WebDriverFactory.GetLocalWebDriver(browser);
            act.Should().ThrowExactly<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatformType}.");
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
