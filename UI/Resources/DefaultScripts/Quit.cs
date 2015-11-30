// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources.DefaultScripts
{
	/// <summary>
	/// Command used to quit the game
	/// </summary>

	[EditorHintImage(ResNames.ImageScript)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public sealed class Quit : Script
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="inSource"></param>
		/// <param name="inParameter"></param>
		public override void Execute(GameObject inSource, object inParameter)
		{
			DualityApp.Terminate();
		}
	}
}