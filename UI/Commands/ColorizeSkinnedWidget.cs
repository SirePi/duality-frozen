// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Commands;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Commands
{
	/// <summary>
	/// TimedCommand used to alter over time the colorization of a Widget
	/// </summary>
	public sealed class ColorizeSkinnedWidget : TimedCommand<Widget>
	{
		private ColorRange _range;
		private ColorRgba _target;

		internal ColorizeSkinnedWidget(GameObject inGameObject, ColorRgba inTargetColor)
		{
			_target = inTargetColor;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inSecondsPast"></param>
		/// <param name="inGameObject"></param>
		public override void Execute(float inSecondsPast, GameObject inGameObject)
		{
			Widget sw = GetComponent(inGameObject);

			if (_timeToComplete <= 0)
			{
				Colorize(sw, _range.Max);
				IsComplete = true;
			}
			else
			{
				_timePast += inSecondsPast;

				if (_timePast >= _timeToComplete)
				{
					Colorize(sw, _range.Max);
					IsComplete = true;
				}
				else
				{
					Colorize(sw, _range.Lerp(_timePast / _timeToComplete));
				}
			}
		}

		/// <summary>
		/// Initialization
		/// </summary>
		/// <param name="inGameObject"></param>
		public override void Initialize(GameObject inGameObject)
		{
			Widget sw = GetComponent(inGameObject);
			_range = new ColorRange(sw.Tint, _target);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		protected override float GetCommandLength()
		{
			return (_range.Max.ToVector4() - _range.Min.ToVector4()).Length;
		}

		private void Colorize(Widget inWidget, ColorRgba inColor)
		{
			inWidget.Tint = inColor;

			if (inWidget is Button)
			{
				(inWidget as Button).TextColor = inColor;
			}
		}
	}
}