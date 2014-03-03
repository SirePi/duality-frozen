// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Linq;
using Duality.Resources;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedRadioButton : SkinnedCheckButton
    {
        private string _radioGroup;

        public string RadioGroup
        {
            get { return _radioGroup; }
            set { _radioGroup = value; }
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
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