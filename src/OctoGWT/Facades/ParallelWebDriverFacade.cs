using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.Events;
using System.Threading;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using OctoGWT.Reports;
using OctoGWT.Extensions;
using OctoGWT.Interfaces;

namespace OctoGWT.Facades
{
    internal sealed class ParallelWebDriverFacade
    {
        private static volatile int windowOffset;

        private int globalStepOffset;

        private IEnumerable<EventFiringWebDriver> webDrivers;
        private ICollection<WebDriverStepReport> stepReports;

        public IEnumerable<WebDriverStepReport> StepReports
        {
            get
            {
                return stepReports;
            }
        }

        public ParallelWebDriverFacade(params EventFiringWebDriver[] webDriversInput) : this((IEnumerable<EventFiringWebDriver>)webDriversInput) { }

        public ParallelWebDriverFacade(IEnumerable<EventFiringWebDriver> webDriversInput)
        {
            this.stepReports = new List<WebDriverStepReport>();
            this.webDrivers = webDriversInput;

            //reposition windows so that you can see what is going on.
            var distanceFactor = 50;

            foreach (var driver in webDrivers)
            {
                var manage = driver.Manage();
                var window = manage.Window;

                window.Position = new Point(windowOffset, windowOffset);
                windowOffset += distanceFactor;
            }
        }

        private void ExecuteOnEachDriver(Action<EventFiringWebDriver> action)
        {
            //get the step offset.
            var stepOffset = Interlocked.Increment(ref globalStepOffset);

            //first infer some information about the method invoked.
            var currentType = this.GetType();

            var stack = new StackTrace();
            var frames = stack.GetFrames();

            var traceMethod = (MethodBase)null;
            var traceType = (Type)null;
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                var type = method.DeclaringType;

                if (type != currentType)
                {
                    traceMethod = method;
                    traceType = type;
                    break;
                }
            }

            //now we can fetch the name of the method that invoked this method.
            var methodName = traceMethod.Name;

            //and we can infer the clause that we are running in by looking at the name of the class.
            var typeName = traceType.Name;

            var typeNameWords = typeName.ExtractWords();
            var clauseName = typeNameWords.First();

            //get the clause order. first given, then when, then then.
            var clauseOrder = GetClauseOrder(clauseName);

            //insert the order into the category.
            var category = string.Format("{0}.{1}", clauseOrder, clauseName);

            //let's see if this was all part of an instruction set. if it was, we can infer its name and append it to the clause name.
            var instructionSetFound = false;
            for (var i = 0; i < frames.Length && !instructionSetFound; i++)
            {
                var frame = frames[i];

                //get the method of the frame.
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var implementedTypes = type.GetInterfaces();

                for (var o = 0; o < implementedTypes.Length && !instructionSetFound; o++)
                {
                    var implementedType = implementedTypes[o];
                    if (implementedType.IsGenericType)
                    {
                        var genericTypeDefinition = implementedType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(IInstruction<>))
                        {
                            var instructionName = type.Name;
                            category += string.Format(".{0}", instructionName);

                            instructionSetFound = true;
                        }
                    }
                }
            }

            //run the task on all drivers, and save a screenshot at the end.
            Parallel.ForEach(webDrivers, (driver) =>
            {
                //get the name of the browser.
                var innerDriver = driver.WrappedDriver;
                var driverType = innerDriver.GetType();

                var driverName = driverType.Name;

                //be ready to catch an exception if one occurs.
                var exception = (Exception)null;
                try
                {

                    //run the action on the driver.
                    action(driver);

                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    //take a screenshot if it's supported.
                    var screenshot = (Screenshot)null;

                    var screenshotDriver = driver as ITakesScreenshot;
                    if (screenshotDriver != null)
                    {
                        screenshot = screenshotDriver.GetScreenshot();
                    }

                    //generate a step report and add it.
                    var report = new WebDriverStepReport(stepOffset, category, methodName, driverName, screenshot, exception);
                    lock (stepReports)
                    {
                        stepReports.Add(report);
                    }

                    if (exception != null)
                    {
                        throw exception;
                    }
                }
            });
        }

        private int GetClauseOrder(string clauseName)
        {
            switch(clauseName)
            {
                case "Given":
                    return 1;

                case "When":
                    return 2;

                case "Then":
                    return 3;

                default:
                    throw new ArgumentException("You have to provide a valid clause name.", "clauseName");
            }
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
    }
}