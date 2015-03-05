using OctoGWT.Facades;
using OctoGWT.Interfaces;
using OctoGWT.Reports;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{
    public abstract class ContextChainBase<TParentChain, TInstruction, TDriverFacade> 
        where TInstruction : IInstruction<TDriverFacade> 
        where TDriverFacade : class
    {
        private IEnumerable<Action<TDriverFacade>> actions;

        private TParentChain parentChain;

        internal TParentChain ParentChain
        {
            get
            {
                return parentChain;
            }
        }

        private ContextChainBase(TParentChain parentChain)
        {
            this.parentChain = parentChain;
        }

        internal ContextChainBase(TParentChain parentChain, params Action<TDriverFacade>[] actions) : this(parentChain) {
            this.actions = actions;
        }

        internal ContextChainBase(TParentChain parentChain, params TInstruction[] instructions) : this(parentChain)
        {
            this.actions = instructions
                .Select<TInstruction, Action<TDriverFacade>>(i => g => i.Run(g));
        }

        internal abstract TDriverFacade ConstructNewFacade(ParallelWebDriverFacade browser);

        internal void Run(ParallelWebDriverFacade browser)
        {
            var facade = ConstructNewFacade(browser);
            foreach (var action in actions)
            {
                action(facade);
            }
        }

    }
}
