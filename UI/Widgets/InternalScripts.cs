// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	internal class InternalScripts
	{
		private static Dictionary<string, ContentRef<Script>> _scriptsCache;

		internal static ContentRef<Script> GetScript<T>() where T : InternalScript, new()
		{
			if (_scriptsCache == null)
			{
				_scriptsCache = new Dictionary<string, ContentRef<Script>>();
			}

			TypeInfo scriptType = typeof(T).GetTypeInfo();

			if (!_scriptsCache.ContainsKey(scriptType.FullName))
			{
				InternalScript newScript = scriptType.DeclaredConstructors.First().Invoke(null) as InternalScript;
				_scriptsCache.Add(scriptType.FullName, new ContentRef<Script>(newScript));
			}

			return _scriptsCache[scriptType.FullName];
		}

		internal abstract class InternalScript : Script
		{ }

		#region Internal Scripts

		internal class CloseButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				inSource.Parent.GetComponent<Widget>().Close();
			}
		}

		internal class MaximizeButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				Window window = inSource.Parent.GetComponent<Widget>() as Window;

				if (window != null)
				{
					window.Maximize();
				}
			}
		}

		internal class MinimizeButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				Window window = inSource.Parent.GetComponent<Widget>() as Window;

				if (window != null)
				{
					window.Minimize();
				}
			}
		}

		internal class RestoreButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				Window window = inSource.Parent.GetComponent<Widget>() as Window;

				if (window != null)
				{
					window.Restore();
				}
			}
		}

		internal class ScrollDecreaseButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				ScrollBar scrollBar = inSource.Parent.GetComponent<Widget>() as ScrollBar;
				if (scrollBar != null)
				{
					scrollBar.Value = Math.Max(scrollBar.Minimum, scrollBar.Value - (int)inParameter);
				}
			}
		}

		internal class ScrollIncreaseButtonLeftMouseDown : InternalScript
		{
			public override void Execute(GameObject inSource, object inParameter)
			{
				ScrollBar scrollBar = inSource.Parent.GetComponent<Widget>() as ScrollBar;
				if (scrollBar != null)
				{
					scrollBar.Value = Math.Min(scrollBar.Maximum, scrollBar.Value + (int)inParameter);
				}
			}
		}

		/*
		internal class CommandGridScrollbarValueChanged : InternalScript
		{
			public override void Execute(Duality.GameObject inSource, object inParameter)
			{
				SkinnedCommandGrid grid = inSource.Parent.GetComponent<SkinnedCommandGrid>();
				grid.ScrollValueChanged();
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