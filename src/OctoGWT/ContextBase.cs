using OctoGWT.ContextChains;
using OctoGWT.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OctoGWT.Facades
{

    public abstract class ContextBase : IDisposable
    {
        //TODO: documentation including parallel.

        private LinkedList<ThenContextChain> pendingContexts;

        private string testName;

        protected abstract IEnumerable<Func<IWebDriver>> WebDriverConstructors { get; }

        /// <summary>
        /// Gets the name of the test method that this context is defined within.
        /// </summary>
        internal string TestName
        {
            get
            {
                return testName;
            }
        }

        protected ContextBase()
        {
            if (!WebDriverConstructors.Any())
            {
                throw new Exception("Can't use a context that doesn't spawn at least one web driver. Add more constructors.");
            }

            //since all classes are sealed, we can infer the test method that was invoked using the stack.
            var stack = new StackTrace();
            var frame = stack.GetFrame(2);
            var method = frame.GetMethod();

            //initialize all fields.
            this.testName = method.Name;
            this.pendingContexts = new LinkedList<ThenContextChain>();
        }

        internal void QueueContextForRunning(ThenContextChain context)
        {
            pendingContexts.AddLast(context);
        }

        private void Run()
        {
            if (pendingContexts.Count == 0)
            {
                throw new InvalidOperationException("No 'Given When Then' has been added. Chain the Given, When and Then method calls to add some.");
            }

            //first do some cleaning up if possible - we don't need earlier reports.
            if(Directory.Exists("Reports"))
            {
                Directory.Delete("Reports", true);
            }

            //keep track of the order that tests ran in.
            var globalTestNumber = 0;

            //work through a copy of the pending contexts to avoid foreach problems.
            Parallel.ForEach(pendingContexts, (thenContext) =>
            {
                //increment test offset.
                var testNumber = Interlocked.Increment(ref globalTestNumber);

                //construct all web drivers and append them to the wrapper class.
                var webDrivers = new List<EventFiringWebDriver>();

                //construct all drivers.
                Parallel.ForEach(WebDriverConstructors, driverConstructor =>
                {
                    var webDriver = new EventFiringWebDriver(driverConstructor());
                    lock (webDrivers)
                    {
                        webDrivers.Add(webDriver);
                    }
                });

                //construct a driver facade that takes care of underlying drivers.
                var browserFacade = new ParallelWebDriverFacade(webDrivers);

                //get the rest of the contexts.
                var whenContext = thenContext.ParentChain;
                var givenContext = whenContext.ParentChain;
                var startContext = givenContext.ParentChain;

                //by now the start context should be the current instance.
                Debug.Assert(startContext == this);

                //do the next part safely, so that we're sure we can terminate all driver instances left behind.
                try
                {

                    //run the given part.
                    givenContext.Run(browserFacade);

                    //now that we have the given part, run the when part.
                    whenContext.Run(browserFacade);

                    //run the then context.
                    thenContext.Run(browserFacade);

                }
                finally
                {
                    //kill all existing browsers in parallel.
                    Parallel.ForEach(webDrivers, driver =>
                    {
                        try
                        {
                            driver.Close();
                        }
                        catch { }
                        try
                        {
                            driver.Quit();
                        }
                        catch { }
                        try
                        {
                            driver.Dispose();
                        }
                        catch { }
                    });

                    //now let's store the reports we have.
                    var reports = browserFacade.StepReports.ToArray();
                    for (var i = 0; i < reports.Length; i++)
                    {
                        var report = reports[i];

                        //generate a path name that tells something about the order, and the order.
                        var basePath = Path.Combine("Reports", testName, testNumber + "", report.CategoryName);

                        //prepare a folder structure for this kind of report.
                        Directory.CreateDirectory(basePath);

                        //now save the screenshot.
                        var fileName = string.Format("{0}.{1}.{2}.{3}.png", report.Offset, report.HasFailed ? "Failed" : "Succeeded", report.MethodName, report.DriverName);
                        var screenshotPath = Path.Combine(basePath, fileName);

                        var screenshot = report.Screenshot;
                        screenshot.SaveAsFile(Path.Combine(basePath, fileName), ImageFormat.Png);
                    }
                }

            });
        }

        public GivenContextChain Given(params Action<GivenWebDriverFacade>[] actions)
        {
            var context = new GivenContextChain(this, actions);
            return context;
        }

        public GivenContextChain Given(params IGivenInstruction[] instructions)
        {
            var context = new GivenContextChain(this, instructions);
            return context;
        }

        public virtual void Dispose()
        {
            Run();
        }

    }
}
