﻿/*
 *  About form class for EasyRino
 *  Copyright (C) 2016 - 2019  Dusan Misic <promisic@outlook.com>
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
using System.Diagnostics;
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
            easyRinoLabel.Text = $"{easyRinoLabel.Text} {MainForm.GetEasyRinoVersion()}";
        }

        // Event fired when user clicks on USC Bor link
        private void uscBorLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(uscBorLinkLabel.Text);
        }
    }
}