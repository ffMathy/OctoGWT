﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.Exceptions
{
    public sealed class ThenException : Exception
    {
        public ThenException(string message) : base(message)
        {
        }
    }
}
