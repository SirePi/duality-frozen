// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using Duality.Drawing;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [EditorHintImage(ResNames.ImageSkin)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class MaterialSkin : Skin
    {

        private int _atlasIndex;

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
        public int AtlasIndex
        {
            get { return _atlasIndex; }
            set { _atlasIndex = value; }
        }

        public MaterialSkin()
            : base()
        { }

        protected override void UVAndColor(ref Core.MultiSpacePoint[] vertices, Vector4 border)
        {
            Texture tx = _material.Res.MainTexture.Res;
            Rect txRect = tx.LookupAtlas(_atlasIndex);

            Vector2 txSize = tx.Size * txRect.Size;

            float borderLeft = border.X / txSize.X * txRect.W;
            float borderTop = border.Y / txSize.Y * txRect.H;
            float borderRight = txRect.W - (border.Z / txSize.X * txRect.W);
            float borderBottom = txRect.H - (border.W / txSize.Y * txRect.H);

            Vector2 topLeft = vertices[0].SceneCoords.Xy;
            Vector2 size = vertices[15].SceneCoords.Xy - topLeft;

            vertices[0].UVCoords = txRect.TopLeft + Vector2.Zero;
            vertices[0].Tint = ColorRgba.White;
            vertices[1].UVCoords = txRect.TopLeft + new Vector2(borderLeft, 0);
            vertices[1].Tint = ColorRgba.White;
            vertices[2].UVCoords = txRect.TopLeft + new Vector2(borderRight, 0);
            vertices[2].Tint = ColorRgba.White;
            vertices[3].UVCoords = txRect.TopLeft + (Vector2.UnitX * txRect.Size);
            vertices[3].Tint = ColorRgba.White;
            vertices[4].UVCoords = txRect.TopLeft + new Vector2(0, borderTop);
            vertices[4].Tint = ColorRgba.White;
            vertices[5].UVCoords = txRect.TopLeft + new Vector2(borderLeft, borderTop);
            vertices[5].Tint = ColorRgba.White;
            vertices[6].UVCoords = txRect.TopLeft + new Vector2(borderRight, borderTop);
            vertices[6].Tint = ColorRgba.White;
            vertices[7].UVCoords = txRect.TopLeft + new Vector2(txRect.Size.X, borderTop);
            vertices[7].Tint = ColorRgba.White;
            vertices[8].UVCoords = txRect.TopLeft + new Vector2(0, borderBottom);
            vertices[8].Tint = ColorRgba.White;
            vertices[9].UVCoords = txRect.TopLeft + new Vector2(borderLeft, borderBottom);
            vertices[9].Tint = ColorRgba.White;
            vertices[10].UVCoords = txRect.TopLeft + new Vector2(borderRight,borderBottom);
            vertices[10].Tint = ColorRgba.White;
            vertices[11].UVCoords = txRect.TopLeft + new Vector2(txRect.Size.X, borderBottom);
            vertices[11].Tint = ColorRgba.White;
            vertices[12].UVCoords = txRect.TopLeft + (Vector2.UnitY * txRect.Size);
            vertices[12].Tint = ColorRgba.White;
            vertices[13].UVCoords = txRect.TopLeft + new Vector2(borderLeft, txRect.Size.Y);
            vertices[13].Tint = ColorRgba.White;
            vertices[14].UVCoords = txRect.TopLeft + new Vector2(borderRight, txRect.Size.Y);
            vertices[14].Tint = ColorRgba.White;
            vertices[15].UVCoords = txRect.TopLeft + (Vector2.One * txRect.Size);
            vertices[15].Tint = ColorRgba.White;
        }
    }
}