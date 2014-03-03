// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;

namespace FrozenCore.Widgets
{
    internal class InternalScripts
    {
        internal abstract class InternalScript : Script
        { }

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

        #region Internal Scripts

        internal class CloseButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                inSource.Parent.GetComponent<Widget>().Close();
            }
        }

        internal class MaximizeButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedWindow window = inSource.Parent.GetComponent<Widget>() as SkinnedWindow;

                if (window != null)
                {
                    window.Maximize();
                }
            }
        }

        internal class MinimizeButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedWindow window = inSource.Parent.GetComponent<Widget>() as SkinnedWindow;

                if (window != null)
                {
                    window.Minimize();
                }
            }
        }

        internal class RestoreButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedWindow window = inSource.Parent.GetComponent<Widget>() as SkinnedWindow;

                if (window != null)
                {
                    window.Restore();
                }
            }
        }

        internal class ScrollDecreaseButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                SkinnedScrollBar scrollBar = inSource.Parent.GetComponent<Widget>() as SkinnedScrollBar;
                if (scrollBar != null)
                {
                    scrollBar.Value = Math.Max(scrollBar.Minimum, scrollBar.Value - (int)inParameter);
                }
            }
        }

        internal class ScrollIncreaseButtonLeftMouseDown : InternalScript
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

        /*
        internal class MultiLineScrollbarValueChanged : InternalScript
        {
            public override void Execute(GameObject inSource, object inParameter)
            {
                SkinnedListBox slb = inSource.Parent.GetComponent<SkinnedListBox>();
                SkinnedTextBlock stb = inSource.Parent.GetComponent<SkinnedTextBlock>();

                if(slb != null)
                {
                    slb.ScrollChanged();
                }

                if (stb != null)
                {
                    stb.ScrollChanged();
                }
            }
        }*/

        #endregion Internal Scripts
    }
}