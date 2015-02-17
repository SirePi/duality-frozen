using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace SnowyPeak.Duality.Editor.Plugin.Frozen.UI.Forms
{
    public partial class Vector2Editor : UserControl
    {
        private Vector2 _value;
        private bool _updating;

        public Vector2 Value 
        {
            get { return _value; } 
            set 
            {
                _value = value;
                _updating = true;

                numX.Value = (int)_value.X;
                numY.Value = (int)_value.Y;

                _updating = false;
            }
        }

        public Vector2 MaxValue
        {
            set
            {
                numX.Maximum = (int)value.X;
                numY.Maximum = (int)value.Y;
            }
        }

        public Vector2Editor()
        {
            InitializeComponent();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                _value.X = (int)numX.Value;
                _value.Y = (int)numY.Value;
            }
        }
    }
}
