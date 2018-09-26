using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.WebDriverFactory.MacOsTests
{
    [TestFixture]
    public class WebDriverFactoryTests
    {
        private IWebDriver Driver { get; set; }
        private readonly PlatformType thisPlatformType = PlatformType.Mac;

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => Platform.CurrentPlatform.IsPlatformType(thisPlatformType));
        }

        [Test]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Safari)]
        public void LocalWebDriverCanBeLaunchedAndLoadExampleDotCom(Browser browser)
        {
            Driver = WebDriverFactory.GetLocalWebDriver(browser);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        public void CallingUnsupportedWebDriverThrowsException(Browser browser)
        {
            Action act = () => WebDriverFactory.GetLocalWebDriver(browser);
            act.Should().ThrowExactly<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatformType}.");
        }

        [Test]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Firefox)]
        public void HeadlessBrowsersCanBeLaunched(Browser browser)
        {
            Driver = WebDriverFactory.GetLocalWebDriver(browser, true);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
