﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Communication.Frames.Incoming;

namespace Configuration.NavigationCommands
{
    public partial class CircleToAbs : UserControl, INavigationCommandViewer
    {
        private NavigationInstruction ni;

        public CircleToAbs(NavigationInstruction ni)
        {
            InitializeComponent();
            SetNavigationInstruction(ni);
        }

        #region INavigationCommandViewer Members

        public NavigationInstruction GetNavigationInstruction()
        {
            ni = new NavigationInstruction(ni);
            ni.b = (int)_dtb_height.DistanceM;
            ni.x = _ce.GetLatitudeRad();
            ni.y = _ce.GetLongitudeRad();
            ni.opcode = NavigationInstruction.navigation_command.CIRCLE_TO_ABS;
            return ni;
        }

        public void SetNavigationInstruction(NavigationInstruction ni)
        {
            this.ni = ni;
            _dtb_height.DistanceM = ni.b;
            _ce.SetCoordinateRad(ni.x, ni.y);
        }

        #endregion
    }
}
