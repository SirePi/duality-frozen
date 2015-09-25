using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Duality.Resources;
using System.IO;

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

            FileInfo fi = new FileInfo("html.txt");
            if(fi.Exists)
            {
                string html;

                using(StreamReader sr = fi.OpenText())
                {
                    html = sr.ReadToEnd();
                }

                //SnowyPeak.Duality.Plugin.Frozen.HTML.Parser p = new SnowyPeak.Duality.Plugin.Frozen.HTML.Parser();
                //SnowyPeak.Duality.Plugin.Frozen.HTML.Dom.Node n = p.ParseHTML(html);
            }

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap("bgSprites.png");
            //SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms.SkinEditor se = new SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms.SkinEditor(bmp);
            //Application.Run(se);
        }
    }
}
