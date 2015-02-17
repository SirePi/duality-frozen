using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace TestApp
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            CellNoise b = new CellNoise(20, false);
            b.Seed = 1;
            b.SetDistanceFunction(SnowyPeak.Duality.Plugin.Frozen.Procedural.Minkowski.Instance);

            Duality.DualityApp.Init(Duality.DualityApp.ExecutionEnvironment.Launcher, Duality.DualityApp.ExecutionContext.Game);
            /*
            WidgetSkin ws = new WidgetSkin()
            {
                Material = new Material(
                    DrawTechnique.Mask,
                    Colors.White,
                    new Texture(new Pixmap(b.ToBitmap(512, 512, new ColorRange(Colors.White, Colors.Red))))
                    )
            };
            */

            System.Drawing.Image bmp = System.Drawing.Bitmap.FromFile("bgSprites.png");
            Application.Run(new SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms.SkinEditor(bmp));
        }
    }
}
