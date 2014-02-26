// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using FrozenCore.Widgets.Skin;
using OpenTK;
using Duality.Drawing;
using Duality.Editor;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedPanel : SkinnedWidget<BaseSkin>
    {
        public SkinnedPanel()
        {
            ActiveArea = Widgets.ActiveArea.None;
        }
    }
}