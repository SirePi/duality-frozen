// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.UI
{
    /// <summary>
    /// Defines a Duality core plugin.
    /// </summary>
    public class FrozenUIPlugin : CorePlugin
    {
        protected override void InitPlugin()
        {
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Normal", Resources.DefaultGradientSkin.NORMAL);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Hover", Resources.DefaultGradientSkin.HOVER);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Active", Resources.DefaultGradientSkin.ACTIVE);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Disabled", Resources.DefaultGradientSkin.DISABLED);

            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Highlight_Normal", Resources.DefaultGradientSkin.HIGHLIGHT_NORMAL);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Skin\Highlight_Disabled", Resources.DefaultGradientSkin.HIGHLIGHT_DISABLED);

            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\Default", Resources.DefaultGradientSkin.DEFAULT);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\DropDown", Resources.DefaultGradientSkin.DROPDOWN);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\Glyph", Resources.DefaultGradientSkin.GLYPH);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\Highlight", Resources.DefaultGradientSkin.HIGHLIGHT);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\ListBox", Resources.DefaultGradientSkin.LISTBOX);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\MultiLine", Resources.DefaultGradientSkin.MULTILINE);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\ProgressBar", Resources.DefaultGradientSkin.PROGRESSBAR);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\ScrollBar", Resources.DefaultGradientSkin.SCROLLBAR);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\Widget", Resources.DefaultGradientSkin.WIDGET);
            ContentProvider.AddContent(@"Frozen\UI\DefaultGradientSkin\Appearance\Window", Resources.DefaultGradientSkin.WINDOW);
        }
    }
}