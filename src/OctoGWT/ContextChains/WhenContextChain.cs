using OctoGWT.Facades;
using OctoGWT.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{
    public sealed class WhenContextChain : ContextChainBase
    {
        private GivenContextChain givenContext;
        internal GivenContextChain GivenContext
        {
            get
            {
                return givenContext;
            }
        }

        internal override ContextBase StartContext
        {
            get
            {
                return givenContext.StartContext;
            }
        }

        private Action<WhenWebDriverFacade>[] conditions;

        private WhenContextChain(GivenContextChain givenContext)
        {
            this.givenContext = givenContext;
        }

        internal WhenContextChain(GivenContextChain givenContext, params Action<WhenWebDriverFacade>[] conditions) : this(givenContext)
        {
            this.conditions = conditions;
        }

        internal WhenContextChain(GivenContextChain givenContext, params IWhenInstruction[] instructions) : this(givenContext)
        {
            this.conditions = instructions
                .Select<IWhenInstruction, Action<WhenWebDriverFacade>>(i => g => i.Run(g))
                .ToArray();
        }

        public ThenContextChain Then(Action<ThenWebDriverFacade> action)
        {
            var context = new ThenContextChain(this, action);
            return context;
        }

        public ThenContextChain Then(IThenInstruction instruction)
        {
            var context = new ThenContextChain(this, d => instruction.Run(d));
            return context;
        }

        internal void Run(ParallelWebDriverFacade browser)
        {
            //run all conditions and check if they all meet.
            var facade = new WhenWebDriverFacade(browser);
            foreach(var condition in conditions)
            {
                condition(facade);
            }

            //increment the run count.
            RunCount++;
        }
    }
}
