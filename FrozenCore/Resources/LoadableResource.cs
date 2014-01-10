using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore
{
    public abstract class LoadableResource : Resource
    {
        public bool IsLoaded { get; protected set; }

        public LoadableResource()
        {
            IsLoaded = false;
        }

        public void LoadInBackground()
        {
            IsLoaded = _LoadInBackground();
        }

        protected abstract bool _LoadInBackground();
    }
}
