// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Threading;
using Duality;

namespace FrozenCore.Components
{
    [Serializable]
    public abstract class ThreadedLoader : Component, ICmpUpdatable, ICmpInitializable
    {
        [NonSerialized]
        private System.Threading.Thread _loadingThread;

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

        public abstract void LoaderOnUpdate();

        public abstract void LoadingComplete();
    }
}