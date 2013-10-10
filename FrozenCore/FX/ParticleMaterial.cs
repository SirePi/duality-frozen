// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Resources;
using OpenTK;

namespace FrozenCore.FX
{
    internal class ParticleMaterial
    {
        public Vector2 Center { get; private set; }
        public ContentRef<Material> Material { get; private set; }
        public Rect Rectangle { get; private set; }

        internal ParticleMaterial(ContentRef<Material> inMaterial)
        {
            Material = inMaterial;
            Texture mainTex = inMaterial.Res.MainTexture.Res;

            Center = mainTex.Size / 2f;
            Rectangle = Rect.AlignCenter(0, 0, mainTex.Size.X, mainTex.Size.Y);
        }
    }
}