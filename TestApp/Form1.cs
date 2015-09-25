using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise;
using SnowyPeak.Duality.Plugin.Frozen.Procedural;
using OpenTK;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using Duality.Drawing;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private class Wonky : Distance
        {
            public override float GetDistance(Vector2 a, Vector2 b)
            {
                float dx = a.X - b.X;
                float dy = b.Y - a.Y;

                return dx + dy;
            }
        }

        private int _size = 256;
        private int _seed;

        private Noise _noise;
        private Bitmap _bmp;
        private Graph<DefaultNode> _graph;

        public Form1()
        {
            InitializeComponent();

            _seed = DateTime.Now.Millisecond;
            _seed = 1;

            Noise a = SimplexNoise.CLOUDS;
            Noise d = SimplexNoise.CLOUDS;
            
            CellNoise b = new CellNoise(20, false);
            b.Seed = 1;
            b.SetDistanceFunction(SnowyPeak.Duality.Plugin.Frozen.Procedural.Minkowski.Instance);
            b.SetDistanceFunction(new Wonky());

            CellNoise c = new CellNoise(1, true);
            c.SetDistanceFunction(SnowyPeak.Duality.Plugin.Frozen.Procedural.Chebychev.Instance);
            c.Sites = b.Sites;

            _graph = new Graph<DefaultNode>();
            foreach (Vector2 v in b.Sites)
            {
                _graph.Nodes.Add(new DefaultNode(v));
            }

            _noise = new Multiply(new Normalize(a), d);
            _noise = new Normalize(b);

            _noise = b;

            _noise = new Normalize(new SimplexNoise(8, .5f, 2, 1, 1));
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            _noise.Seed = _seed;

            this.BackgroundImage = null;
            /*
            ColorRgba[] colors = new ColorRgba[(_noise as CellNoise).Sites.Count];
            for (int i = 0; i < colors.Length; i++)
            {
                float value = 1f / (_noise as CellNoise).Sites.Count * i;
                colors[i] = new ColorRgba(value, value, value, 1f);
            }

            _bmp = (_noise as CellNoise).SitesToBitmap(150, 150, colors);
            */
            /*
            _bmp = new Bitmap(_size, _size);
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    int value = (int)(_noise.NoiseMap[x][y] * 255);
                    Color c = Color.FromArgb(255, value, value, value);
                    _bmp.SetPixel(x, y, c);
                }
            }
            *

            _graph.Triangulate();

            using (Graphics g = Graphics.FromImage(_bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                int i = 0;
                foreach (Link<INode> l in _graph.Links)
                {
                    i++;

                    Vector2 from = l.From.Position * _size;
                    Vector2 to = l.To.Position * _size;
                    Vector2 half = (from + to) / 2;

                    g.DrawLine(Pens.Red, from.X, from.Y, to.X, to.Y);
                    g.DrawString(i.ToString(), SystemFonts.DefaultFont, Brushes.LightBlue, half.X, half.Y);
                }
            }
            */

            _bmp = _noise.ToBitmap(_size, _size, new ColorRange(Colors.Red, Colors.Yellow));
            this.BackgroundImage = _bmp;
        }
    }
}
