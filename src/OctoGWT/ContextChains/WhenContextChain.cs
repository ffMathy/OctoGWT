using System;
using OctoGWT.Facades;
using OctoGWT.Interfaces;

namespace OctoGWT.ContextChains
{
    public sealed class WhenContextChain : ContextChainBase<GivenContextChain, IWhenInstruction, GivenWhenWebDriverFacade>
    {
        internal WhenContextChain(GivenContextChain parentChain, params IWhenInstruction[] instructions) : base(parentChain, instructions)
        {
        }

        internal WhenContextChain(GivenContextChain parentChain, params Action<GivenWhenWebDriverFacade>[] actions)
            : base(parentChain, actions)
        {
        }

        public ThenContextChain Then(params Action<ThenWebDriverFacade>[] actions)
        {
            var context = new ThenContextChain(this, actions);
            return context;
        }

        public ThenContextChain Then(params IThenInstruction[] instructions)
        {
            var context = new ThenContextChain(this, instructions);
            return context;
        }

        internal override GivenWhenWebDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser)
        {
            return new GivenWhenWebDriverFacade(browser);
        }
    }
}
