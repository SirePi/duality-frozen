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
        public Vector2 Value 
        {
            get
            {
                return new Vector2((int)numX.Value, (int)numY.Value);
            }
            set 
            {
                if ((int)numX.Value != (int)value.X)
                {
                    numX.Value = (int)value.X;
                }
                if ((int)numY.Value != (int)value.Y)
                {
                    numY.Value = (int)value.Y;
                }
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
            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }
        }

        public delegate void ValueChangedHandler(object sender, EventArgs e);
        public event ValueChangedHandler ValueChanged;
    }
}
