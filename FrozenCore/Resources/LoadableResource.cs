// This code is provided under the MIT license. Originally by Alessandro Pilati.

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