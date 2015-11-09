using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    internal class DefaultGradientSkin
    {
        internal static readonly GradientSkin NORMAL = new GradientSkin()
        {
            ColorTopLeft = ColorRgba.VeryLightGrey,
            ColorTopRight = ColorRgba.VeryLightGrey,
            ColorBottomLeft = ColorRgba.DarkGrey,
			ColorBottomRight = ColorRgba.DarkGrey
        };

        internal static readonly GradientSkin HOVER = new GradientSkin()
        {
            ColorTopLeft = ColorRgba.White,
            ColorTopRight = ColorRgba.White,
            ColorBottomLeft = ColorRgba.Grey,
			ColorBottomRight = ColorRgba.Grey
        };

        internal static readonly GradientSkin ACTIVE = new GradientSkin()
        {
            ColorTopLeft = ColorRgba.Red,
            ColorTopRight = ColorRgba.Red,
            ColorBottomLeft = ColorRgba.White,
            ColorBottomRight = ColorRgba.White
        };

        internal static readonly GradientSkin DISABLED = new GradientSkin()
        {
            ColorTopLeft = ColorRgba.DarkGrey,
            ColorTopRight = ColorRgba.DarkGrey,
            ColorBottomLeft = ColorRgba.Grey,
            ColorBottomRight = ColorRgba.Grey
        };

        internal static readonly SolidColorSkin HIGHLIGHT_NORMAL = new SolidColorSkin {Color = ColorRgba.Blue};
        internal static readonly SolidColorSkin HIGHLIGHT_DISABLED = new SolidColorSkin { Color = ColorRgba.Grey };

        internal static readonly Appearance DEFAULT = new Appearance()
            {
                Border = Vector4.Zero,
                Normal = NORMAL,
                Hover = HOVER,
                Active = ACTIVE,
                Disabled = DISABLED
            };

        internal static readonly Appearance HIGHLIGHT = new Appearance()
        {
            Border = Vector4.Zero,
            Normal = HIGHLIGHT_NORMAL,
            Hover = HIGHLIGHT_NORMAL,
            Active = HIGHLIGHT_NORMAL,
            Disabled = HIGHLIGHT_DISABLED
        };

        internal static readonly WidgetAppearance WIDGET = new WidgetAppearance()
        {
            Widget = DEFAULT
        };

        internal static readonly GlyphAppearance GLYPH = new GlyphAppearance()
        {
            Widget = DEFAULT,
            Glyph = HIGHLIGHT,
            GlyphSize = new Vector2(16, 16)
        };

        internal static readonly ScrollBarAppearance SCROLLBAR = new ScrollBarAppearance()
        {
            Widget = DEFAULT,
            Increase = DEFAULT,
            Decrease = DEFAULT,
            Cursor = HIGHLIGHT,
            ButtonSize = new Vector2(24, 24),
            CursorSize = new Vector2(16, 16)
        };

        internal static readonly ListBoxAppearance LISTBOX = new ListBoxAppearance()
        {
            Widget = DEFAULT,
            Highlight = HIGHLIGHT,
            ScrollBar = SCROLLBAR
        };

        internal static readonly DropDownAppearance DROPDOWN = new DropDownAppearance()
        {
            Widget = DEFAULT,
            ListBox = LISTBOX
        };

        internal static readonly MultiLineAppearance MULTILINE = new MultiLineAppearance()
        {
            Widget = DEFAULT,
            ScrollBar = SCROLLBAR
        };

        internal static readonly ProgressBarAppearance PROGRESSBAR = new ProgressBarAppearance()
        {
            Widget = DEFAULT,
            ProgressBar = HIGHLIGHT
        };

        internal static readonly WindowAppearance WINDOW = new WindowAppearance()
        {
            Widget = DEFAULT,
            Maximize = DEFAULT,
            Minimize = DEFAULT,
            Close = DEFAULT,
            Restore = DEFAULT,
            ButtonSize = new Vector2(16, 16),
            CloseButtonSize = new Vector2(24, 24)
        };
    }
}
