using OctoGWT.Facades;
using OctoGWT.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{

    public sealed class GivenContextChain : ContextChainBase<ContextBase, IGivenInstruction, GivenWebDriverFacade>
    {
        internal GivenContextChain(ContextBase parentChain, params IGivenInstruction[] instructions) : base(parentChain, instructions)
        {
        }

        internal GivenContextChain(ContextBase parentChain, params Action<GivenWebDriverFacade>[] actions) : base(parentChain, actions)
        {
        }

        public WhenContextChain When(params Action<WhenWebDriverFacade>[] actions)
        {
            var context = new WhenContextChain(this, actions);
            return context;
        }

        public WhenContextChain When(params IWhenInstruction[] instructions)
        {
            var context = new WhenContextChain(this, instructions);
            return context;
        }

        internal override GivenWebDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser)
        {
            return new GivenWebDriverFacade(browser);
        }
    }
}
