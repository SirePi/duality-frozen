// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore
{
    public abstract class Script : Resource
    {
        public void Execute(GameObject inSource)
        {
            Execute(inSource, null);
        }

        public abstract void Execute(GameObject inSource, object inParameter);
    }
}
