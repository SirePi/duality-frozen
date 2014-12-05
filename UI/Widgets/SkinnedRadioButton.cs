// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Linq;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A RadioButton Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageRadioButton)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedRadioButton : SkinnedCheckButton
    {
        private string _radioGroup;

        /// <summary>
        /// The RadioGroup this button is part of
        /// </summary>
        public string RadioGroup
        {
            get { return _radioGroup; }
            set { _radioGroup = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Left && _isMouseOver && !IsChecked)
                {
                    IsChecked = true;

                    if (!String.IsNullOrWhiteSpace(RadioGroup))
                    {
                        foreach (SkinnedRadioButton button in Scene.Current.FindComponents<SkinnedRadioButton>().Where(rb => rb.RadioGroup == this.RadioGroup && rb != this))
                        {
                            button.IsChecked = false;
                        }
                    }
                }
            }
        }
    }
}