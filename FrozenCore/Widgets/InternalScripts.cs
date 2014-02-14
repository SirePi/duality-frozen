// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore.Widgets
{
    internal class InternalScripts
    {
        private static Dictionary<string, Duality.ContentRef<Script>> _scriptsCache;

        internal static Duality.ContentRef<Script> GetScript<T>() where T : InternalScript, new()
        {
            if (_scriptsCache == null)
            {
                _scriptsCache = new Dictionary<string, Duality.ContentRef<Script>>();
            }

            Type scriptType = typeof(T);

            if (!_scriptsCache.ContainsKey(scriptType.FullName))
            {
                InternalScript newScript = scriptType.GetConstructor(Type.EmptyTypes).Invoke(null) as InternalScript;
                _scriptsCache.Add(scriptType.FullName, new Duality.ContentRef<Script>(newScript));
            }

            return _scriptsCache[scriptType.FullName];
        }

        internal abstract class InternalScript : Script
        { }

        #region Internal Scripts
        internal class CloseButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                inSource.Parent.GetComponent<Widget>().Close();
            }
        }

        internal class MinimizeButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedWindow window = inSource.Parent.GetComponent<Widget>() as SkinnedWindow;

                if (window != null)
                {
                    
                }
            }
        }

        internal class MaximizeButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedWindow window = inSource.Parent.GetComponent<Widget>() as SkinnedWindow;

                if (window != null)
                {
                    Rect r = window.Rect;
                    r.W = r.W * 2;

                    window.Rect = r;
                }
            }
        }

        internal class RestoreButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                throw new NotImplementedException();
            }
        }

        internal class ScrollUpButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedScrollBar scrollBar = inSource.Parent.GetComponent<Widget>() as SkinnedScrollBar;
                if (scrollBar != null)
                {
                    scrollBar.Value = Math.Min(scrollBar.Maximum, scrollBar.Value + (int)inParameter);
                }
            }
        }

        internal class ScrollDownButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedScrollBar scrollBar = inSource.Parent.GetComponent<Widget>() as SkinnedScrollBar;
                if (scrollBar != null)
                {
                    scrollBar.Value = Math.Min(scrollBar.Minimum, scrollBar.Value - (int)inParameter);
                }
            }
        }

        #endregion
    }
}
