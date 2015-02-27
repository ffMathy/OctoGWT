using OctoGWT.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.Interfaces
{
    public interface IWhenInstruction
    {
        void Run(WhenWebDriverFacade w);
    }
}
