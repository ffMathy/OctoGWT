using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.Events;
using System.Threading;
using System.Linq;
using System.Drawing;

namespace OctoGWT.Facades
{
    internal sealed class ParallelWebDriverFacade : IDisposable
    {
        private static int windowOffset;

        private EventFiringWebDriver[] webDrivers;

        public ParallelWebDriverFacade(IEnumerable<EventFiringWebDriver> webDriversInput)
        {
            this.webDrivers = webDriversInput.ToArray();

            //reposition windows so that you can see what is going on.
            var distanceFactor = 200;
            var baseDistance = windowOffset++ * distanceFactor;

            var previousDriver = webDrivers.First();
            foreach(var driver in webDrivers.Skip(1))
            {
                var manage = driver.Manage();
                var window = manage.Window;

                var previousManage = previousDriver.Manage();
                var previousWindow = previousManage.Window;
                var previousPosition = previousWindow.Position;

                window.Position = new Point(baseDistance + previousPosition.X + distanceFactor, baseDistance + previousPosition.Y + distanceFactor);

                previousDriver = driver;
            }
        }

        private void ExecuteOnEachDriver(Action<EventFiringWebDriver> action)
        {
            Parallel.ForEach(webDrivers, action);
        }

        internal void WaitForElements(By by, Action<IEnumerable<IWebElement>> callback)
        {
            ExecuteOnEachDriver((driver) =>
            {
                IEnumerable<IWebElement> elements = null;
                while (elements == null || !elements.Any())
                {
                    elements = driver.FindElements(by);
                    Thread.Sleep(100);
                }

                callback(elements);
            });
        }

        internal void GetElements(By by, Action<IEnumerable<IWebElement>> callback)
        {
            ExecuteOnEachDriver((driver) =>
            {
                var elements = driver.FindElements(by);
                callback(elements);
            });
        }

        internal void NavigateToPage(string url)
        {
            ExecuteOnEachDriver((driver) =>
            {

                var navigate = driver.Navigate();
                var currentUrl = driver.Url;

                var navigated = false;

                EventHandler<WebDriverNavigationEventArgs> callback = null;
                callback = new EventHandler<WebDriverNavigationEventArgs>((object sender, WebDriverNavigationEventArgs e) =>
                {
                    if (e.Url != currentUrl)
                    {
                        driver.Navigated -= callback;

                        navigated = true;
                    }
                });

                driver.Navigated += callback;

                navigate.GoToUrl(url);

                while (!navigated)
                {
                    Thread.Sleep(10);
                }
            });
        }

        public void Dispose()
        {
            foreach (var driver in webDrivers)
            {
                driver.Close();
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}