using OctoGWT.Facades;
using OctoGWT.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{
    public sealed class WhenContextChain : ContextChainBase<GivenContextChain, IWhenInstruction, WhenWebDriverFacade>
    {
        internal WhenContextChain(GivenContextChain parentChain, params IWhenInstruction[] instructions) : base(parentChain, instructions)
        {
        }

        internal WhenContextChain(GivenContextChain parentChain, params Action<WhenWebDriverFacade>[] actions) : base(parentChain, actions)
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

        internal override WhenWebDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser)
        {
            return new WhenWebDriverFacade(browser);
        }
    }
}
