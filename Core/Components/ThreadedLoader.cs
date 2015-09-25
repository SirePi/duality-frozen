// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    public abstract class ThreadedLoader : Component, ICmpUpdatable, ICmpInitializable
    {
        [DontSerialize]
        private Task _task;

        [DontSerialize]
        private CancellationTokenSource _cts;

        /// <summary>
        ///
        /// </summary>
        public ContentRef<LoadableResource> ResourceToLoad { get; set; }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if (context == InitContext.Activate)
            {
                if (ResourceToLoad.Res != null && _task == null)
                {
                    _cts = new CancellationTokenSource();
                    _task = LoadAsync(_cts.Token);
                }
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (_task != null && !_task.IsCompleted)
            {
                _cts.Cancel();
                _task = null;
            }
        }

        void ICmpUpdatable.OnUpdate()
        {
            LoaderOnUpdate();

            if (ResourceToLoad == null || ResourceToLoad.Res.IsLoaded)
            {
                LoadingComplete();
                _task = null;
            }
        }

        private Task LoadAsync(CancellationToken cancellationToken)
        {
            return Task.Run(new Action(ResourceToLoad.Res.LoadInBackground));
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