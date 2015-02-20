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
        private Dictionary<Color, Color> _transparentColors;
        private Dictionary<Color, Brush> _brushes;
        private Dictionary<Color, Pen> _pens;

        private bool _loaded;
        private bool _mouseDown;
        private ColorDialog _cd = new ColorDialog() { AllowFullOpen = true, SolidColorOnly = false, FullOpen = true };

        private Point _zoomLocation;
        private int _scaledSize;

        public WidgetSkin ModifiedSkin { get; private set; }

        public SkinEditor(WidgetSkin inSkin)
        {
            InitializeComponent();

            ModifiedSkin = new WidgetSkin()
            {
                Border = inSkin.Border,
                Material = inSkin.Material,
                Origin = inSkin.Origin,
                Size = inSkin.Size
            };
        }

        private void picZoom_Paint(object sender, PaintEventArgs e)
        {
            Vector2 _zoomVector = new Vector2(_zoomLocation.X, _zoomLocation.Y);

            Rectangle src = picZoom.ClientRectangle;
            src.Offset(_zoomLocation);

            src.Width = _scaledSize;
            src.Height = _scaledSize;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            e.Graphics.DrawImage(picOriginal.Image, picZoom.ClientRectangle, src, GraphicsUnit.Pixel);

            DrawLines(e.Graphics, Color.MediumOrchid, (ModifiedSkin.Origin.Normal + ModifiedSkin.Border.Xy - _zoomVector) * zoom.Value, picZoom.Size);
            DrawLines(e.Graphics, Color.MediumOrchid, (ModifiedSkin.Origin.Normal + ModifiedSkin.Size - new Vector2(ModifiedSkin.Border.Z, ModifiedSkin.Border.W) - _zoomVector) * zoom.Value, picZoom.Size);

            DrawLines(e.Graphics, Color.DarkGray, (ModifiedSkin.Origin.Disabled - _zoomVector) * zoom.Value, picZoom.Size);
            DrawLines(e.Graphics, Color.LimeGreen, (ModifiedSkin.Origin.Active - _zoomVector) * zoom.Value, picZoom.Size);
            DrawLines(e.Graphics, Color.Gold, (ModifiedSkin.Origin.Hover - _zoomVector) * zoom.Value, picZoom.Size);
            DrawLines(e.Graphics, Color.OrangeRed, (ModifiedSkin.Origin.Normal - _zoomVector) * zoom.Value, picZoom.Size);
            DrawLines(e.Graphics, Color.Coral, (ModifiedSkin.Origin.Normal + ModifiedSkin.Size - _zoomVector) * zoom.Value, picZoom.Size);
        }

        private void zoom_ValueChanged(object sender, EventArgs e)
        {
            _scaledSize = picZoom.ClientRectangle.Width / zoom.Value;

            picOriginal.Invalidate();
            picZoom.Invalidate();
        }

        private void picOriginal_Paint(object sender, PaintEventArgs e)
        {
            if (_zoomLocation.X + _scaledSize > picOriginal.Image.Width)
            {
                _zoomLocation.X = picOriginal.Image.Width - _scaledSize;
            }
            if (_zoomLocation.Y + _scaledSize > picOriginal.Image.Height)
            {
                _zoomLocation.Y = picOriginal.Image.Height - _scaledSize;
            }
            if (_zoomLocation.X < 0)
            {
                _zoomLocation.X = 0;
            }
            if (_zoomLocation.Y < 0)
            {
                _zoomLocation.Y = 0;
            }

            FillRectangle(e.Graphics, Color.DarkGray, ModifiedSkin.Origin.Disabled, ModifiedSkin.Size);
            FillRectangle(e.Graphics, Color.LimeGreen, ModifiedSkin.Origin.Active, ModifiedSkin.Size);
            FillRectangle(e.Graphics, Color.Gold, ModifiedSkin.Origin.Hover, ModifiedSkin.Size);
            FillRectangle(e.Graphics, Color.OrangeRed, ModifiedSkin.Origin.Normal, ModifiedSkin.Size);

            FillRectangle(e.Graphics, Color.CornflowerBlue, _zoomLocation.X, _zoomLocation.Y, _scaledSize, _scaledSize);
        }

        private void DrawLines(Graphics graphics, Color color, Vector2 point, Size limits)
        {
            graphics.DrawLine(GetPen(color), 0, point.Y, limits.Width, point.Y);
            graphics.DrawLine(GetPen(color), point.X, 0, point.X, limits.Height);
        }

        private void FillRectangle(Graphics graphics, Color color, Vector2 location, Vector2 size)
        {
            FillRectangle(graphics, color, location.X, location.Y, size.X, size.Y);
        }

        private void FillRectangle(Graphics graphics, Color color, float x, float y, float w, float h)
        {
            if (!_transparentColors.ContainsKey(color))
            {
                _transparentColors[color] = Color.FromArgb(128, color);
            }

            graphics.FillRectangle(GetBrush(_transparentColors[color]), x, y, w, h);
            graphics.DrawRectangle(GetPen(color), x, y, w, h);
        }

        private Brush GetBrush(Color color)
        {
            if (!_brushes.ContainsKey(color))
            {
                _brushes[color] = new SolidBrush(color);
            }

            return _brushes[color];
        }

        private Pen GetPen(Color color)
        {
            if (!_pens.ContainsKey(color))
            {
                _pens[color] = new Pen(color, 1);
            }

            return _pens[color];
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
                        v2eSize.Value = newPoint;
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
                    v2eBorderTL.Value = point - ModifiedSkin.Origin.Normal;
                }
                else if (radBorderBR.Checked)
                {
                    v2eBorderBR.Value = ModifiedSkin.Origin.Normal + ModifiedSkin.Size - point;
                }

                ApplyValues();
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
            border.Z = v2eBorderBR.Value.X;
            border.W = v2eBorderBR.Value.Y;

            ModifiedSkin.Size = v2eSize.Value;
            ModifiedSkin.Origin = origin;
            ModifiedSkin.Border = border;
        }

        private void SkinEditor_Load(object sender, EventArgs e)
        {
            _transparentColors = new Dictionary<Color, Color>();
            _brushes = new Dictionary<Color, Brush>();
            _pens = new Dictionary<Color, Pen>();

            _zoomLocation = Point.Empty;

            picOriginal.Image = ModifiedSkin.Material.Res.MainTexture.Res.BasePixmap.Res.MainLayer.ToBitmap();
            Vector2 maxSize = new Vector2(picOriginal.Image.Size.Width, picOriginal.Image.Size.Height);

            v2eSize.MaxValue = maxSize;
            v2eNormal.MaxValue = maxSize;
            v2eHover.MaxValue = maxSize;
            v2eActive.MaxValue = maxSize;
            v2eDisabled.MaxValue = maxSize;
            v2eBorderTL.MaxValue = maxSize;
            v2eBorderBR.MaxValue = maxSize;

            v2eSize.Value = ModifiedSkin.Size;
            v2eNormal.Value = ModifiedSkin.Origin.Normal;
            v2eHover.Value = ModifiedSkin.Origin.Hover;
            v2eActive.Value = ModifiedSkin.Origin.Active;
            v2eDisabled.Value = ModifiedSkin.Origin.Disabled;
            v2eBorderTL.Value = new Vector2(ModifiedSkin.Border.X, ModifiedSkin.Border.Y);
            v2eBorderBR.Value = new Vector2(ModifiedSkin.Border.Z, ModifiedSkin.Border.W);

            picOriginal.Invalidate();
            picZoom.Invalidate();

            comboBox1.SelectedIndex = 0;
            zoom_ValueChanged(null, null);

            _loaded = true;
        }

        private void picOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                _zoomLocation.X = e.X - _scaledSize / 2;
                _zoomLocation.Y = e.Y - _scaledSize / 2;

                picOriginal.Invalidate();
                picZoom.Invalidate();
            }
        }

        private void picOriginal_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mouseDown = false;

                _zoomLocation.X = e.X - _scaledSize / 2;
                _zoomLocation.Y = e.Y - _scaledSize / 2;

                picOriginal.Invalidate();
                picZoom.Invalidate();
            }
        }

        private void picOriginal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mouseDown = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    picZoom.BackColor = picOriginal.BackColor = Color.Transparent;
                    break;

                case 1:
                    picZoom.BackColor = picOriginal.BackColor = Color.Black;
                    break;

                case 2:
                    picZoom.BackColor = picOriginal.BackColor = Color.White;
                    break;

                case 3:
                    if (_cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        picZoom.BackColor = picOriginal.BackColor = _cd.Color;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void v2_ValueChanged(object sender, EventArgs e)
        {
            if (_loaded)
            {
                ApplyValues();
                picOriginal.Invalidate();
                picZoom.Invalidate();
            }
        }
    }
}
