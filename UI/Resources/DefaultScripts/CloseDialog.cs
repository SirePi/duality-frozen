﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources.DefaultScripts
{
    /// <summary>
    /// Command used to close a Dialog window
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageScript)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class CloseDialog : Script
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inSource"></param>
        /// <param name="inParameter"></param>
        public override void Execute(GameObject inSource, object inParameter)
        {
            inSource.FindAncestorWithComponent<Widget>().GetComponent<Widget>().Close();
            Scene.Current.FindComponent<WidgetController>().SetDialogWindow(null);
        }
    }
}