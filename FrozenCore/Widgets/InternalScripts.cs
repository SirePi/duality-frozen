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
                Window window = inSource.Parent.GetComponent<Widget>() as Window;

                if (window != null)
                {
                    
                }
            }
        }

        internal class MaximizeButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                Window window = inSource.Parent.GetComponent<Widget>() as Window;

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

        internal class ScrollUpLeftButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                throw new NotImplementedException();
            }
        }

        internal class ScrollDownRightButtonLeftMouseDown : InternalScript
        {
            public override void Execute(Duality.GameObject inSource, object inParameter)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
