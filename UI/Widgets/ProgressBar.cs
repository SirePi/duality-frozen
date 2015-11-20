// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Progressbar Widget
    /// </summary>

    [EditorHintImage(ResNames.ImageProgressBar)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class ProgressBar : Widget
    {
        #region NonSerialized fields

        [DontSerialize]
        private GameObject _bar;

        [DontSerialize]
        private FormattedText _fText;

        #endregion NonSerialized fields

        private ContentRef<ProgressBarAppearance> _progressAppearance;
        private string _text;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;
        private int _value;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressBar()
        {
            ActiveArea = ActiveArea.None;

            _fText = new FormattedText();
            _textColor = Colors.White;

            Appearance = DefaultGradientSkin.PROGRESSBAR;
        }

        /// <summary>
        /// [GET / SET] the Skin used for the ProgressBar
        /// </summary>
        public ContentRef<ProgressBarAppearance> Appearance
        {
            get { return _progressAppearance; }
            set
            {
                _progressAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
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
                _dirtyFlags |= DirtyFlags.Value;
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
            if (_bar == null && !_progressAppearance.Res.ProgressBar.IsExplicitNull)
            {
                AddBar();
            }

            if (_bar != null)
            {
                if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
                {
                    _bar.GetComponent<Panel>().Appearance = AppearanceManager.RequestAppearanceContentRef(_progressAppearance.Res.ProgressBar);
                }
                if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
                {
                    UpdateBar();
                }

                _bar.GetComponent<Widget>().Status = Status;
            }
        }

        private void AddBar()
        {
            _bar = new GameObject("bar", this.GameObj);

            Transform t = _bar.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, 0);
            t.RelativeAngle = 0;

            Panel sp = new Panel();
            sp.VisibilityGroup = this.VisibilityGroup;
            sp.Appearance = AppearanceManager.RequestAppearanceContentRef(_progressAppearance.Res.ProgressBar);
            sp.Rect = Rect.Align(Alignment.Left, -Rect.W / 2 + _progressAppearance.Res.Widget.Res.Border.X, 0, 0, Rect.H - _progressAppearance.Res.Widget.Res.Border.Y - _progressAppearance.Res.Widget.Res.Border.W);

            _bar.AddComponent<Panel>(sp);
			this.GameObj.ParentScene.AddObject(_bar);
        }

        private void UpdateBar()
        {
            _value = Math.Max(Value, 0);
            _value = Math.Min(Value, 100);

            if (_bar != null)
            {
                Panel sw = _bar.GetComponent<Panel>();
                Rect rect = sw.Rect;
                rect.W = (_vertices[6].Pos - _vertices[5].Pos).X * _value / 100;

                sw.Rect = rect;
            }
        }

        protected override Appearance GetBaseAppearance()
        {
            return _progressAppearance.Res.Widget.Res;
        }
    }
}