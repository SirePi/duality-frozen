// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
	/// <summary>
	///
	/// </summary>
	public class Appearance
	{
		/// <summary>
		/// 
		/// </summary>
		public Vector4 Border { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int Normal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int Hover { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int Active { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int Disabled { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public int GetAtlasForStatus(Widget.WidgetStatus status)
		{
			int result = 0;

			switch (status)
			{
				case Widget.WidgetStatus.Normal:
					result = Normal;
					break;

				case Widget.WidgetStatus.Hover:
					result = Hover;
					break;

				case Widget.WidgetStatus.Active:
					result = Active;
					break;

				case Widget.WidgetStatus.Disabled:
					result = Disabled;
					break;
			}

			return result;
		}
	}
}