// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;

namespace FrozenCore
{
    [Serializable]
    public abstract class Script : Resource
    {
        public void Execute(GameObject inSource)
        {
            Execute(inSource, null);
        }

        public abstract void Execute(GameObject inSource, object inParameter);

        public ContentRef<Script> ToScriptContentRef()
        {
            return new ContentRef<Script>(this);
        }
    }
}