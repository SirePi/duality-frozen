﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI
{
    public static class AppearanceManager
    {
        private static readonly string PREFIX = "__FCUIAM__";

        public static ContentRef<WidgetAppearance> RequestAppearanceContentRef(ContentRef<Appearance> app)
        {
            ContentRef<WidgetAppearance> contentRef;

            string path = PREFIX + app.Path;

            contentRef = ContentProvider.RequestContent<WidgetAppearance>(path);
            if (!contentRef.IsAvailable)
            {
                WidgetAppearance wapp = new WidgetAppearance() { Widget = app };
                contentRef = new ContentRef<WidgetAppearance>(wapp);

                ContentProvider.AddContent(path, wapp);
            }

            return contentRef;
        }
    }
}