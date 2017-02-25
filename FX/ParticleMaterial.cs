// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.FX
{
    internal class ParticleMaterial
    {
        private Pixmap _internalPixmap;

        internal ParticleMaterial(ContentRef<Material> inMaterial)
        {
            Material = inMaterial;
            Texture mainTex = inMaterial.Res.MainTexture.Res;
            _internalPixmap = mainTex.BasePixmap.Res;

            Center = mainTex.Size / 2f;
            Rectangle = Rect.Align(Alignment.TopLeft, 0, 0, mainTex.Size.X, mainTex.Size.Y);
        }

        public Vector2 Center { get; private set; }
        public ContentRef<Material> Material { get; private set; }
        public Rect Rectangle { get; private set; }

        public Rect GetFrame()
        {
            Rect result = Rectangle;

            if (_internalPixmap.AnimFrames > 0)
            {
                result = _internalPixmap.LookupAtlas(MathF.Rnd.Next(_internalPixmap.AnimFrames));
            }

            return result;
        }
    }
}