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

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => Platform.CurrentPlatform.IsPlatformType(PlatformType.Mac));
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Chrome)]
        public void LocalWebDriverCanBeLaunchedAndLoadExampleDotCom(Browser browser)
        {
            Driver = WebDriverFactory.GetHdLocalWindowsWebDriver(browser);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        public void CallingForSafariWebDriverThrowsException()
        {
            Action act = () => WebDriverFactory.GetLocalWindowsWebDriver(Browser.Safari);
            act.Should().ThrowExactly<PlatformNotSupportedException>("because local running is only supported on Windows.");
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
