using SnowyPeak.Duality.Plugin.Frozen.Core.Components;
using SnowyPeak.Duality.Plugin.Frozen.UI.Commands;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI
{
	/// <summary>
	///
	/// </summary>
	public static class CommanderExtensions
	{
		/// <summary>
		/// Adds a ChangeWidgetStatus Command
		/// </summary>
		/// /// <param name="inCommander"></param>
		/// <param name="inStatus">Target Status</param>
		/// <returns>The ChangeWidgetStatus Command</returns>
		public static ChangeWidgetStatus ChangeWidgetStatus(this Commander inCommander, Widget.WidgetStatus inStatus)
		{
			return inCommander.Add(new ChangeWidgetStatus(inStatus));
		}
	}
}