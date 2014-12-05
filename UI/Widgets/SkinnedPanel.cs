// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Panel Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImagePanel)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedPanel : SkinnedWidget
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedPanel()
        {
            ActiveArea = Widgets.ActiveArea.None;
        }
    }
}