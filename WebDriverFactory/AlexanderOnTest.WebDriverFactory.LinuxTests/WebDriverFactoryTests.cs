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

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => Platform.CurrentPlatform.IsPlatformType(PlatformType.Linux));
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        public void LocalWebDriverCanBeLaunched(Browser browser)
        {
            Driver = WebDriverFactory.GetHdLocalWindowsWebDriver(browser);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Safari)]
        public void CallingUnsupportedWebDriverThrowsException(Browser browser)
        {
            Action act = () => WebDriverFactory.GetLocalWindowsWebDriver(browser);
            act.Should().ThrowExactly<PlatformNotSupportedException>($"because {browser} is not supported on Linux.");
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
