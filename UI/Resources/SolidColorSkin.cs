// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [EditorHintImage(ResNames.ImageSkin)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class SolidColorSkin : Skin
    {
        public static readonly SolidColorSkin WHITE = new SolidColorSkin() { Color = ColorRgba.White };
        public static readonly SolidColorSkin LIGHT_GREY = new SolidColorSkin() { Color = ColorRgba.LightGrey };
        public static readonly SolidColorSkin DARK_GREY = new SolidColorSkin() { Color = ColorRgba.DarkGrey };
        public static readonly SolidColorSkin RED = new SolidColorSkin() { Color = ColorRgba.Red };

        private ColorRgba _color;

        public ColorRgba Color
        {
            get { return _color; }
            set { _color = value; }
        }

        protected override void UVAndColor(ref Core.MultiSpacePoint[] vertices, Vector4 border)
        {
            Vector2 topLeft = vertices[0].SceneCoords.Xy;
            Vector2 size = vertices[15].SceneCoords.Xy - topLeft;

            vertices[0].UVCoords = Vector2.Zero;
            vertices[0].Tint = _color;
            vertices[1].UVCoords = (vertices[1].SceneCoords.Xy - topLeft) / size;
            vertices[1].Tint = _color;
            vertices[2].UVCoords = (vertices[2].SceneCoords.Xy - topLeft) / size;
            vertices[2].Tint = _color;
            vertices[3].UVCoords = Vector2.UnitX;
            vertices[3].Tint = _color;
            vertices[4].UVCoords = (vertices[4].SceneCoords.Xy - topLeft) / size;
            vertices[4].Tint = _color;
            vertices[5].UVCoords = (vertices[5].SceneCoords.Xy - topLeft) / size;
            vertices[5].Tint = _color;
            vertices[6].UVCoords = (vertices[6].SceneCoords.Xy - topLeft) / size;
            vertices[6].Tint = _color;
            vertices[7].UVCoords = (vertices[7].SceneCoords.Xy - topLeft) / size;
            vertices[7].Tint = _color;
            vertices[8].UVCoords = (vertices[8].SceneCoords.Xy - topLeft) / size;
            vertices[8].Tint = _color;
            vertices[9].UVCoords = (vertices[9].SceneCoords.Xy - topLeft) / size;
            vertices[9].Tint = _color;
            vertices[10].UVCoords = (vertices[10].SceneCoords.Xy - topLeft) / size;
            vertices[10].Tint = _color;
            vertices[11].UVCoords = (vertices[11].SceneCoords.Xy - topLeft) / size;
            vertices[11].Tint = _color;
            vertices[12].UVCoords = Vector2.UnitY;
            vertices[12].Tint = _color;
            vertices[13].UVCoords = (vertices[13].SceneCoords.Xy - topLeft) / size;
            vertices[13].Tint = _color;
            vertices[14].UVCoords = (vertices[14].SceneCoords.Xy - topLeft) / size;
            vertices[14].Tint = _color;
            vertices[15].UVCoords = Vector2.One;
            vertices[15].Tint = _color;
        }
    }
}