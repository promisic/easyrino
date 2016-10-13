/*
 *  Main form class of EasyRino
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GriffinSoft.EasyRino.Core;
using GriffinSoft.EasyRino.RinoXmlFilter;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino
{
    public partial class MainForm : Form
    {

        #region EasyRino version information region

        /// <summary>
        /// Gets EasyRino version 
        /// </summary>
        /// <returns>EasyRino version</returns>
        public static string GetEasyRinoVersion()
        {
            return "1.0 RC 2";
        }

        #endregion

        #region Internal lists of RINO obligation and reconcilement objects region

        /// <summary>
        /// Internal list of RinoObligationItem's
        /// </summary>
        private List<RinoObligationItem> roiList = new List<RinoObligationItem>();

        /// <summary>
        /// Internal list of RinoReconcilementItem's.
        /// </summary>
        private List<RinoReconcilementItem> rriList = new List<RinoReconcilementItem>();

        #endregion

        #region Internal row number index field's region

        /// <summary>
        /// Internal row number field.
        /// </summary>
        private int rowNumber = 0;

        /// <summary>
        /// Internal reconcilation row number field.
        /// </summary>
        private int reconRowNumber = 0;

        #endregion

        public MainForm()
        {
            InitializeComponent();

            // Setting version information in the tittle bar
            this.Text = this.Text + " " + GetEasyRinoVersion();
        }

        #region Load obligation XML region

        private void loadObligationXmlBtn_Click(object sender, EventArgs e)
        {
            // Clearing existing path if program is already running
            this.openObligationXmlDialog.FileName = "";

            if (this.openObligationXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                string xmlPath = this.openObligationXmlDialog.FileName;

                // Load RINO XML file into memory
                // Creating RINO import object
                IRinoImport rinoObligationXmlImport = new RinoXmlImport();

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Populating roiList object
                roiList = rom.ConvertXmlToRinoList(rinoObligationXmlImport.ImportRinoObligationXml(xmlPath));

                // Checking if XML file is valid reconcilement type
                if (rom.ValidObligation == true)
                {
                    // Converting ROI list to DataTable for display
                    rom.ConvertRinoListToDataTable(roiList);

                    // Setting data to datagrid
                    this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                }
                else
                {
                    // Display error message
                    string errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za zaduženje.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset fields
                this.ResetObligationFields();
            }
        }

        #endregion

        #region Insert new obligation region

        private void insertNewObligationBtn_Click(object sender, EventArgs e)
        {
            // Getting action type
            RinoActionType actionType;

            if (this.rinoActionTypeComboBox.SelectedIndex == 0)
            {
                actionType = RinoActionType.Unos;
            }
            else if (this.rinoActionTypeComboBox.SelectedIndex == 1)
            {
                actionType = RinoActionType.Izmena;
            }
            else if (this.rinoActionTypeComboBox.SelectedIndex == 2)
            {

                actionType = RinoActionType.Otkazivanje;
            }
            else
            {
                actionType = RinoActionType.Unos;
                // Setting Unos as default
                this.rinoActionTypeComboBox.SelectedIndex = 0;
            }

            // Getting vrstaPoverioca type
            RinoVrstaPoverioca vrstaPoverioca;

            if (this.vrstaPoveriocaComboBox.SelectedIndex == 0)
            {
                vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 1)
            {
                vrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 2)
            {
                vrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 3)
            {
                vrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
            }
            else
            {
                vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                // Setting Pravna lica as default
                this.vrstaPoveriocaComboBox.SelectedIndex = 0;
            }

            // Creating dueDate object
            DateTime dueDate = new DateTime();

            // If due date exists, set its value
            if (this.noDueDateCheckBox.Checked == false)
            {
                dueDate = this.datumRokaIzmirenjaDateTimePicker.Value.Date;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = Decimal.Parse(this.rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                // Error message
                string errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                    "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            // Checking if Iznos is not empty
            if (this.rinoIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoObligationItem object
                RinoObligationItem roi = new RinoObligationItem(
                    actionType,
                    iznos,
                    this.nazivPoveriocaTextBox.Text,
                    this.pibTextBox.Text,
                    this.mbTextBox.Text,
                    vrstaPoverioca,
                    this.nazivDokumentaTextBox.Text,
                    this.brojDokumentaTextBox.Text,
                    this.datumDokumentaDateTimePicker.Value.Date,
                    this.datumNastankaDateTimePicker.Value.Date,
                    dueDate,
                    this.razlogIzmeneTextBox.Text
                    );

                // Variable holds ROI check result
                bool RoiDoesNotAlreadyExist = true;

                foreach (RinoObligationItem roiItem in this.roiList)
                {
                    if (((roi.PIBPoverioca == roiItem.PIBPoverioca) &&
                        (roi.BrojDokumenta == roiItem.BrojDokumenta)))
                    {
                        // ROI already exists
                        RoiDoesNotAlreadyExist = false;
                        break;
                    }

                }

                // If ROI does not already exists in the list, then continue
                if (RoiDoesNotAlreadyExist == true)
                {
                    // Checking for data validity
                    if (roi.IsPibValid() == true)
                    {
                        if (roi.IsMbValid() == true)
                        {
                            if (roi.Action == RinoActionType.Izmena || roi.Action == RinoActionType.Otkazivanje)
                            {
                                if (roi.ReasonForChangeValid() == true)
                                {
                                    if (roi.CheckForGeneralValidity() == true)
                                    {
                                        // Creating RinoObligationManager object
                                        RinoObligationManager rom = new RinoObligationManager();

                                        // Inserting new item and setting new value to roiList object
                                        this.roiList = rom.InsertNewItem(this.roiList, roi);

                                        // Converting roiList to DataTable object to display data
                                        rom.ConvertRinoListToDataTable(this.roiList);

                                        // Setting DataSource property to dataGridView object
                                        this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                    }
                                    else
                                    {
                                        // Show error message
                                        string errMsg = "Niste uneli sve obavezne podatke";
                                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    // Show error message
                                    string errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                if (roi.CheckForGeneralValidity() == true)
                                {
                                    // Creating RinoObligationManager object
                                    RinoObligationManager rom = new RinoObligationManager();

                                    // Inserting new item and setting new value to roiList object
                                    this.roiList = rom.InsertNewItem(this.roiList, roi);

                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(this.roiList);

                                    // Setting DataSource property to dataGridView object
                                    this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                }
                                else
                                {
                                    // Show error message
                                    string errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            // Show error message
                            string errMsg = "Niste uneli validan MB.";
                            MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Show error message
                        string errMsg = "Niste uneli validan PIB.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Setting row index to 0
                    this.rowNumber = 0;

                    // Reset fields
                    this.ResetObligationFields();
                }
                else
                {
                    // Show error message
                    string errMsg = "Već ste uneli jedno zaduženje koje ima isti PIB i broj računa u listu obaveza.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Show error message
                string errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region No due date check box manipulation region

        private void noDueDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.noDueDateCheckBox.Checked == true)
            {
                // Disable due date DateTimePicker
                this.datumRokaIzmirenjaDateTimePicker.Enabled = false;
            }
            else
            {
                // Enable due date DateTimePicker
                this.datumRokaIzmirenjaDateTimePicker.Enabled = true;
            }
        }

        #endregion

        #region JBBK configuration region

        private void jbbkConfigLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EasyRinoConfigForm jbbkConfigForm = new EasyRinoConfigForm();
            jbbkConfigForm.ShowDialog();
        }

        #endregion

        #region Modify obligation region

        private void modifyObligationBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.roiList.Capacity > 0)
            {
                // Get row position
                this.rowNumber = this.rinoObligationDataGridView.CurrentCellAddress.Y;

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Declaring RinoObligationItem object
                RinoObligationItem roi;

                // Getting ROI object
                roi = rom.GetRoiItemAt(this.roiList, this.rowNumber);

                // Setting values to all UI fields
                // Setting action type
                if (roi.Action == RinoActionType.Unos)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 0;
                }
                else if (roi.Action == RinoActionType.Izmena)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 1;
                }
                else if (roi.Action == RinoActionType.Otkazivanje)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 2;
                }

                // Setting iznos
                this.rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");

                // Setting NazivPoverioca
                this.nazivPoveriocaTextBox.Text = roi.NazivPoverioca;

                // Setting PIBPoverioca
                this.pibTextBox.Text = roi.PIBPoverioca;

                // Setting MBPoverioca
                this.mbTextBox.Text = roi.MBPoverioca;

                // Setting VrstaPoverioca
                if (roi.VrstaPoverioca == RinoVrstaPoverioca.PravnaLica)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 0;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.JavniSektor)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 1;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.PoljoprivrednaGazdinstva)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 2;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.Kompenzacija)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 3;
                }

                // Setting NazivDokumenta
                this.nazivDokumentaTextBox.Text = roi.NazivDokumenta;

                // Setting BrojDokumenta
                this.brojDokumentaTextBox.Text = roi.BrojDokumenta;

                // Setting DatumDokumenta
                this.datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;

                // Setting DatumNastanka
                this.datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                // Setting DatumRokaZaIzmirenje
                if (rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje) != true)
                {
                    // Unchecking check box
                    this.noDueDateCheckBox.Checked = false;
                    // Enabling DateTimePicker
                    this.datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    // Setting DatumRokaZaIzmirenje
                    this.datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    // Checking check box
                    this.noDueDateCheckBox.Checked = true;
                    // Disabling DateTimePicker
                    this.datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                // Setting RazlogIzmene
                if (roi.RazlogIzmene != null)
                {
                    if (roi.RazlogIzmene.Length > 0)
                    {
                        this.razlogIzmeneTextBox.Text = roi.RazlogIzmene;
                    }
                }
                else
                {
                    this.razlogIzmeneTextBox.Text = null;
                }

                // Enabling save obligation changes button
                this.saveObligationChangesBtn.Visible = true;
            }
            else
            {
                // Do nothing or possibly pop a message box :)
            }
        }

        #endregion

        #region Delete obligation region

        private void deleteObligationBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    this.rowNumber = this.rinoObligationDataGridView.CurrentCellAddress.Y;

                    // Creating RinoObligationManager object
                    RinoObligationManager rom = new RinoObligationManager();

                    // Remove item at selected index position
                    this.roiList = rom.RemoveItemAt(this.roiList, this.rowNumber);

                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(this.roiList);

                    // Setting DataSource property to dataGridView object
                    this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;

                    // Setting row index to 0
                    this.rowNumber = 0;

                    // Reset fields
                    this.ResetObligationFields();
                }
            }
        }

        #endregion

        #region Delete all obligations region

        private void deleteAllObligationsBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Clearing roiList object
                    this.roiList.Clear();

                    // Creating RinoObligationManager object
                    RinoObligationManager rom = new RinoObligationManager();

                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(this.roiList);

                    // Setting DataSource property to dataGridView object
                    this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;

                    // Reset all fields
                    this.ResetObligationFields();
                }
            }
        }

        #endregion

        #region Save obligation XML file region

        private void saveObligationXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.CheckIfJbbkSettingsExist() == true)
            {
                if (this.roiList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    this.saveObligationXmlDialog.FileName = "";

                    if (this.saveObligationXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        // RINO XML path
                        string xmlPath = this.saveObligationXmlDialog.FileName;

                        // Getting JBBK number from settings file
                        string jbbk = Properties.Settings.Default.jbbk;

                        // Creating RinoObligationManager object
                        RinoObligationManager rom = new RinoObligationManager();

                        // Setting JBBK number
                        rom.Jbbk = jbbk;

                        // Creating RinoXmlExport object
                        IRinoExporter rinoXmlExport = new RinoXmlExport();

                        // Saving file to filesystem
                        rinoXmlExport.ExportRinoObligationXml(rom.ConvertRinoListToXml(this.roiList),
                            xmlPath);

                        // Reset fields
                        this.ResetObligationFields();
                    }
                }
                else
                {
                    // Showing error
                    MessageBox.Show("Nije moguće snimanje prazne liste obaveza.", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Showing error
                MessageBox.Show("Niste uneli Vaš JBBK u program. Molimo Vas, unesite ga da bi Vam bilo omogućeno snimanje.",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region Save obligation changes region

        private void saveObligationChangesBtn_Click(object sender, EventArgs e)
        {

            // Getting action type
            RinoActionType actionType;

            if (this.rinoActionTypeComboBox.SelectedIndex == 0)
            {
                actionType = RinoActionType.Unos;
            }
            else if (this.rinoActionTypeComboBox.SelectedIndex == 1)
            {
                actionType = RinoActionType.Izmena;
            }
            else if (this.rinoActionTypeComboBox.SelectedIndex == 2)
            {

                actionType = RinoActionType.Otkazivanje;
            }
            else
            {
                actionType = RinoActionType.Unos;
                // Setting Unos as default
                this.rinoActionTypeComboBox.SelectedIndex = 0;
            }

            // Getting vrstaPoverioca type
            RinoVrstaPoverioca vrstaPoverioca;

            if (this.vrstaPoveriocaComboBox.SelectedIndex == 0)
            {
                vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 1)
            {
                vrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 2)
            {
                vrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
            }
            else if (this.vrstaPoveriocaComboBox.SelectedIndex == 3)
            {
                vrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
            }
            else
            {
                vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                // Setting Pravna lica as default
                this.vrstaPoveriocaComboBox.SelectedIndex = 0;
            }

            // Creating dueDate object
            DateTime dueDate = new DateTime();

            // If due date exists, set its value
            if (this.noDueDateCheckBox.Checked == false)
            {
                dueDate = this.datumRokaIzmirenjaDateTimePicker.Value.Date;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = Decimal.Parse(this.rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                // Error message
                string errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                    "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            // Checking if Iznos is not empty
            if (this.rinoIznosTextBox.Text.Length > 0)
            {

                // Creating new RinoObligationItem object
                RinoObligationItem roi = new RinoObligationItem(
                actionType,
                iznos,
                this.nazivPoveriocaTextBox.Text,
                this.pibTextBox.Text,
                this.mbTextBox.Text,
                vrstaPoverioca,
                this.nazivDokumentaTextBox.Text,
                this.brojDokumentaTextBox.Text,
                this.datumDokumentaDateTimePicker.Value.Date,
                this.datumNastankaDateTimePicker.Value.Date,
                dueDate,
                this.razlogIzmeneTextBox.Text
                );

                // Checking for data validity
                if (roi.IsPibValid() == true)
                {
                    if (roi.IsMbValid() == true)
                    {
                        if (roi.Action == RinoActionType.Izmena || roi.Action == RinoActionType.Otkazivanje)
                        {
                            if (roi.ReasonForChangeValid() == true)
                            {
                                if (roi.CheckForGeneralValidity() == true)
                                {
                                    // Creating RinoObligationManager object
                                    RinoObligationManager rom = new RinoObligationManager();

                                    // Inserting new item and setting new value to roiList object
                                    this.roiList = rom.ModifyExistingItem(this.roiList, roi, this.rowNumber);

                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(this.roiList);

                                    // Setting DataSource property to dataGridView object
                                    this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                }
                                else
                                {
                                    // Show error message
                                    string errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (roi.CheckForGeneralValidity() == true)
                            {
                                // Creating RinoObligationManager object
                                RinoObligationManager rom = new RinoObligationManager();

                                // Inserting new item and setting new value to roiList object
                                this.roiList = rom.ModifyExistingItem(this.roiList, roi, this.rowNumber);

                                // Converting roiList to DataTable object to display data
                                rom.ConvertRinoListToDataTable(this.roiList);

                                // Setting DataSource property to dataGridView object
                                this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        // Show error message
                        string errMsg = "Niste uneli validan MB.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Show error message
                    string errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Setting row index to 0
                this.rowNumber = 0;

                // Hide save obligation changes button
                this.saveObligationChangesBtn.Visible = false;

                // Reset fields
                this.ResetObligationFields();
            }
            else
            {
                // Show error message
                string errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        #endregion

        #region Clone obligation region

        private void cloneObliationBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.roiList.Capacity > 0)
            {
                // Get row position
                this.rowNumber = this.rinoObligationDataGridView.CurrentCellAddress.Y;

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Declaring RinoObligationItem object
                RinoObligationItem roi;

                // Getting RIO object
                roi = rom.GetRoiItemAt(this.roiList, this.rowNumber);

                // Setting values to all UI fields
                // Setting action type
                if (roi.Action == RinoActionType.Unos)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 0;
                }
                else if (roi.Action == RinoActionType.Izmena)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 1;
                }
                else if (roi.Action == RinoActionType.Otkazivanje)
                {
                    this.rinoActionTypeComboBox.SelectedIndex = 2;
                }

                // Setting iznos
                this.rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");

                // Setting NazivPoverioca
                this.nazivPoveriocaTextBox.Text = roi.NazivPoverioca;

                // Setting PIBPoverioca
                this.pibTextBox.Text = roi.PIBPoverioca;

                // Setting MBPoverioca
                this.mbTextBox.Text = roi.MBPoverioca;

                // Setting VrstaPoverioca
                if (roi.VrstaPoverioca == RinoVrstaPoverioca.PravnaLica)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 0;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.JavniSektor)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 1;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.PoljoprivrednaGazdinstva)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 2;
                }
                else if (roi.VrstaPoverioca == RinoVrstaPoverioca.Kompenzacija)
                {
                    this.vrstaPoveriocaComboBox.SelectedIndex = 3;
                }

                // Setting NazivDokumenta
                this.nazivDokumentaTextBox.Text = roi.NazivDokumenta;

                // Setting BrojDokumenta
                this.brojDokumentaTextBox.Text = roi.BrojDokumenta;

                // Setting DatumDokumenta
                this.datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;

                // Setting DatumNastanka
                this.datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                // Setting DatumRokaZaIzmirenje
                if (rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje) != true)
                {
                    // Unchecking check box
                    this.noDueDateCheckBox.Checked = false;
                    // Enabling DateTimePicker
                    this.datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    // Setting DatumRokaZaIzmirenje
                    this.datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    // Checking check box
                    this.noDueDateCheckBox.Checked = true;
                    // Disabling DateTimePicker
                    this.datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                // Setting RazlogIzmene
                if (roi.RazlogIzmene != null)
                {
                    if (roi.RazlogIzmene.Length > 0)
                    {
                        this.razlogIzmeneTextBox.Text = roi.RazlogIzmene;
                    }
                }
                else
                {
                    this.razlogIzmeneTextBox.Text = null;
                }
            }
        }

        #endregion

        #region Load reconcilement XML region

        private void loadReconcilementXmlBtn_Click(object sender, EventArgs e)
        {
            // Clearing existing path if program is already running
            this.openReconcilementXmlDialog.FileName = "";

            if (this.openReconcilementXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                string xmlPath = this.openReconcilementXmlDialog.FileName;

                // Load RINO XML file into memory
                // Creating RINO import object
                IRinoImport rinoReconcilementXmlImport = new RinoXmlImport();

                // Creating RinoObligationManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Populating roiList object
                rriList = rrm.ConvertXmlToRinoList(rinoReconcilementXmlImport.ImportRinoObligationXml(xmlPath));

                // Checking if XML file is valid reconcilement type
                if (rrm.ValidReconcilement == true)
                {
                    // Converting ROI list to DataTable for display
                    rrm.ConvertRinoListToDataTable(rriList);

                    // Setting data to datagrid
                    this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                }
                else
                {
                    // Display error message
                    string errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za razduženje.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset fields
                this.ResetReconcilementFields();
            }
        }

        #endregion

        #region Save reconcilement XML file region

        private void saveReconcilementXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.CheckIfJbbkSettingsExist() == true)
            {
                if (this.rriList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    this.saveReconcilementXmlDialog.FileName = "";

                    if (this.saveReconcilementXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        // RINO XML path
                        string xmlPath = this.saveReconcilementXmlDialog.FileName;

                        // Getting JBBK number from settings file
                        string jbbk = Properties.Settings.Default.jbbk;

                        // Creating RinoReconcilementManager object
                        RinoReconcilementManager rrm = new RinoReconcilementManager();

                        // Setting JBBK number
                        rrm.Jbbk = jbbk;

                        // Creating RinoXmlExport object
                        IRinoExporter rinoXmlExport = new RinoXmlExport();

                        // Saving file to filesystem
                        rinoXmlExport.ExportRinoObligationXml(rrm.ConvertRinoListToXml(this.rriList),
                            xmlPath);

                        // Reset fields
                        this.ResetReconcilementFields();
                    }
                }
                else
                {
                    // Showing error
                    MessageBox.Show("Nije moguće snimanje prazne liste razduženja.", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Showing error
                MessageBox.Show("Niste uneli Vaš JBBK u program. Molimo Vas, unesite ga da bi Vam bilo omogućeno snimanje.",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region Insert as new reconcilation region

        private void insertAsNewReconBtn_Click(object sender, EventArgs e)
        {
            // Getting action type
            RinoActionType actionType;

            if (this.reconRinoActionTypeComboBox.SelectedIndex == 0)
            {
                actionType = RinoActionType.Unos;
            }
            else if (this.reconRinoActionTypeComboBox.SelectedIndex == 1)
            {
                actionType = RinoActionType.Izmena;
            }
            else if (this.reconRinoActionTypeComboBox.SelectedIndex == 2)
            {

                actionType = RinoActionType.Otkazivanje;
            }
            else
            {
                actionType = RinoActionType.Unos;
                // Setting Unos as default
                this.reconRinoActionTypeComboBox.SelectedIndex = 0;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = Decimal.Parse(this.reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                // Error message
                string errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                    "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            if (this.reconIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoReconcilementItem object
                RinoReconcilementItem rri = new RinoReconcilementItem(
                    actionType,
                    Convert.ToInt64(this.rinoIdTextBox.Text),
                    this.reconBrojDokumentaTextBox.Text,
                    this.reconPibTextBox.Text,
                    this.reconBankTextBox.Text,
                    this.reconPodZaReklTextBox.Text,
                    this.reconDatumIzmirenjaDateTimePicker.Value.Date,
                    iznos,
                    this.razlogIzmeneTextBox.Text
                    );

                // Checking for data validity
                if (rri.IsPibValid() == true)
                {
                    if (rri.IsReklPodZaRekValid() == true)
                    {
                        if (rri.Action == RinoActionType.Izmena || rri.Action == RinoActionType.Otkazivanje)
                        {
                            if (rri.ReasonForChangeValid() == true)
                            {
                                if (rri.CheckForGeneralValidity() == true)
                                {
                                    // Creating RinoReconcilementManager object
                                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                                    // Inserting new item and setting new value to rriList object
                                    this.rriList = rrm.InsertNewItem(this.rriList, rri);

                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(this.rriList);

                                    // Setting DataSource property to dataGridView object
                                    this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                                }
                                else
                                {
                                    // Show error message
                                    string errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (rri.CheckForGeneralValidity() == true)
                            {
                                // Creating RinoReconcilementManager object
                                RinoReconcilementManager rrm = new RinoReconcilementManager();

                                // Inserting new item and setting new value to rriList object
                                this.rriList = rrm.InsertNewItem(this.rriList, rri);

                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(this.rriList);

                                // Setting DataSource property to dataGridView object
                                this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        // Show error message
                        string errMsg = "Niste uneli podatak za reklamaciju.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    // Show error message
                    string errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Setting row index to 0
                this.reconRowNumber = 0;

                // Hiding insert as new reconcilation button
                this.insertAsNewReconBtn.Visible = false;

                // Reset fields
                this.ResetReconcilementFields();
            }
            else
            {
                // Show error message
                string errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Save reconcilement changes region

        private void saveReconcilementChangesBtn_Click(object sender, EventArgs e)
        {
            // Getting action type
            RinoActionType actionType;

            if (this.reconRinoActionTypeComboBox.SelectedIndex == 0)
            {
                actionType = RinoActionType.Unos;
            }
            else if (this.reconRinoActionTypeComboBox.SelectedIndex == 1)
            {
                actionType = RinoActionType.Izmena;
            }
            else if (this.reconRinoActionTypeComboBox.SelectedIndex == 2)
            {

                actionType = RinoActionType.Otkazivanje;
            }
            else
            {
                actionType = RinoActionType.Unos;
                // Setting Unos as default
                this.reconRinoActionTypeComboBox.SelectedIndex = 0;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = Decimal.Parse(this.reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                // Error message
                string errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                    "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            if (this.reconIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoReconcilementItem object
                RinoReconcilementItem rri = new RinoReconcilementItem(
                    actionType,
                    Int64.Parse(this.rinoIdTextBox.Text),
                    this.reconBrojDokumentaTextBox.Text,
                    this.reconPibTextBox.Text,
                    this.reconBankTextBox.Text,
                    this.reconPodZaReklTextBox.Text,
                    this.reconDatumIzmirenjaDateTimePicker.Value.Date,
                    iznos,
                    this.razlogIzmeneTextBox.Text
                    );

                // Checking for data validity
                if (rri.IsPibValid() == true)
                {
                    if (rri.IsReklPodZaRekValid() == true)
                    {
                        if (rri.Action == RinoActionType.Izmena || rri.Action == RinoActionType.Otkazivanje)
                        {
                            if (rri.ReasonForChangeValid() == true)
                            {
                                if (rri.CheckForGeneralValidity() == true)
                                {
                                    // Creating RinoReconcilementManager object
                                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                                    // Inserting new item and setting new value to rriList object
                                    this.rriList = rrm.ModifyExistingItem(this.rriList, rri, this.reconRowNumber);

                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(this.rriList);

                                    // Setting DataSource property to dataGridView object
                                    this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                                }
                                else
                                {
                                    // Show error message
                                    string errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (rri.CheckForGeneralValidity() == true)
                            {
                                // Creating RinoReconcilementManager object
                                RinoReconcilementManager rrm = new RinoReconcilementManager();

                                // Inserting new item and setting new value to rriList object
                                this.rriList = rrm.ModifyExistingItem(this.rriList, rri, this.reconRowNumber);

                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(this.rriList);

                                // Setting DataSource property to dataGridView object
                                this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                            }
                            else
                            {
                                // Show error message
                                string errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        // Show error message
                        string errMsg = "Niste uneli podatak za reklamaciju.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Show error message
                    string errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Hiding save reconcilation changes button
                this.saveReconcilementChangesBtn.Visible = false;

                // Setting row index to 0
                this.reconRowNumber = 0;

                // Reset fields
                this.ResetReconcilementFields();
            }
            else
            {
                // Show error message
                string errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Modify reconcilement region

        private void modifyReconcilementBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.rriList.Capacity > 0)
            {
                // Get row position
                this.reconRowNumber = this.rinoReconcilementDataGridView.CurrentCellAddress.Y;

                // Creating RinoReconcilementManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Declaring RinoObligationItem object
                RinoReconcilementItem rri;

                // Getting RRI object
                rri = rrm.GetRriItemAt(this.rriList, this.reconRowNumber);

                // Setting values to all UI fields
                // Setting action type
                if (rri.Action == RinoActionType.Unos)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 0;
                }
                else if (rri.Action == RinoActionType.Izmena)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 1;
                }
                else if (rri.Action == RinoActionType.Otkazivanje)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 2;
                }

                // Setting RINO ID
                this.rinoIdTextBox.Text = rri.RinoId.ToString();

                // Setting PIBPoverioca
                this.reconPibTextBox.Text = rri.PIBPoverioca;

                // Setting BrojDokumenta
                this.reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;

                // Setting Banka
                this.reconBankTextBox.Text = rri.Banka;

                // Setting ReklPodZaRek
                this.reconPodZaReklTextBox.Text = rri.ReklPodZaRek;

                // Setting DatumIzmirenja
                if (rrm.CheckUninitializedDate(rri.DatumIzmirenja) != true)
                {
                    // Setting DatumIzmirenja
                    this.reconDatumIzmirenjaDateTimePicker.Value = rri.DatumIzmirenja;
                }
                else
                {
                    // Default uninitialized value detected, put today as main date
                    this.reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
                }

                // Setting iznos
                this.reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));

                // Setting RazlogIzmene
                if (rri.RazlogIzmene != null)
                {
                    if (rri.RazlogIzmene.Length > 0)
                    {
                        this.razlogIzmeneTextBox.Text = rri.RazlogIzmene;
                    }
                }
                else
                {
                    this.razlogIzmeneTextBox.Text = null;
                }

                // Enabling save reconcilement changes button
                this.saveReconcilementChangesBtn.Visible = true;
            }
            else
            {
                // Do nothing or possibly pop a message box :)
            }
        }

        #endregion

        #region Remove selected reconcilement from list region

        private void deleteReconcilementBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    this.reconRowNumber = this.rinoReconcilementDataGridView.CurrentCellAddress.Y;

                    // Creating RinoReconcilementManager object
                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                    // Remove item at selected index position
                    this.rriList = rrm.RemoveItemAt(this.rriList, this.reconRowNumber);

                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(this.rriList);

                    // Setting DataSource property to dataGridView object
                    this.rinoObligationDataGridView.DataSource = rrm.RinoDataTable;

                    // Setting row index to 0
                    this.reconRowNumber = 0;

                    // Reset fields
                    this.ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Clone reconcilement region

        private void cloneReconcilementBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (this.rriList.Capacity > 0)
            {
                // Get row position
                this.reconRowNumber = this.rinoReconcilementDataGridView.CurrentCellAddress.Y;

                // Creating RinoReconcilementManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Declaring RinoObligationItem object
                RinoReconcilementItem rri;

                // Getting RRI object
                rri = rrm.GetRriItemAt(this.rriList, this.reconRowNumber);

                // Setting values to all UI fields
                // Setting action type
                if (rri.Action == RinoActionType.Unos)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 0;
                }
                else if (rri.Action == RinoActionType.Izmena)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 1;
                }
                else if (rri.Action == RinoActionType.Otkazivanje)
                {
                    this.reconRinoActionTypeComboBox.SelectedIndex = 2;
                }

                // Setting RINO ID
                this.rinoIdTextBox.Text = rri.RinoId.ToString();

                // Setting PIBPoverioca
                this.reconPibTextBox.Text = rri.PIBPoverioca;

                // Setting BrojDokumenta
                this.reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;

                // Setting Banka
                this.reconBankTextBox.Text = rri.Banka;

                // Setting ReklPodZaRek
                this.reconPodZaReklTextBox.Text = rri.ReklPodZaRek;

                // Setting DatumIzmirenja
                if (rrm.CheckUninitializedDate(rri.DatumIzmirenja) != true)
                {
                    // Setting DatumIzmirenja
                    this.reconDatumIzmirenjaDateTimePicker.Value = rri.DatumIzmirenja;
                }
                else
                {
                    // Default uninitialized value detected, put today as main date
                    this.reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
                }
                // Setting iznos
                this.reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));

                // Setting RazlogIzmene
                if (rri.RazlogIzmene != null)
                {
                    if (rri.RazlogIzmene.Length > 0)
                    {
                        this.razlogIzmeneTextBox.Text = rri.RazlogIzmene;
                    }
                }
                else
                {
                    this.razlogIzmeneTextBox.Text = null;
                }

                // Enabling save reconcilement changes button
                this.insertAsNewReconBtn.Visible = true;
            }
        }

        #endregion

        #region Remove empty statements region

        private void removeUnpopReconBtn_Click(object sender, EventArgs e)
        {

            // Checking if internal rriList object is not empty
            if (this.rriList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da uklonite prazne stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Creting RinoReconcilement object
                    List<RinoReconcilementItem> fullRriList = new List<RinoReconcilementItem>();

                    // Creating RinoReconcilementManager object
                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                    foreach (RinoReconcilementItem rriItem in this.rriList)
                    {
                        // Checking if requested fields are NOT empty
                        if (rriItem.Banka.Length > 0 && rriItem.ReklPodZaRek.Length > 0
                            && (rrm.CheckUninitializedDate(rriItem.DatumIzmirenja) == false) &&
                            rriItem.Iznos > 0)
                        {
                            // Add RRI item to list
                            fullRriList.Add(rriItem);
                        }
                    }

                    // Set full clean list as new main list
                    this.rriList = fullRriList;

                    // Performing conversion
                    rrm.ConvertRinoListToDataTable(this.rriList);

                    // Displaying new "clean" list
                    this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;

                    // Reset fields
                    this.ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Removing all reconcilements from list region

        private void removeAllReconcilementsBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal rriList object is not empty
            if (this.rriList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Clearing roiList object
                    this.rriList.Clear();

                    // Creating RinoReconcilementManager object
                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(this.rriList);

                    // Setting DataSource property to dataGridView object
                    this.rinoObligationDataGridView.DataSource = rrm.RinoDataTable;

                    // Reset fields
                    this.ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Form_Load event region

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Getting startMaximized flag
            bool startMaximized = Properties.Settings.Default.startMaximized;

            // Restoring window size
            if (startMaximized == true)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        #endregion

        #region FormClosing event region

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Saving windows size
            if (this.WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.startMaximized = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.startMaximized = false;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Form field's reset region

        private void ResetObligationFields()
        {
            this.rinoActionTypeComboBox.SelectedIndex = -1;
            this.rinoIznosTextBox.Text = "";
            this.nazivPoveriocaTextBox.Text = "";
            this.pibTextBox.Text = "";
            this.mbTextBox.Text = "";
            this.vrstaPoveriocaComboBox.SelectedIndex = -1;
            this.nazivDokumentaTextBox.Text = "";
            this.brojDokumentaTextBox.Text = "";
            this.datumDokumentaDateTimePicker.Value = DateTime.Now;
            this.datumNastankaDateTimePicker.Value = DateTime.Now;
            this.datumRokaIzmirenjaDateTimePicker.Value = DateTime.Now;
            this.datumRokaIzmirenjaDateTimePicker.Enabled = true;
            this.noDueDateCheckBox.Checked = false;
            this.razlogIzmeneTextBox.Text = "";
        }

        private void ResetReconcilementFields()
        {
            this.reconRinoActionTypeComboBox.SelectedIndex = -1;
            this.rinoIdTextBox.Text = "";
            this.reconBrojDokumentaTextBox.Text = "";
            this.reconPibTextBox.Text = "";
            this.reconBankTextBox.Text = "";
            this.reconPodZaReklTextBox.Text = "";
            // DatumIzmirenjaDateTimePicker is not reseted by design, because of faster reconcilation times
            // this.reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
            this.reconIznosTextBox.Text = "";
            this.reconRazlogIzmeneTextBox.Text = "";
        }

        #endregion

        #region JBBK info check region

        private bool CheckIfJbbkSettingsExist()
        {
            if (Properties.Settings.Default.jbbk.Length > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region About form click region

        private void aboutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Creating about window
            EasyRinoAboutForm aboutForm = new EasyRinoAboutForm();
            aboutForm.ShowDialog();
        }

        #endregion
    }
}
