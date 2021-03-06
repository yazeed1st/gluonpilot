﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Configuration
{
    public partial class DistanceTextBox : UserControl
    {
        private double current_distance_m;

        public event EventHandler DistanceChanged;

        [BrowsableAttribute(true)]
        public Boolean ReadOnly
        {
            set
            {
                tb_distance.ReadOnly = value;
            }
            get
            {
                return tb_distance.ReadOnly;
            }
        }

        [BrowsableAttribute(true)]
        public Color Color
        {
            set
            {
                tb_distance.BackColor = value;
            }
            get
            {
                return tb_distance.BackColor;
            }
        }

        private bool altitude_coloring = false;
        [BrowsableAttribute(true)]
        public bool UseAltitudeColoring
        {
            set
            {
                altitude_coloring = value;
            }
            get
            {
                return altitude_coloring;
            }
        }

        [BrowsableAttribute(true)]
        public double DistanceM
        {
            get
            {
                if (cb_unit.SelectedIndex == 0) // m
                    return tb_distance.DoubleValue;
                else if (cb_unit.SelectedIndex == 1) // km
                    return tb_distance.DoubleValue * 1000.0;
                else if (cb_unit.SelectedIndex == 2) // ft
                    return tb_distance.DoubleValue * 3.2808399;
                else // mile
                    return tb_distance.DoubleValue / 0.000621371192;
            }
            set
            {
                current_distance_m = value;
                cb_unit_SelectedIndexChanged(null, null);
                DistanceTextBox_DistanceChanged(null, EventArgs.Empty);
            }
        }

        public DistanceTextBox()
        {
            InitializeComponent();
            tb_distance.Text = "0";
            if (GluonCS.Properties.Settings.Default.DistanceUnit == "m")
                cb_unit.SelectedIndex = 0;
            else if (GluonCS.Properties.Settings.Default.DistanceUnit == "km")
                cb_unit.SelectedIndex = 1;
            else if (GluonCS.Properties.Settings.Default.DistanceUnit == "ft")
                cb_unit.SelectedIndex = 2;
            else //if (Properties.Settings.Default.DistanceUnit == "mi")
                cb_unit.SelectedIndex = 3;

            DistanceChanged += new EventHandler(DistanceTextBox_DistanceChanged);
            DistanceTextBox_DistanceChanged(null, EventArgs.Empty);
        }

        void DistanceTextBox_DistanceChanged(object sender, EventArgs e)
        {
            if (UseAltitudeColoring)
            {
                if (DistanceM < 30)
                    Color = Color.Red;
                else if (DistanceM < 50)
                    Color = Color.Yellow;
                else
                    Color = Color.White;
            }
        }

        private void cb_unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_unit.SelectedIndex == 0) // m
            {
                tb_distance.Text = current_distance_m.ToString(CultureInfo.InvariantCulture);
                GluonCS.Properties.Settings.Default.DistanceUnit = "m";
            }
            else if (cb_unit.SelectedIndex == 1) // km
            {
                tb_distance.Text = (current_distance_m / 1000.0).ToString(CultureInfo.InvariantCulture);
                GluonCS.Properties.Settings.Default.DistanceUnit = "km";
            }
            else if (cb_unit.SelectedIndex == 2) // ft
            {
                tb_distance.Text = (current_distance_m / 3.2808399).ToString(CultureInfo.InvariantCulture);
                GluonCS.Properties.Settings.Default.DistanceUnit = "ft";
            }
            else // mile
            {
                tb_distance.Text = (current_distance_m * 0.000621371192).ToString(CultureInfo.InvariantCulture);
                GluonCS.Properties.Settings.Default.DistanceUnit = "mi";
            }
            GluonCS.Properties.Settings.Default.Save();
        }

        private void cb_unit_DropDown(object sender, EventArgs e)
        {
            current_distance_m = DistanceM;
        }

        private void tb_distance_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void tb_distance_TextChanged(object sender, EventArgs e)
        {
            if (DistanceChanged != null)
                DistanceChanged(this, e); // or whatever args you want
        }


    }
}
