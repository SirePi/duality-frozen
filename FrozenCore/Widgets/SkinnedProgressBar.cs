﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;
using Duality.Resources;
using FrozenCore.Resources.Widgets;
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedProgressBar : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private GameObject _bar;

        #endregion NonSerialized fields

        private ContentRef<WidgetSkin> _barSkin;
        private ContentRef<Font> _textFont;
        private string _text;
        private ColorRgba _textColor;
        private int _value;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ContentRef<Font> TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public ContentRef<WidgetSkin> BarSkin
        {
            get { return _barSkin; }
            set { _barSkin = value; }
        }

        public int Value
        {
            get { return _value; }
            set 
            {
                _value = value;
                UpdateBar();
            }
        }

        public SkinnedProgressBar()
        {
            ActiveArea = Widgets.ActiveArea.None;

            _fText = new FormattedText();
            _textColor = Colors.White;
        }

        private void AddBar()
        {
            _bar = new GameObject("bar", this.GameObj);

            Transform t = _bar.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, 0);
            t.RelativeAngle = 0;

            SkinnedPanel sp = new SkinnedPanel();
            sp.VisibilityGroup = this.VisibilityGroup;
            sp.Skin = BarSkin;
            sp.Rect = Rect.AlignLeft(0, 0, 0, Rect.H);

            _bar.AddComponent<SkinnedPanel>(sp);
            Scene.Current.AddObject(_bar);
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_bar == null)
                {
                    AddBar();

                    _bar.GetComponent<Widget>().Status = Status;
                }

                UpdateBar();
            }
        }

        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (!String.IsNullOrWhiteSpace(_text))
            {
                Vector3 barCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                _fText.SourceText = _text;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_fText, barCenter.X, barCenter.Y, barCenter.Z + DELTA_Z, null, Alignment.Center);
                inCanvas.PopState();
            }
        }

        private void UpdateBar()
        {
            _value = Math.Max(Value, 0);
            _value = Math.Min(Value, 100);

            SkinnedWidget sw = _bar.GetComponent<SkinnedWidget>();
            Rect rect = sw.Rect;
            rect.W = 10;

            sw.Rect = rect;
        }
    }
}