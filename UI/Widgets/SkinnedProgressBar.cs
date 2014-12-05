// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Progressbar Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageProgressBar)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedProgressBar : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private GameObject _bar;

        [NonSerialized]
        private FormattedText _fText;

        #endregion NonSerialized fields

        private ContentRef<WidgetSkin> _barSkin;
        private string _text;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;
        private int _value;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedProgressBar()
        {
            ActiveArea = Widgets.ActiveArea.None;

            _fText = new FormattedText();
            _textColor = Colors.White;
        }

        /// <summary>
        /// [GET / SET] the Skin used for the ProgressBar
        /// </summary>
        public ContentRef<WidgetSkin> BarSkin
        {
            get { return _barSkin; }
            set
            {
                _barSkin = value;
                _dirtyFlags |= DirtyFlags.Custom1;
            }
        }
        /// <summary>
        /// [GET / SET] the Text of the ProgressBar
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// [GET / SET] the Color of the Text
        /// </summary>
        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }
        /// <summary>
        /// [GET / SET] the Font of the Text
        /// </summary>
        public ContentRef<Font> TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }
        /// <summary>
        /// [GET / SET] The value of the progressbar, from 0 to 100
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _dirtyFlags |= DirtyFlags.Custom2;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_bar == null && _barSkin != null)
            {
                AddBar();
            }

            if (_bar != null)
            {
                if ((_dirtyFlags & DirtyFlags.Custom1) != DirtyFlags.None)
                {
                    _bar.GetComponent<SkinnedWidget>().Skin = _barSkin;
                }
                if ((_dirtyFlags & DirtyFlags.Custom2) != DirtyFlags.None)
                {
                    UpdateBar();
                }

                _bar.GetComponent<Widget>().Status = Status;
            }

            base.OnUpdate(inSecondsPast);
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
            sp.Rect = Rect.AlignLeft(-Rect.W / 2 + Skin.Res.Border.X, 0, 0, Rect.H - Skin.Res.Border.Y - Skin.Res.Border.W);

            _bar.AddComponent<SkinnedPanel>(sp);
            Scene.Current.AddObject(_bar);
        }

        private void UpdateBar()
        {
            _value = Math.Max(Value, 0);
            _value = Math.Min(Value, 100);

            if (_bar != null)
            {
                SkinnedWidget sw = _bar.GetComponent<SkinnedWidget>();
                Rect rect = sw.Rect;
                rect.W = (_vertices[6].Pos - _vertices[5].Pos).X * _value / 100;

                sw.Rect = rect;
            }
        }
    }
}