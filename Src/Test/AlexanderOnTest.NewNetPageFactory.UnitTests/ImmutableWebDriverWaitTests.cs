using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using log4net;
using log4net.Config;
using log4net.Repository;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ILog = log4net.ILog;

namespace AlexanderOnTest.NewNetPageFactory.UnitTests
{
    [TestFixture]
    public class ImmutableWebDriverWaitTests
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled;

        private IWebDriver driver;
        private IClock clock;
        private readonly Type type = typeof(ImmutableWebDriverWaitTests);

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "log4net.config")));

            this.driver = A.Fake<IWebDriver>();
            this.clock = A.Fake<IClock>();
        }

        [TestCase(30000)]
        [TestCase(300)]
        public void DefaultConstructor1GeneratesDefaultValues(int timeoutInMillisseconds)
        {
            IWait<IWebDriver> wait = new ImmutableWait(
                driver, 
                TimeSpan.FromMilliseconds(timeoutInMillisseconds));

            using (new AssertionScope())
            {
                wait.Timeout.Should().Be(TimeSpan.FromMilliseconds(timeoutInMillisseconds));
                wait.Message.Should().Be(string.Empty);
                wait.PollingInterval.Should().Be(TimeSpan.FromMilliseconds(500));
            }
        }

        [TestCase(30000, 500)]
        [TestCase(300, 100)]
        public void DefaultConstructor2GeneratesDefaultValues(int timeoutInMilliseconds, int pollingIntervalInMilliseconds)
        {
            IWait<IWebDriver> wait = new ImmutableWait(
                this.clock, 
                this.driver, 
                TimeSpan.FromMilliseconds(timeoutInMilliseconds), 
                TimeSpan.FromMilliseconds(pollingIntervalInMilliseconds));

            using (new AssertionScope())
            {
                wait.Timeout.Should().Be(TimeSpan.FromMilliseconds(timeoutInMilliseconds));
                wait.Message.Should().Be(string.Empty);
                wait.PollingInterval.Should().Be(TimeSpan.FromMilliseconds(pollingIntervalInMilliseconds));
            }
        }

        [Test]
        public void TryingToModifyIgnoredExceptionsThrowsException()
        {
            TimeSpan initialTimeout = TimeSpan.FromSeconds(30);

            Trace.WriteLine($"Test: {nameof(TryingToSetTimeoutFailsSilently)} should log an error below.");
            IWait<IWebDriver> wait = new ImmutableWait(
                driver,
                initialTimeout);
            
            Action act = () => wait.IgnoreExceptionTypes(new Type[]{typeof(WebDriverException)});

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Unsupported attempt to modify the ignored Exceptions of the immutable Wait for ImmutableWebDriverWaitTests");
        }

        [Test]
        public void TryingToSetTimeoutFailsSilently()
        {
            TimeSpan initialTimeout = TimeSpan.FromSeconds(30);

            Trace.WriteLine($"Test: {nameof(TryingToSetTimeoutFailsSilently)} should log an error below.");
            IWait<IWebDriver> wait = new ImmutableWait(
                driver,
                initialTimeout);

            wait.Timeout = TimeSpan.FromSeconds(3);
            Trace.WriteLine($"Test: {nameof(TryingToSetTimeoutFailsSilently)} should have logged an error above.");

            wait.Timeout.Should().Be(initialTimeout);
        }

        [Test]
        public void TryingToSetPollingIntervalFailsSilently()
        {
            TimeSpan initialTimeout = TimeSpan.FromSeconds(30);

            IWait<IWebDriver> wait = new ImmutableWait(
                driver,
                initialTimeout);


            Trace.WriteLine($"Test: {nameof(TryingToSetPollingIntervalFailsSilently)} should log an error below.");
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            Trace.WriteLine($"Test: {nameof(TryingToSetPollingIntervalFailsSilently)} should log an error below.");

            wait.PollingInterval.Should().Be(TimeSpan.FromMilliseconds(500));
        }

        [Test]
        public void TryingToSetAMessageFailsSilently()
        {
            TimeSpan initialTimeout = TimeSpan.FromSeconds(30);

            IWait<IWebDriver> wait = new ImmutableWait(
                driver,
                initialTimeout);

            Trace.WriteLine($"Test: {nameof(TryingToSetAMessageFailsSilently)} should log an error below.");
            wait.Message = "This is a new message";
            Trace.WriteLine($"Test: {nameof(TryingToSetAMessageFailsSilently)} should log an error below.");

            wait.Message.Should().Be(string.Empty);
        }
    }
}
