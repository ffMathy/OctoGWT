using OctoGWT.Facades;
using OctoGWT.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{

    public sealed class GivenContextChain : ContextChainBase
    {

        private ContextBase innerContext;

        internal override ContextBase StartContext
        {
            get
            {
                return innerContext;
            }
        }

        private Action<GivenWebDriverFacade>[] actions;

        private GivenContextChain(ContextBase innerContext)
        {
            this.innerContext = innerContext;
        }

        internal GivenContextChain(ContextBase innerContext, params Action<GivenWebDriverFacade>[] actions) : this(innerContext)
        {
            this.actions = actions;
        }

        internal GivenContextChain(ContextBase innerContext, params IGivenInstruction[] instructions) : this(innerContext)
        {
            this.actions = instructions
                .Select<IGivenInstruction, Action<GivenWebDriverFacade>>(i => g => i.Run(g))
                .ToArray();
        }

        public WhenContextChain When(params Action<WhenWebDriverFacade>[] conditions)
        {
            var context = new WhenContextChain(this, conditions);
            return context;
        }

        public WhenContextChain When(params IWhenInstruction[] instructions)
        {
            var context = new WhenContextChain(this, instructions);
            return context;
        }

        internal void Run()
        {
            //run all actions.
            var facade = new GivenWebDriverFacade(Browser);
            foreach(var action in actions)
            {
                action(facade);
            }

            //increment run count.
            RunCount++;
        }
    }
}
