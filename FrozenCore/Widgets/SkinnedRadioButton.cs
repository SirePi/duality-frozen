using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Components;
using Duality.Components.Renderers;
using OpenTK;
using Duality.Resources;
using FrozenCore.Widgets.Skin;
using Duality.EditorHints;

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
            if (e.Button == OpenTK.Input.MouseButton.Left && !IsChecked)
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

        protected override void OnUpdate(float inSecondsPast)
        {
            
        }
    }
}
