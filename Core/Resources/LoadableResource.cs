// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LoadableResource : Resource
    {
        /// <summary>
        ///
        /// </summary>
        public LoadableResource()
        {
            IsLoaded = false;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsLoaded { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public void LoadInBackground()
        {
            IsLoaded = _LoadInBackground();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool _LoadInBackground();
    }
}