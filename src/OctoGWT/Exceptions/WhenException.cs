using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.Exceptions
{
    public sealed class WhenException : Exception
    {
        public WhenException(string message) : base(message)
        {
        }
    }
}
