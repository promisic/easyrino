﻿/*
 *  About form class for EasyRino
 *  Copyright (C) 2016  Dusan Misic <promisic@outlook.com>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GriffinSoft.EasyRino
{
    public partial class EasyRinoAboutForm : Form
    {
        public EasyRinoAboutForm()
        {
            InitializeComponent();
        }

        // Event fired when about form is shown
        private void EasyRinoAboutForm_Load(object sender, EventArgs e)
        {
            // Setting version information
            this.easyRinoLabel.Text = this.easyRinoLabel.Text + " " + MainForm.GetEasyRinoVersion();
        }

        // 
        private void uscBorLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.uscBorLinkLabel.Text);
        }
    }
}
