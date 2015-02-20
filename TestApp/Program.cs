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
            Application.Run(new Form1());
        }
    }
}
