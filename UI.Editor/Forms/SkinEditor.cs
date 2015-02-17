using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using OpenTK;

namespace SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms
{
    public partial class SkinEditor : Form
    {
        private Point _zoomLocation;
        private int _scaledSize;
        private Brush _rectangleBrush;

        public WidgetSkin ModifiedSkin { get; private set; }

        public SkinEditor(WidgetSkin inSkin)
        {
            InitializeComponent();

            ModifiedSkin = inSkin;
            picOriginal.Image = ModifiedSkin.Material.Res.MainTexture.Res.BasePixmap.Res.MainLayer.ToBitmap();
        }

        public SkinEditor(Image inSkin)
        {
            InitializeComponent();

            ModifiedSkin = new WidgetSkin();
            picOriginal.Image = inSkin;
        }

        private void picZoom_Paint(object sender, PaintEventArgs e)
        {
            Rectangle src = picZoom.ClientRectangle;
            src.Offset(_zoomLocation);

            src.Width = _scaledSize;
            src.Height = _scaledSize;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            e.Graphics.DrawImage(picOriginal.Image, picZoom.ClientRectangle, src, GraphicsUnit.Pixel);
        }

        private void zoom_ValueChanged(object sender, EventArgs e)
        {
            _scaledSize = picZoom.ClientRectangle.Width / zoom.Value;
            picOriginal_MouseClick(null, null);
        }

        private void picOriginal_MouseClick(object sender, MouseEventArgs e)
        {
            if (e != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _zoomLocation.X = e.X;
                _zoomLocation.Y = e.Y;
            }

            if (_zoomLocation.X + _scaledSize > picOriginal.Image.Width)
            {
                _zoomLocation.X = picOriginal.Image.Width - _scaledSize;
            }
            if (_zoomLocation.Y + _scaledSize > picOriginal.Image.Height)
            {
                _zoomLocation.Y = picOriginal.Image.Height - _scaledSize;
            }

            picOriginal.Invalidate();
            picZoom.Invalidate();
        }

        private void picOriginal_Paint(object sender, PaintEventArgs e)
        {
            DrawLines(e.Graphics, Pens.Gray, ModifiedSkin.Origin.Disabled, picOriginal.Image.Size);
            DrawLines(e.Graphics, Pens.Yellow, ModifiedSkin.Origin.Active, picOriginal.Image.Size);
            DrawLines(e.Graphics, Pens.Cyan, ModifiedSkin.Origin.Hover, picOriginal.Image.Size);
            DrawLines(e.Graphics, Pens.Red, ModifiedSkin.Origin.Normal, picOriginal.Image.Size);

            e.Graphics.FillRectangle(_rectangleBrush, _zoomLocation.X, _zoomLocation.Y, _scaledSize, _scaledSize);
            e.Graphics.DrawRectangle(Pens.AliceBlue, _zoomLocation.X, _zoomLocation.Y, _scaledSize, _scaledSize);
        }

        private void DrawLines(Graphics graphics, Pen pen, Vector2 point, Size limits)
        {
            graphics.DrawLine(pen, 0, point.Y, limits.Width, point.Y);
            graphics.DrawLine(pen, point.X, 0, point.X, limits.Height);
        }

        private void FillRectangle(Graphics graphics)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void picZoom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SkinOrigin origin = ModifiedSkin.Origin;
                Vector4 border = ModifiedSkin.Border;

                Vector2 point = new Vector2((e.X / zoom.Value) + _zoomLocation.X, (e.Y / zoom.Value) + _zoomLocation.Y);

                if (radRightBottom.Checked)
                {
                    Vector2 newPoint = point - ModifiedSkin.Origin.Normal;
                    if (newPoint.X >= 0 && newPoint.Y >= 0)
                    {
                        v2eRightBottom.Value = newPoint;
                    }
                }
                else if (radNormal.Checked)
                {
                    v2eNormal.Value = point;
                }
                else if (radHover.Checked)
                {
                    v2eHover.Value = point;
                }
                else if (radActive.Checked)
                {
                    v2eActive.Value = point;
                }
                else if (radDisabled.Checked)
                {
                    v2eDisabled.Value = point;
                }
                else if (radBorderTL.Checked)
                {
                    v2eBorderTL.Value = point;
                }
                else if (radBorderBR.Checked)
                {
                    v2eBorderBR.Value = point;
                }

                ApplyValues();
                picOriginal.Invalidate();
                picZoom.Invalidate();
            }
        }

        private void ApplyValues()
        {
            SkinOrigin origin = ModifiedSkin.Origin;
            Vector4 border = ModifiedSkin.Border;
            
            origin.Normal = v2eNormal.Value;
            origin.Hover = v2eHover.Value;
            origin.Active = v2eActive.Value;
            origin.Disabled = v2eDisabled.Value;
            border.X = v2eBorderTL.Value.X;
            border.Y = v2eBorderTL.Value.Y;
            border.W = v2eBorderBR.Value.X;
            border.Z = v2eBorderBR.Value.Y;

            ModifiedSkin.Size = v2eRightBottom.Value - v2eNormal.Value;
            ModifiedSkin.Origin = origin;
            ModifiedSkin.Border = border;
        }

        private void SkinEditor_Load(object sender, EventArgs e)
        {
            _zoomLocation = Point.Empty;
            _rectangleBrush = new SolidBrush(Color.FromArgb(128, Color.AliceBlue));

            Vector2 maxSize = new Vector2(picOriginal.Image.Size.Width, picOriginal.Image.Size.Height);

            v2eRightBottom.MaxValue = maxSize;
            v2eNormal.MaxValue = maxSize;
            v2eHover.MaxValue = maxSize;
            v2eActive.MaxValue = maxSize;
            v2eDisabled.MaxValue = maxSize;
            v2eBorderTL.MaxValue = maxSize;
            v2eBorderBR.MaxValue = maxSize;

            picOriginal_MouseClick(null, null);
            zoom_ValueChanged(null, null);
        }
    }
}
