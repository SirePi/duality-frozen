/*
 * A set of static helper classes that provide easy runtime access to the games resources.
 * This file is auto-generated. Any changes made to it are lost as soon as Duality decides
 * to regenerate it.
 */
namespace GameRes
{
	public static class Data {
		public static class Graphics {
			public static class Skins {
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> Panel_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\Panel.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> Progress_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\Progress.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> ProgressBar_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\ProgressBar.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> RadioButton_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\RadioButton.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> RadioGlyph_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\RadioGlyph.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> ScrollBar_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\ScrollBar.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> ScrollBarCursor_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\ScrollBarCursor.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> ScrollBarDecrease_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\ScrollBarDecrease.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> ScrollBarIncrease_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\ScrollBarIncrease.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> Window_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\Window.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> WindowCloseBtn_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\WindowCloseBtn.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> WindowMaximizeBtn_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\WindowMaximizeBtn.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> WindowMinimizeBtn_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\WindowMinimizeBtn.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> WindowRestoreBtn_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\WindowRestoreBtn.WidgetSkin.res"); }}
				public static Duality.ContentRef<FrozenCore.Resources.Widgets.WidgetSkin> YellowButton_WidgetSkin { get { return Duality.ContentProvider.RequestContent<FrozenCore.Resources.Widgets.WidgetSkin>(@"Data\Graphics\Skins\YellowButton.WidgetSkin.res"); }}
				public static void LoadAll() {
					Panel_WidgetSkin.MakeAvailable();
					Progress_WidgetSkin.MakeAvailable();
					ProgressBar_WidgetSkin.MakeAvailable();
					RadioButton_WidgetSkin.MakeAvailable();
					RadioGlyph_WidgetSkin.MakeAvailable();
					ScrollBar_WidgetSkin.MakeAvailable();
					ScrollBarCursor_WidgetSkin.MakeAvailable();
					ScrollBarDecrease_WidgetSkin.MakeAvailable();
					ScrollBarIncrease_WidgetSkin.MakeAvailable();
					Window_WidgetSkin.MakeAvailable();
					WindowCloseBtn_WidgetSkin.MakeAvailable();
					WindowMaximizeBtn_WidgetSkin.MakeAvailable();
					WindowMinimizeBtn_WidgetSkin.MakeAvailable();
					WindowRestoreBtn_WidgetSkin.MakeAvailable();
					YellowButton_WidgetSkin.MakeAvailable();
				}
			}
			public static Duality.ContentRef<Duality.Resources.Material> frozenSkin_Material { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Material>(@"Data\Graphics\frozenSkin.Material.res"); }}
			public static Duality.ContentRef<Duality.Resources.Pixmap> frozenSkin_Pixmap { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Pixmap>(@"Data\Graphics\frozenSkin.Pixmap.res"); }}
			public static Duality.ContentRef<Duality.Resources.Texture> frozenSkin_Texture { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Texture>(@"Data\Graphics\frozenSkin.Texture.res"); }}
			public static void LoadAll() {
				Skins.LoadAll();
				frozenSkin_Material.MakeAvailable();
				frozenSkin_Pixmap.MakeAvailable();
				frozenSkin_Texture.MakeAvailable();
			}
		}
		public static class Scenes {
			public static Duality.ContentRef<Duality.Resources.Scene> UI_Scene { get { return Duality.ContentProvider.RequestContent<Duality.Resources.Scene>(@"Data\Scenes\UI.Scene.res"); }}
			public static void LoadAll() {
				UI_Scene.MakeAvailable();
			}
		}
		public static class Scripts {
			public static Duality.ContentRef<FrozenCoreSamples.Scripts.ProgressIncrease> ProgressIncrease_ProgressIncrease { get { return Duality.ContentProvider.RequestContent<FrozenCoreSamples.Scripts.ProgressIncrease>(@"Data\Scripts\ProgressIncrease.ProgressIncrease.res"); }}
			public static void LoadAll() {
				ProgressIncrease_ProgressIncrease.MakeAvailable();
			}
		}
		public static void LoadAll() {
			Graphics.LoadAll();
			Scenes.LoadAll();
			Scripts.LoadAll();
		}
	}

}
