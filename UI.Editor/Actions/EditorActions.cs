// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms;

namespace SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Actions
{
    /// <summary>
    /// Double-click binding for the editor
    /// </summary>
    public class EditorActionEditSkin : EditorSingleAction<WidgetSkin>
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get { return EditorRes.ActionDesc_EditSkin; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get { return EditorRes.ActionName_EditSkin; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool CanPerformOn(WidgetSkin obj)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool MatchesContext(string context)
        {
            return context == DualityEditorApp.ActionContextOpenRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public override void Perform(WidgetSkin skin)
        {
            SkinEditor se = new SkinEditor(skin);
            if (se.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                skin = se.ModifiedSkin;
            }
        }
    }
}