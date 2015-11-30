// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SnowyPeak.Duality.Plugin.Frozen.UI
{
	/// <summary>
	/// Defines a Duality core plugin.
	/// </summary>
	public class FrozenUIPlugin : CorePlugin
	{
		public static readonly string FROZEN_UI_PIXMAP = @"Frozen\UI\FrozenUIPixmap";
		public static readonly string DEFAULT_SKIN = @"Frozen\UI\FrozenUISkin";

		/// <summary>
		/// 
		/// </summary>
		protected override void InitPlugin()
		{
			ContentProvider.ResourceResolve += ContentProvider_ResourceResolve;

			IImageCodec codec = ImageCodec.GetRead(ImageCodec.FormatPng);
			Assembly asm = this.GetType().GetTypeInfo().Assembly;

			Pixmap frozenUIPixmap;
			using (Stream stream = asm.GetManifestResourceStream(ResNames.ManifestBaseName + "frozenUi.png"))
			{
				frozenUIPixmap = new Pixmap(codec.Read(stream));
			}

			frozenUIPixmap.Atlas = new System.Collections.Generic.List<Rect>();

			// Window - 0
			frozenUIPixmap.Atlas.Add(new Rect(0, 0, 64, 64));
			frozenUIPixmap.Atlas.Add(new Rect(64, 0, 64, 64));
			frozenUIPixmap.Atlas.Add(new Rect(128, 0, 64, 64));
			frozenUIPixmap.Atlas.Add(new Rect(192, 0, 64, 64));

			// Multiline - 4
			frozenUIPixmap.Atlas.Add(new Rect(0, 64, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 64, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(128, 64, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(192, 64, 64, 32));

			// Dropdown - 8
			frozenUIPixmap.Atlas.Add(new Rect(0, 96, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 96, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(128, 96, 64, 32));
			frozenUIPixmap.Atlas.Add(new Rect(192, 96, 64, 32));

			// Button - 12
			frozenUIPixmap.Atlas.Add(new Rect(0, 128, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(32, 128, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 128, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(96, 128, 32, 32));

			// CheckGlyph - 16
			frozenUIPixmap.Atlas.Add(new Rect(128, 128, 32, 32));

			// RadioGlyph - 17
			frozenUIPixmap.Atlas.Add(new Rect(160, 128, 32, 32));

			// Bar - 18
			frozenUIPixmap.Atlas.Add(new Rect(192, 128, 32, 32));

			// Highlight - 19
			frozenUIPixmap.Atlas.Add(new Rect(224, 128, 32, 32));

			// CloseButton - 20
			frozenUIPixmap.Atlas.Add(new Rect(0, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(32, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(96, 160, 32, 32));

			// MaximizeButton - 24
			frozenUIPixmap.Atlas.Add(new Rect(128, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(160, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(192, 160, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(224, 160, 32, 32));

			// MinimizeButton - 28
			frozenUIPixmap.Atlas.Add(new Rect(0, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(32, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(96, 192, 32, 32));

			// RestoreButton - 32
			frozenUIPixmap.Atlas.Add(new Rect(128, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(160, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(192, 192, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(224, 192, 32, 32));

			// ScrollMinus - 36
			frozenUIPixmap.Atlas.Add(new Rect(0, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(32, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(64, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(96, 224, 32, 32));

			// ScrollPlus - 40
			frozenUIPixmap.Atlas.Add(new Rect(128, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(160, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(192, 224, 32, 32));
			frozenUIPixmap.Atlas.Add(new Rect(224, 224, 32, 32));

			ContentProvider.AddContent(FROZEN_UI_PIXMAP, frozenUIPixmap);
		}

		protected override void OnDisposePlugin()
		{
			ContentProvider.ResourceResolve -= ContentProvider_ResourceResolve;
		}

		void ContentProvider_ResourceResolve(object sender, ResourceResolveEventArgs e)
		{
			if(e.RequestedContent == DEFAULT_SKIN)
			{
				if(ContentProvider.GetAvailableContent<Skin>().IndexOfFirst(s => s.Name == DEFAULT_SKIN) == -1)
				{
					ContentRef<Pixmap> frozenUIPixmap = ContentProvider.RequestContent<Pixmap>(FROZEN_UI_PIXMAP);
					Material frozenUIMaterial = new Material(DrawTechnique.Alpha, ColorRgba.White, new Texture(frozenUIPixmap));

					Skin frozenUISkin = new Skin()
					{
						Material = frozenUIMaterial,
						WidgetAppearances = new Dictionary<string, Appearance>()
					};

					frozenUISkin.WidgetAppearances.Add("Bar", new Appearance() { Border = new Vector4(3), Normal = 18, Hover = 18, Active = 18, Disabled = 18 });
					frozenUISkin.WidgetAppearances.Add("Button", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("CheckButton", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("CheckButtonGlyph", new Appearance() { Border = Vector4.Zero, Normal = 16, Hover = 16, Active = 16, Disabled = 16 });
					frozenUISkin.WidgetAppearances.Add("CommandGrid", new Appearance() { Border = new Vector4(3, 3, 3, 29), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("DropDownButton", new Appearance() { Border = new Vector4(3, 3, 31, 3), Normal = 8, Hover = 9, Active = 10, Disabled = 11 });
					frozenUISkin.WidgetAppearances.Add("Highlight", new Appearance() { Border = new Vector4(3), Normal = 19, Hover = 19, Active = 19, Disabled = 19 });
					frozenUISkin.WidgetAppearances.Add("ListBox", new Appearance() { Border = new Vector4(3, 3, 31, 3), Normal = 4, Hover = 5, Active = 6, Disabled = 7 });
					frozenUISkin.WidgetAppearances.Add("Panel", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("ProgressBar", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("RadioButton", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("RadioButtonGlyph", new Appearance() { Border = Vector4.Zero, Normal = 17, Hover = 17, Active = 17, Disabled = 17 });
					frozenUISkin.WidgetAppearances.Add("ScrollBar", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("ScrollBarCursor", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("ScrollBarDecreaseButton", new Appearance() { Border = new Vector4(3), Normal = 36, Hover = 37, Active = 38, Disabled = 39 });
					frozenUISkin.WidgetAppearances.Add("ScrollBarIncreaseButton", new Appearance() { Border = new Vector4(3), Normal = 40, Hover = 41, Active = 42, Disabled = 43 });
					frozenUISkin.WidgetAppearances.Add("TextBlock", new Appearance() { Border = new Vector4(3, 3, 31, 3), Normal = 4, Hover = 5, Active = 6, Disabled = 7 });
					frozenUISkin.WidgetAppearances.Add("TextBox", new Appearance() { Border = new Vector4(3), Normal = 12, Hover = 13, Active = 14, Disabled = 15 });
					frozenUISkin.WidgetAppearances.Add("Window", new Appearance() { Border = new Vector4(3, 33, 3, 3), Normal = 0, Hover = 1, Active = 2, Disabled = 3 });
					frozenUISkin.WidgetAppearances.Add("WindowCloseButton", new Appearance() { Border = new Vector4(3), Normal = 20, Hover = 21, Active = 22, Disabled = 23 });
					frozenUISkin.WidgetAppearances.Add("WindowMaximizeButton", new Appearance() { Border = new Vector4(3), Normal = 24, Hover = 25, Active = 26, Disabled = 27 });
					frozenUISkin.WidgetAppearances.Add("WindowMinimizeButton", new Appearance() { Border = new Vector4(3), Normal = 28, Hover = 29, Active = 30, Disabled = 31 });
					frozenUISkin.WidgetAppearances.Add("WindowRestoreButton", new Appearance() { Border = new Vector4(3), Normal = 32, Hover = 33, Active = 34, Disabled = 35 });

					ContentProvider.AddContent(DEFAULT_SKIN, frozenUISkin);
				}

				e.Resolve(ContentProvider.RequestContent<Skin>(DEFAULT_SKIN).Res);
			}
		}
	}
}