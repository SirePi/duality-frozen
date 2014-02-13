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
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedScrollBar : SkinnedWidget<ScrollBarSkin>
    {
        private int _min;
        private int _max;
        private int _value;

        public int Minimum
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Maximum
        {
            get { return _max; }
            set { _max = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SkinnedScrollBar()
        {
            _min = 1;
            _max = 100;
            _value = _min;
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {

            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            
        }

        internal override Polygon GetActiveAreaOnScreen(Camera inCamera)
        {
            return Polygon.NO_POLYGON;
        }

        private void AddDecreaseButton()
        {
            GameObject button = new GameObject("decreaseButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, -DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = button.AddComponent<RestoreButton>();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = new ContentRef<BaseSkin>(Skin.Res.ButtonsSkin);
            rb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);
            rb.IsWidgetEnabled = false;

            Scene.Current.AddObject(button);
        }

        private void AddIncreaseButton()
        {
            GameObject button = new GameObject("increaseButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, -DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = button.AddComponent<RestoreButton>();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = new ContentRef<BaseSkin>(Skin.Res.ButtonsSkin);
            rb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);
            rb.IsWidgetEnabled = false;

            Scene.Current.AddObject(button);
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            
        }
    }
}
