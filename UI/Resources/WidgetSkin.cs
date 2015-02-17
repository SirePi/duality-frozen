// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageSkin)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class WidgetSkin : Resource
    {
        public new static string FileExt = ".WidgetSkin" + Resource.FileExt;

        /// <summary>
        ///
        /// </summary>
        public static readonly ContentRef<WidgetSkin> DEFAULT = new ContentRef<WidgetSkin>(new WidgetSkin()
        {
            Material = global::Duality.Resources.Material.Checkerboard,
            Size = new Vector2(64, 64),
            Border = Vector4.Zero,
            Origin = new SkinOrigin()
            {
                Active = Vector2.Zero,
                Disabled = Vector2.Zero,
                Hover = Vector2.Zero,
                Normal = Vector2.Zero
            }
        });

        private Vector4 _border;
        private ContentRef<Material> _material;
        private SkinOrigin _origin;
        private Vector2 _size;
        /// <summary>
        ///
        /// </summary>
        public Vector4 Border
        {
            get { return _border; }
            set { _border = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public ContentRef<Material> Material
        {
            get { return _material; }
            set { _material = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public SkinOrigin Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}