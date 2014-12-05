// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Threading;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Components
{
    /// <summary>
    /// This Component is in charge of the asynchronous loading of a LoadableResource.
    /// The Resource is loaded using a separate thread working in background; the thread can be monitored
    /// by implementing the LoaderOnUpdate and LoadingComplete methods.
    /// <example>It's possible to have the LoadableResource set a volatile field from time to time that
    /// reflects the loading percentage. LoaderOnUpdate could be then be implemented in order to display
    /// such percentage, or affect a loading bar.</example>
    /// </summary>
    [Serializable]
    public abstract class ThreadedLoader : Component, ICmpUpdatable, ICmpInitializable
    {
        [NonSerialized]
        private System.Threading.Thread _loadingThread;

        /// <summary>
        ///
        /// </summary>
        public ContentRef<LoadableResource> ResourceToLoad { get; set; }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if (context == InitContext.Activate)
            {
                if (ResourceToLoad.Res != null && _loadingThread == null)
                {
                    _loadingThread = new Thread(new ThreadStart(ResourceToLoad.Res.LoadInBackground));
                    _loadingThread.IsBackground = true;
                    _loadingThread.Start();
                }
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (_loadingThread != null && _loadingThread.IsAlive)
            {
                _loadingThread.Abort();
                _loadingThread = null;
            }
        }

        void ICmpUpdatable.OnUpdate()
        {
            LoaderOnUpdate();

            if (ResourceToLoad != null)
            {
                if (ResourceToLoad.Res.IsLoaded)
                {
                    LoadingComplete();
                    _loadingThread = null;
                }
            }
            else
            {
                LoadingComplete();
                _loadingThread = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public abstract void LoaderOnUpdate();

        /// <summary>
        ///
        /// </summary>
        public abstract void LoadingComplete();
    }
}