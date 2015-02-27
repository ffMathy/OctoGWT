using OctoGWT.Facades;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.ContextChains
{
    public abstract class ContextChainBase
    {
        protected ContextChainBase() { }

        protected internal int RunCount { get; protected set; }

        internal abstract ContextBase StartContext { get; }
    }
}
