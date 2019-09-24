using System;
using System.IO;
using System.Runtime.CompilerServices;
using AlexanderOnTest.NewNetPageFactory.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using LogLevel = AlexanderOnTest.NewNetPageFactory.Logging.LogLevel;

namespace AlexanderOnTest.NewNetPageFactory
{
    /// <summary>
    /// An immutable implementation of a WebDriverWait.
    /// </summary>
    internal class ImmutableWait : IWait<IWebDriver>
    {
        private static readonly ILog Logger = LogProvider.For<ImmutableWait>();
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled();

        private readonly string parentClassName;
        private readonly WebDriverWait webDriverWait;


        public ImmutableWait(IWebDriver driver, TimeSpan timeout, [CallerFilePath] string parentClassName = null)
        {
            this.parentClassName = Path.GetFileNameWithoutExtension(parentClassName);
            this.webDriverWait = new WebDriverWait(driver, timeout);
        }

        public ImmutableWait(IClock clock, IWebDriver driver, TimeSpan timeout, TimeSpan pollingInterval, [CallerFilePath] string parentClassName = null)
        {
            this.parentClassName = Path.GetFileNameWithoutExtension(parentClassName);
            this.webDriverWait = new WebDriverWait(clock, driver, timeout, pollingInterval);
        }

        public void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            this.webDriverWait.IgnoreExceptionTypes(exceptionTypes);
        }

        public TResult Until<TResult>(Func<IWebDriver, TResult> condition)
        {
            return this.webDriverWait.Until(condition);
        }

        public TimeSpan Timeout
        {
            get => this.webDriverWait.Timeout;
            set
            {
                var message =
                    $"Unsupported attempt made to alter the timeout of the immutable Wait for {parentClassName} to {value}.";
                Logger.Log(LogLevel.Error,
                    () => message);
            }
        }

        public TimeSpan PollingInterval
        {
            get => this.webDriverWait.PollingInterval;
            set
            {
                var message =
                    $"Unsupported attempt made to alter the polling interval of the immutable Wait for {parentClassName} to {value}.";
                Logger.Log(LogLevel.Error,
                    () => message);
            }
        }

        public string Message
        {
            get => this.webDriverWait.Message;
            set
            {
                var message =
                    $"Unsupported attempt made to alter the message of the immutable Wait for {parentClassName} to {value}.";
                Logger.Log(LogLevel.Error, () => message);
            }
        }
    }
}
