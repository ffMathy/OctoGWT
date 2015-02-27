using OctoGWT.ContextChains;
using OctoGWT.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OctoGWT.Facades
{

    public abstract class ContextBase : IDisposable
    {
        //TODO: documentation including parallel.

        private LinkedList<ThenContextChain> pendingContexts;

        protected abstract IEnumerable<Func<IWebDriver>> WebDriverConstructors { get; }

        protected ContextBase()
        {
            if (!WebDriverConstructors.Any())
            {
                throw new Exception("Can't use a context that doesn't spawn at least one web driver. Add more constructors.");
            }

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

            //work through a copy of the pending contexts to avoid foreach problems.
            Parallel.ForEach(pendingContexts, (thenContext) =>
            {

                //construct all web drivers and append them to the wrapper class.
                var webDrivers = WebDriverConstructors.Select((constructor) => new EventFiringWebDriver(constructor()));
                using (var browser = new ParallelWebDriverFacade(webDrivers))
                {

                    //get the rest of the contexts.
                    var whenContext = thenContext.WhenContext;
                    var givenContext = whenContext.GivenContext;
                    var startContext = givenContext.StartContext;

                    Debug.Assert(startContext == this);

                    //run the given part.
                    givenContext.Run(browser);

                    //now that we have the given part, run the when part.
                    whenContext.Run(browser);

                    //run the then context.
                    thenContext.Run(browser);

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
