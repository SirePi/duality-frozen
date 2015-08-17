// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageSkin)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class GradientSkin : Skin
    {
        public new static string FileExt = ".GradientSkin" + Resource.FileExt;

        private ColorRgba _colorTL;
        private ColorRgba _colorTR;
        private ColorRgba _colorBL;
        private ColorRgba _colorBR;

        public ColorRgba ColorTopLeft
        {
            get { return _colorTL; }
            set { _colorTL = value; }
        }

        public ColorRgba ColorTopRight
        {
            get { return _colorTR; }
            set { _colorTR = value; }
        }

        public ColorRgba ColorBottomLeft
        {
            get { return _colorBL; }
            set { _colorBL = value; }
        }

        public ColorRgba ColorBottomRight
        {
            get { return _colorBR; }
            set { _colorBR = value; }
        }

        protected override void UVAndColor(ref Core.MultiSpacePoint[] vertices, Vector4 border)
        {
            Vector2 topLeft = vertices[0].SceneCoords.Xy;
            Vector2 size = vertices[15].SceneCoords.Xy - topLeft;

            vertices[0].UVCoords = Vector2.Zero;
            vertices[0].Tint = _colorTL;
            vertices[1].UVCoords = (vertices[1].SceneCoords.Xy - topLeft) / size;
            vertices[1].Tint = TriangularInterpolation(ref vertices, vertices[1].UVCoords);
            vertices[2].UVCoords = (vertices[2].SceneCoords.Xy - topLeft) / size;
            vertices[2].Tint = TriangularInterpolation(ref vertices, vertices[2].UVCoords); ;
            vertices[3].UVCoords = Vector2.UnitX;
            vertices[3].Tint = _colorTR;
            vertices[4].UVCoords = (vertices[4].SceneCoords.Xy - topLeft) / size;
            vertices[4].Tint = TriangularInterpolation(ref vertices, vertices[4].UVCoords); ;
            vertices[5].UVCoords = (vertices[5].SceneCoords.Xy - topLeft) / size;
            vertices[5].Tint = TriangularInterpolation(ref vertices, vertices[5].UVCoords); ;
            vertices[6].UVCoords = (vertices[6].SceneCoords.Xy - topLeft) / size;
            vertices[6].Tint = TriangularInterpolation(ref vertices, vertices[6].UVCoords); ;
            vertices[7].UVCoords = (vertices[7].SceneCoords.Xy - topLeft) / size;
            vertices[7].Tint = TriangularInterpolation(ref vertices, vertices[7].UVCoords); ;
            vertices[8].UVCoords = (vertices[8].SceneCoords.Xy - topLeft) / size;
            vertices[8].Tint = TriangularInterpolation(ref vertices, vertices[8].UVCoords); ;
            vertices[9].UVCoords = (vertices[9].SceneCoords.Xy - topLeft) / size;
            vertices[9].Tint = TriangularInterpolation(ref vertices, vertices[9].UVCoords); ;
            vertices[10].UVCoords = (vertices[10].SceneCoords.Xy - topLeft) / size;
            vertices[10].Tint = TriangularInterpolation(ref vertices, vertices[10].UVCoords); ;
            vertices[11].UVCoords = (vertices[11].SceneCoords.Xy - topLeft) / size;
            vertices[11].Tint = TriangularInterpolation(ref vertices, vertices[11].UVCoords); ;
            vertices[12].UVCoords = Vector2.UnitY;
            vertices[12].Tint = _colorBL;
            vertices[13].UVCoords = (vertices[13].SceneCoords.Xy - topLeft) / size;
            vertices[13].Tint = TriangularInterpolation(ref vertices, vertices[13].UVCoords); ;
            vertices[14].UVCoords = (vertices[14].SceneCoords.Xy - topLeft) / size;
            vertices[14].Tint = TriangularInterpolation(ref vertices, vertices[14].UVCoords); ;
            vertices[15].UVCoords = Vector2.One;
            vertices[15].Tint = _colorBR;
        }

        private ColorRgba TriangularInterpolation(ref MultiSpacePoint[] vertices, Vector2 uvPosition)
        {
            // opengl draws quads with 2 triangles like this:
            // +------+
            // |     /|
            // | 1  / |
            // |   /  |
            // |  /   |
            // | /  2 |
            // |/     |
            // +------+

            ColorRgba result = ColorRgba.White;

            if (uvPosition.X + uvPosition.Y <= 1) // first triangle
            {
                result = ThreePointInterpolation(vertices[0].UVCoords, _colorTL, vertices[3].UVCoords, _colorTR, vertices[12].UVCoords, _colorBL, uvPosition);
            }
            else
            {
                result = ThreePointInterpolation(vertices[3].UVCoords, _colorTR, vertices[15].UVCoords, _colorBR, vertices[12].UVCoords, _colorBL, uvPosition);
            }

            return result;
        }

        private ColorRgba ThreePointInterpolation(Vector2 p0, ColorRgba c0, Vector2 p1, ColorRgba c1, Vector2 p2, ColorRgba c2, Vector2 p)
        {
            // calculate barycentric coordinates of point p
            // http://jsfiddle.net/PerroAZUL/zdaY8/1/
            float A = 1 / 2 * (-p1.Y * p2.X + p0.Y * (-p1.X + p2.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y);
            float sign = A < 0 ? -1 : 1;

            float l1 = (p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y) * sign;
            float l2 = (p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y) * sign;
            float l0 = 1 - l1 - l2;

            Vector4 color = (c0.ToVector4() * l0) + (c1.ToVector4() * l1) + (c2.ToVector4() * l2);

            return Colors.FromBase255Vector4(color);
        }
    }
}