using OctoGWT.Facades;
using System;

namespace OctoGWT.ContextChains
{
    public sealed class ThenContextChain : ContextChainBase
    {
        private WhenContextChain whenContext;
        internal WhenContextChain WhenContext
        {
            get
            {
                return whenContext;
            }
        }

        internal override ContextBase StartContext
        {
            get
            {
                return whenContext.GivenContext.StartContext;
            }
        }

        private Action<ThenWebDriverFacade> action;

        internal ThenContextChain(WhenContextChain whenContext, Action<ThenWebDriverFacade> action)
        {
            this.action = action;
            this.whenContext = whenContext;

            //find all the contexts.
            var givenContext = whenContext.GivenContext;
            var startContext = givenContext.StartContext;

            //append this context to be run by the start context.
            startContext.QueueContextForRunning(this);
        }

        internal void Run(ParallelWebDriverFacade browser)
        {
            //execute the then clause.
            action(new ThenWebDriverFacade(browser));

            //increment run count.
            RunCount++;
        }
    }
}
