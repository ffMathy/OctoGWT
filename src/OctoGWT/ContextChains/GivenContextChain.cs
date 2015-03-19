using System;
using OctoGWT.Facades;
using OctoGWT.Interfaces;

namespace OctoGWT.ContextChains
{

    public sealed class GivenContextChain : ContextChainBase<ContextBase, IGivenInstruction, GivenWhenWebDriverFacade>
    {
        internal GivenContextChain(ContextBase parentChain, params IGivenInstruction[] instructions) : base(parentChain, instructions)
        {
        }

        internal GivenContextChain(ContextBase parentChain, params Action<GivenWhenWebDriverFacade>[] actions) : base(parentChain, actions)
        {
        }

        public WhenContextChain When(params Action<GivenWhenWebDriverFacade>[] actions)
        {
            var context = new WhenContextChain(this, actions);
            return context;
        }

        public WhenContextChain When(params IWhenInstruction[] instructions)
        {
            var context = new WhenContextChain(this, instructions);
            return context;
        }

        internal override GivenWhenWebDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser)
        {
            return new GivenWhenWebDriverFacade(browser);
        }
    }
}
