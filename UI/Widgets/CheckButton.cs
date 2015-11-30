// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	/// <summary>
	/// A CheckButton Widget
	/// </summary>

	[EditorHintImage(ResNames.ImageCheckButton)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public class CheckButton : Widget
	{
		#region NonSerialized fields

		[DontSerialize]
		private FormattedText _fText;

		[DontSerialize]
		private GameObject _glyph;

		#endregion NonSerialized fields

		private object _checkedArgument;
		private bool _isChecked;
		private ContentRef<Script> _onChecked;
		private ContentRef<Script> _onUnchecked;
		private string _text;
		private ColorRgba _textColor;
		private ContentRef<Font> _textFont;
		private object _uncheckedArgument;
		private Alignment _glyphLocation;
		private string _customGlyphAppearance;
		private Vector2 _glyphSize;

		/// <summary>
		/// [GET / SET] The size of the Glyph
		/// </summary>
		public Vector2 GlyphSize
		{
			get { return _glyphSize; }
			set { _glyphSize = value; }
		}

		/// <summary>
		/// [GET / SET] The custom appearance for the Glyph
		/// </summary>
		public string CustomGlyphAppearance
		{
			get { return _customGlyphAppearance; }
			set
			{
				_customGlyphAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CheckButton()
		{
			ActiveArea = ActiveArea.All;

			_fText = new FormattedText();
			_textColor = Colors.White;
			_glyphLocation = Alignment.Left;
		}

		/// <summary>
		///
		/// </summary>
		public object CheckedArgument
		{
			get { return _checkedArgument; }
			set { _checkedArgument = value; }
		}

		/// <summary>
		/// [GET / SET] if the Button is Checked
		/// </summary>
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				if (_isChecked != value)
				{
					_isChecked = value;
					_dirtyFlags |= DirtyFlags.Value;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public ContentRef<Script> OnChecked
		{
			get { return _onChecked; }
			set { _onChecked = value; }
		}

		/// <summary>
		///
		/// </summary>
		public ContentRef<Script> OnUnchecked
		{
			get { return _onUnchecked; }
			set { _onUnchecked = value; }
		}

		/// <summary>
		/// [GET / SET] the Text of the Button
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		/// <summary>
		/// [GET / SET] the Location of the Check Glyph
		/// </summary>
		public Alignment GlyphLocation
		{
			get { return _glyphLocation; }
			set { _glyphLocation = value; }
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
		///
		/// </summary>
		public object UncheckedArgument
		{
			get { return _uncheckedArgument; }
			set { _uncheckedArgument = value; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		public override void MouseDown(MouseButtonEventArgs e)
		{
			if (Status != WidgetStatus.Disabled)
			{
				if (e.Button == MouseButton.Left)
				{
					Status = WidgetStatus.Active;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public override void MouseEnter()
		{
			_isMouseOver = true;

			if (Status != WidgetStatus.Disabled)
			{
				Status = WidgetStatus.Hover;
			}
		}

		/// <summary>
		///
		/// </summary>
		public override void MouseLeave()
		{
			_isMouseOver = false;

			if (Status != WidgetStatus.Disabled)
			{
				Status = WidgetStatus.Normal;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		public override void MouseUp(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left && _isMouseOver)
			{
				IsChecked = !IsChecked;
				Status = _isMouseOver ? WidgetStatus.Hover : WidgetStatus.Normal;

				if (IsChecked && OnChecked.Res != null)
				{
					OnChecked.Res.Execute(this.GameObj, CheckedArgument);
				}
				if (!IsChecked && OnUnchecked.Res != null)
				{
					OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
				}
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
				if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
				{
					_fText.Fonts[0] = _textFont;
				}

				_fText.SourceText = _text;
				Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;

				inCanvas.PushState();

				inCanvas.State.ColorTint = _textColor;
				inCanvas.State.TransformAngle = GameObj.Transform.Angle;
				inCanvas.DrawText(_fText, buttonCenter.X, buttonCenter.Y, buttonCenter.Z + DELTA_Z, null, Alignment.Center);

				inCanvas.PopState();
			}
		}

		/// <summary>
		///
		/// </summary>
		protected override void OnStatusChange()
		{
			base.OnStatusChange();

			if (_glyph != null)
			{
				_glyph.GetComponent<Panel>().Status = (Status == WidgetStatus.Disabled ? WidgetStatus.Disabled : WidgetStatus.Normal);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inSecondsPast"></param>
		protected override void OnUpdate(float inSecondsPast)
		{
			if (_glyph == null)
			{
				AddGlyph();
			}

			if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
			{
				OnCheckUncheck();
			}

			if (_glyph != null)
			{
				if ((_dirtyFlags & DirtyFlags.Skin) != DirtyFlags.None)
				{
					_glyph.GetComponent<Panel>().Skin = Skin;
					_glyph.GetComponent<Panel>().CustomWidgetAppearance = _customGlyphAppearance;
				}

				_glyph.Active = IsChecked;
			}
		}

		private void AddGlyph()
		{
			_glyph = new GameObject(this.GetType().Name + "Glyph", this.GameObj);

			Panel p = new Panel();
			p.VisibilityGroup = VisibilityGroup;
			p.Skin = Skin;
			p.CustomWidgetAppearance = _customGlyphAppearance;
			p.Rect = Rect.Align(GlyphLocation, 0, 0, _glyphSize.X, _glyphSize.Y);

			_glyph.AddComponent<Panel>(p);

			Transform t = _glyph.AddComponent<Transform>();
			t.RelativeAngle = 0;

			switch (GlyphLocation)
			{
				case Alignment.TopLeft:
					t.RelativePos = new Vector3(0, 0, 0);
					break;

				case Alignment.Top:
					t.RelativePos = new Vector3(Rect.CenterX, 0, 0);
					break;

				case Alignment.TopRight:
					t.RelativePos = new Vector3(Rect.W, 0, 0);
					break;

				case Alignment.Left:
					t.RelativePos = new Vector3(0, Rect.CenterY, 0);
					break;

				case Alignment.Center:
					t.RelativePos = new Vector3(Rect.CenterX, Rect.CenterY, 0);
					break;

				case Alignment.Right:
					t.RelativePos = new Vector3(Rect.W, Rect.CenterY, 0);
					break;

				case Alignment.BottomLeft:
					t.RelativePos = new Vector3(0, Rect.H, 0);
					break;

				case Alignment.Bottom:
					t.RelativePos = new Vector3(Rect.CenterX, Rect.H, 0);
					break;

				case Alignment.BottomRight:
					t.RelativePos = new Vector3(Rect.W, Rect.H, 0);
					break;
			}

			this.GameObj.ParentScene.AddObject(_glyph);
		}

		private void OnCheckUncheck()
		{
			if (IsChecked)
			{
				if (OnChecked.Res != null)
				{
					OnChecked.Res.Execute(this.GameObj, UncheckedArgument);
				}
			}
			else
			{
				if (OnUnchecked.Res != null)
				{
					OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
				}
			}
		}
	}
}