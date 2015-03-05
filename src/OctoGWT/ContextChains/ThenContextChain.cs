using OctoGWT.Facades;
using OctoGWT.Interfaces;
using System;

namespace OctoGWT.ContextChains
{
    public sealed class ThenContextChain : ContextChainBase<WhenContextChain, IThenInstruction, ThenWebDriverFacade>
    {
        /// <summary>
        /// Adds the current Given When Then chain to the run queue.
        /// </summary>
        private void AddGivenWhenThen()
        {
            var whenContext = this.ParentChain;
            var givenContext = whenContext.ParentChain;
            var startContext = givenContext.ParentChain;

            startContext.QueueContextForRunning(this);
        }

        internal ThenContextChain(WhenContextChain parentChain, params IThenInstruction[] instructions) : base(parentChain, instructions)
        {
            AddGivenWhenThen();
        }

        internal ThenContextChain(WhenContextChain parentChain, params Action<ThenWebDriverFacade>[] actions) : base(parentChain, actions)
        {
            AddGivenWhenThen();
        }

        internal override ThenWebDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser)
        {
            return new ThenWebDriverFacade(browser);
        }
    }
}
