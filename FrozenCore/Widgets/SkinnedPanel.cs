// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedPanel : SkinnedWidget
    {
        public SkinnedPanel()
        {
            ActiveArea = Widgets.ActiveArea.None;
        }
    }
}