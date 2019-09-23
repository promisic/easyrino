/*
 *  Main form class of EasyRino
 *  Copyright (C) 2016 -2019 Dusan Misic <promisic@outlook.com>
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
using System.Globalization;
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
            return "1.0";
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
            Text = $"{Text} {GetEasyRinoVersion()}";
        }

        #region Load obligation XML region

        private void loadObligationXmlBtn_Click(object sender, EventArgs e)
        {
            // Clearing existing path if program is already running
            openObligationXmlDialog.FileName = "";

            if (openObligationXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                string xmlPath = openObligationXmlDialog.FileName;

                // Load RINO XML file into memory
                // Creating RINO import object
                IRinoImport rinoObligationXmlImport = new RinoXmlImport();

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Populating roiList object
                roiList = rom.ConvertXmlToRinoList(rinoObligationXmlImport.ImportRinoObligationXml(xmlPath));

                // Checking if XML file is valid reconcilement type
                if (rom.ValidObligation)
                {
                    // Converting ROI list to DataTable for display
                    rom.ConvertRinoListToDataTable(roiList);

                    // Setting data to datagrid
                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                }
                else
                {
                    // Display error message
                    string errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za zaduženje.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset fields
                ResetObligationFields();
            }
        }

        #endregion

        #region Insert new obligation region

        private void insertNewObligationBtn_Click(object sender, EventArgs e)
        {
            RinoActionType actionType = GetRinoActionType();
            RinoVrstaPoverioca vrstaPoverioca = GetRinoVrstaPoverioca();

            // Creating dueDate object
            DateTime dueDate = new DateTime();

            // If due date exists, set its value
            if (noDueDateCheckBox.Checked == false)
            {
                dueDate = datumRokaIzmirenjaDateTimePicker.Value.Date;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = decimal.Parse(rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
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
            if (rinoIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoObligationItem object
                RinoObligationItem roi = new RinoObligationItem(
                    actionType,
                    iznos,
                    nazivPoveriocaTextBox.Text,
                    pibTextBox.Text,
                    mbTextBox.Text,
                    vrstaPoverioca,
                    nazivDokumentaTextBox.Text,
                    brojDokumentaTextBox.Text,
                    datumDokumentaDateTimePicker.Value.Date,
                    datumNastankaDateTimePicker.Value.Date,
                    dueDate,
                    razlogIzmeneTextBox.Text
                    );

                // Variable holds ROI check result
                bool RoiDoesNotAlreadyExist = true;

                foreach (RinoObligationItem roiItem in roiList)
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
                                if (roi.ReasonForChangeValid())
                                {
                                    if (roi.CheckForGeneralValidity())
                                    {
                                        // Creating RinoObligationManager object
                                        RinoObligationManager rom = new RinoObligationManager();

                                        // Inserting new item and setting new value to roiList object
                                        roiList = rom.InsertNewItem(roiList, roi);

                                        // Converting roiList to DataTable object to display data
                                        rom.ConvertRinoListToDataTable(roiList);

                                        // Setting DataSource property to dataGridView object
                                        rinoObligationDataGridView.DataSource = rom.RinoDataTable;
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
                                if (roi.CheckForGeneralValidity())
                                {
                                    // Creating RinoObligationManager object
                                    RinoObligationManager rom = new RinoObligationManager();

                                    // Inserting new item and setting new value to roiList object
                                    roiList = rom.InsertNewItem(roiList, roi);

                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(roiList);

                                    // Setting DataSource property to dataGridView object
                                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
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
                    rowNumber = 0;

                    // Reset fields
                    ResetObligationFields();
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

        private RinoVrstaPoverioca GetRinoVrstaPoverioca()
        {
            // Getting vrstaPoverioca type
            RinoVrstaPoverioca vrstaPoverioca;

            switch (vrstaPoveriocaComboBox.SelectedIndex)
            {
                case 0:
                    vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                    break;
                case 1:
                    vrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
                    break;
                case 2:
                    vrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
                    break;
                case 3:
                    vrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
                    break;
                default:
                    vrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                    vrstaPoveriocaComboBox.SelectedIndex = 0;
                    break;
            }

            return vrstaPoverioca;
        }

        private RinoActionType GetRinoActionType()
        {
            // Getting action type
            RinoActionType actionType;

            switch (rinoActionTypeComboBox.SelectedIndex)
            {
                case 0:
                    actionType = RinoActionType.Unos;
                    break;
                case 1:
                    actionType = RinoActionType.Izmena;
                    break;
                case 2:
                    actionType = RinoActionType.Otkazivanje;
                    break;
                default:
                    actionType = RinoActionType.Unos;
                    rinoActionTypeComboBox.SelectedIndex = 0;
                    break;
            }

            return actionType;
        }

        #endregion

        #region No due date check box manipulation region

        private void noDueDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            datumRokaIzmirenjaDateTimePicker.Enabled = !noDueDateCheckBox.Checked;
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
            if (roiList.Capacity > 0)
            {
                // Get row position
                rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Declaring RinoObligationItem object
                RinoObligationItem roi;

                // Getting ROI object
                roi = rom.GetRoiItemAt(roiList, rowNumber);

                // Setting values to all UI fields
                // Setting action type
                switch (roi.Action)
                {
                    case RinoActionType.Unos:
                        rinoActionTypeComboBox.SelectedIndex = 0;
                        break;
                    case RinoActionType.Izmena:
                        rinoActionTypeComboBox.SelectedIndex = 1;
                        break;
                    case RinoActionType.Otkazivanje:
                        rinoActionTypeComboBox.SelectedIndex = 2;
                        break;
                }

                // Setting iznos
                rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");

                // Setting NazivPoverioca
                nazivPoveriocaTextBox.Text = roi.NazivPoverioca;

                // Setting PIBPoverioca
                pibTextBox.Text = roi.PIBPoverioca;

                // Setting MBPoverioca
                mbTextBox.Text = roi.MBPoverioca;

                // Setting VrstaPoverioca
                switch (roi.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        vrstaPoveriocaComboBox.SelectedIndex = 0;
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        vrstaPoveriocaComboBox.SelectedIndex = 1;
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        vrstaPoveriocaComboBox.SelectedIndex = 2;
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        vrstaPoveriocaComboBox.SelectedIndex = 3;
                        break;
                }

                // Setting NazivDokumenta
                nazivDokumentaTextBox.Text = roi.NazivDokumenta;

                // Setting BrojDokumenta
                brojDokumentaTextBox.Text = roi.BrojDokumenta;

                // Setting DatumDokumenta
                datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;

                // Setting DatumNastanka
                datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                // Setting DatumRokaZaIzmirenje
                if (!rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje))
                {
                    // Unchecking check box
                    noDueDateCheckBox.Checked = false;
                    // Enabling DateTimePicker
                    datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    // Setting DatumRokaZaIzmirenje
                    datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    // Checking check box
                    noDueDateCheckBox.Checked = true;
                    // Disabling DateTimePicker
                    datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                // Setting RazlogIzmene
                if (roi.RazlogIzmene != null && roi.RazlogIzmene.Length > 0)
                {
                    razlogIzmeneTextBox.Text = roi.RazlogIzmene;
                }
                else
                {
                    razlogIzmeneTextBox.Text = null;
                }

                // Enabling save obligation changes button
                saveObligationChangesBtn.Visible = true;
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
            if (roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;

                    // Creating RinoObligationManager object
                    RinoObligationManager rom = new RinoObligationManager();

                    // Remove item at selected index position
                    roiList = rom.RemoveItemAt(roiList, rowNumber);

                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(roiList);

                    // Setting DataSource property to dataGridView object
                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;

                    // Setting row index to 0
                    rowNumber = 0;

                    // Reset fields
                    ResetObligationFields();
                }
            }
        }

        #endregion

        #region Delete all obligations region

        private void deleteAllObligationsBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Clearing roiList object
                    roiList.Clear();

                    // Creating RinoObligationManager object
                    RinoObligationManager rom = new RinoObligationManager();

                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(roiList);

                    // Setting DataSource property to dataGridView object
                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;

                    // Reset all fields
                    ResetObligationFields();
                }
            }
        }

        #endregion

        #region Save obligation XML file region

        private void saveObligationXmlBtn_Click(object sender, EventArgs e)
        {
            if (CheckIfJbbkSettingsExist())
            {
                if (roiList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    saveObligationXmlDialog.FileName = "";

                    if (saveObligationXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        // RINO XML path
                        string xmlPath = saveObligationXmlDialog.FileName;

                        // Getting JBBK number from settings file
                        string jbbk = Properties.Settings.Default.jbbk;

                        // Creating RinoObligationManager object
                        RinoObligationManager rom = new RinoObligationManager();

                        // Setting JBBK number
                        rom.Jbbk = jbbk;

                        // Creating RinoXmlExport object
                        IRinoExporter rinoXmlExport = new RinoXmlExport();

                        // Saving file to filesystem
                        rinoXmlExport.ExportRinoObligationXml(rom.ConvertRinoListToXml(roiList),
                            xmlPath);

                        // Reset fields
                        ResetObligationFields();
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

            RinoActionType actionType = GetRinoActionType();
            RinoVrstaPoverioca vrstaPoverioca = GetRinoVrstaPoverioca();

            // Creating dueDate object
            DateTime dueDate = new DateTime();

            // If due date exists, set its value
            if (!noDueDateCheckBox.Checked)
            {
                dueDate = datumRokaIzmirenjaDateTimePicker.Value.Date;
            }

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = decimal.Parse(rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
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
            if (rinoIznosTextBox.Text.Length > 0)
            {

                // Creating new RinoObligationItem object
                RinoObligationItem roi = new RinoObligationItem(
                actionType,
                iznos,
                nazivPoveriocaTextBox.Text,
                pibTextBox.Text,
                mbTextBox.Text,
                vrstaPoverioca,
                nazivDokumentaTextBox.Text,
                brojDokumentaTextBox.Text,
                datumDokumentaDateTimePicker.Value.Date,
                datumNastankaDateTimePicker.Value.Date,
                dueDate,
                razlogIzmeneTextBox.Text
                );

                // Checking for data validity
                if (roi.IsPibValid())
                {
                    if (roi.IsMbValid())
                    {
                        if (roi.Action == RinoActionType.Izmena || roi.Action == RinoActionType.Otkazivanje)
                        {
                            if (roi.ReasonForChangeValid())
                            {
                                if (roi.CheckForGeneralValidity())
                                {
                                    // Creating RinoObligationManager object
                                    RinoObligationManager rom = new RinoObligationManager();

                                    // Inserting new item and setting new value to roiList object
                                    roiList = rom.ModifyExistingItem(roiList, roi, rowNumber);

                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(roiList);

                                    // Setting DataSource property to dataGridView object
                                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
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
                            if (roi.CheckForGeneralValidity())
                            {
                                // Creating RinoObligationManager object
                                RinoObligationManager rom = new RinoObligationManager();

                                // Inserting new item and setting new value to roiList object
                                roiList = rom.ModifyExistingItem(roiList, roi, rowNumber);

                                // Converting roiList to DataTable object to display data
                                rom.ConvertRinoListToDataTable(roiList);

                                // Setting DataSource property to dataGridView object
                                rinoObligationDataGridView.DataSource = rom.RinoDataTable;
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
                rowNumber = 0;

                // Hide save obligation changes button
                saveObligationChangesBtn.Visible = false;

                // Reset fields
                ResetObligationFields();
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
            if (roiList.Capacity > 0)
            {
                // Get row position
                rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;

                // Creating RinoObligationManager object
                RinoObligationManager rom = new RinoObligationManager();

                // Declaring RinoObligationItem object
                RinoObligationItem roi;

                // Getting RIO object
                roi = rom.GetRoiItemAt(roiList, rowNumber);

                // Setting values to all UI fields
                // Setting action type
                switch (roi.Action)
                {
                    case RinoActionType.Unos:
                        rinoActionTypeComboBox.SelectedIndex = 0;
                        break;
                    case RinoActionType.Izmena:
                        rinoActionTypeComboBox.SelectedIndex = 1;
                        break;
                    case RinoActionType.Otkazivanje:
                        rinoActionTypeComboBox.SelectedIndex = 2;
                        break;
                }

                // Setting iznos
                rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");

                // Setting NazivPoverioca
                nazivPoveriocaTextBox.Text = roi.NazivPoverioca;

                // Setting PIBPoverioca
                pibTextBox.Text = roi.PIBPoverioca;

                // Setting MBPoverioca
                mbTextBox.Text = roi.MBPoverioca;

                // Setting VrstaPoverioca
                switch (roi.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        vrstaPoveriocaComboBox.SelectedIndex = 0;
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        vrstaPoveriocaComboBox.SelectedIndex = 1;
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        vrstaPoveriocaComboBox.SelectedIndex = 2;
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        vrstaPoveriocaComboBox.SelectedIndex = 3;
                        break;
                }

                // Setting NazivDokumenta
                nazivDokumentaTextBox.Text = roi.NazivDokumenta;

                // Setting BrojDokumenta
                brojDokumentaTextBox.Text = roi.BrojDokumenta;

                // Setting DatumDokumenta
                datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;

                // Setting DatumNastanka
                datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                // Setting DatumRokaZaIzmirenje
                if (!rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje))
                {
                    // Unchecking check box
                    noDueDateCheckBox.Checked = false;
                    // Enabling DateTimePicker
                    datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    // Setting DatumRokaZaIzmirenje
                    datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    // Checking check box
                    noDueDateCheckBox.Checked = true;
                    // Disabling DateTimePicker
                    datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                // Setting RazlogIzmene
                if (roi.RazlogIzmene != null && roi.RazlogIzmene.Length > 0)
                {
                    razlogIzmeneTextBox.Text = roi.RazlogIzmene;
                }
                else
                {
                    razlogIzmeneTextBox.Text = null;
                }
            }
        }

        #endregion

        #region Load reconcilement XML region

        private void loadReconcilementXmlBtn_Click(object sender, EventArgs e)
        {
            // Clearing existing path if program is already running
            openReconcilementXmlDialog.FileName = "";

            if (openReconcilementXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                string xmlPath = openReconcilementXmlDialog.FileName;

                // Load RINO XML file into memory
                // Creating RINO import object
                IRinoImport rinoReconcilementXmlImport = new RinoXmlImport();

                // Creating RinoObligationManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Populating roiList object
                rriList = rrm.ConvertXmlToRinoList(rinoReconcilementXmlImport.ImportRinoObligationXml(xmlPath));

                // Checking if XML file is valid reconcilement type
                if (rrm.ValidReconcilement)
                {
                    // Converting ROI list to DataTable for display
                    rrm.ConvertRinoListToDataTable(rriList);

                    // Setting data to datagrid
                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                }
                else
                {
                    // Display error message
                    string errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za razduženje.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset fields
                ResetReconcilementFields();
            }
        }

        #endregion

        #region Save reconcilement XML file region

        private void saveReconcilementXmlBtn_Click(object sender, EventArgs e)
        {
            if (CheckIfJbbkSettingsExist())
            {
                if (rriList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    saveReconcilementXmlDialog.FileName = "";

                    if (saveReconcilementXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        // RINO XML path
                        string xmlPath = saveReconcilementXmlDialog.FileName;

                        // Getting JBBK number from settings file
                        string jbbk = Properties.Settings.Default.jbbk;

                        // Creating RinoReconcilementManager object
                        RinoReconcilementManager rrm = new RinoReconcilementManager();

                        // Setting JBBK number
                        rrm.Jbbk = jbbk;

                        // Creating RinoXmlExport object
                        IRinoExporter rinoXmlExport = new RinoXmlExport();

                        // Saving file to filesystem
                        rinoXmlExport.ExportRinoObligationXml(rrm.ConvertRinoListToXml(rriList),
                            xmlPath);

                        // Reset fields
                        ResetReconcilementFields();
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
            RinoActionType actionType = GetRinoActionType();

            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = decimal.Parse(reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
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

            if (reconIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoReconcilementItem object
                RinoReconcilementItem rri = new RinoReconcilementItem(
                    actionType,
                    Convert.ToInt64(rinoIdTextBox.Text),
                    reconBrojDokumentaTextBox.Text,
                    reconPibTextBox.Text,
                    reconBankTextBox.Text,
                    reconPodZaReklTextBox.Text,
                    reconDatumIzmirenjaDateTimePicker.Value.Date,
                    iznos,
                    razlogIzmeneTextBox.Text
                    );

                // Checking for data validity
                if (rri.IsPibValid())
                {
                    if (rri.IsReklPodZaRekValid())
                    {
                        if (rri.Action == RinoActionType.Izmena || rri.Action == RinoActionType.Otkazivanje)
                        {
                            if (rri.ReasonForChangeValid())
                            {
                                if (rri.CheckForGeneralValidity())
                                {
                                    // Creating RinoReconcilementManager object
                                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                                    // Inserting new item and setting new value to rriList object
                                    rriList = rrm.InsertNewItem(rriList, rri);

                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(rriList);

                                    // Setting DataSource property to dataGridView object
                                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
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
                            if (rri.CheckForGeneralValidity())
                            {
                                // Creating RinoReconcilementManager object
                                RinoReconcilementManager rrm = new RinoReconcilementManager();

                                // Inserting new item and setting new value to rriList object
                                rriList = rrm.InsertNewItem(rriList, rri);

                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(rriList);

                                // Setting DataSource property to dataGridView object
                                rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
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
                reconRowNumber = 0;

                // Hiding insert as new reconcilation button
                insertAsNewReconBtn.Visible = false;

                // Reset fields
                ResetReconcilementFields();
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
            RinoActionType actionType = GetRinoActionType();
            // Iznos variable
            decimal iznos = 0;

            try
            {
                // Trying to parse value from text box
                iznos = Decimal.Parse(reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
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

            if (reconIznosTextBox.Text.Length > 0)
            {
                // Creating new RinoReconcilementItem object
                RinoReconcilementItem rri = new RinoReconcilementItem(
                    actionType,
                    Int64.Parse(rinoIdTextBox.Text),
                    reconBrojDokumentaTextBox.Text,
                    reconPibTextBox.Text,
                    reconBankTextBox.Text,
                    reconPodZaReklTextBox.Text,
                    reconDatumIzmirenjaDateTimePicker.Value.Date,
                    iznos,
                    razlogIzmeneTextBox.Text
                    );

                // Checking for data validity
                if (rri.IsPibValid())
                {
                    if (rri.IsReklPodZaRekValid())
                    {
                        if (rri.Action == RinoActionType.Izmena || rri.Action == RinoActionType.Otkazivanje)
                        {
                            if (rri.ReasonForChangeValid())
                            {
                                if (rri.CheckForGeneralValidity())
                                {
                                    // Creating RinoReconcilementManager object
                                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                                    // Inserting new item and setting new value to rriList object
                                    rriList = rrm.ModifyExistingItem(rriList, rri, reconRowNumber);

                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(rriList);

                                    // Setting DataSource property to dataGridView object
                                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
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
                            if (rri.CheckForGeneralValidity())
                            {
                                // Creating RinoReconcilementManager object
                                RinoReconcilementManager rrm = new RinoReconcilementManager();

                                // Inserting new item and setting new value to rriList object
                                rriList = rrm.ModifyExistingItem(rriList, rri, reconRowNumber);

                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(rriList);

                                // Setting DataSource property to dataGridView object
                                rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
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
                saveReconcilementChangesBtn.Visible = false;

                // Setting row index to 0
                reconRowNumber = 0;

                // Reset fields
                ResetReconcilementFields();
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
            if (rriList.Capacity > 0)
            {
                // Get row position
                reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;

                // Creating RinoReconcilementManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Declaring RinoObligationItem object
                RinoReconcilementItem rri;

                // Getting RRI object
                rri = rrm.GetRriItemAt(rriList, reconRowNumber);

                // Setting values to all UI fields
                // Setting action type
                switch (rri.Action)
                {
                    case RinoActionType.Unos:
                        reconRinoActionTypeComboBox.SelectedIndex = 0;
                        break;
                    case RinoActionType.Izmena:
                        reconRinoActionTypeComboBox.SelectedIndex = 1;
                        break;
                    case RinoActionType.Otkazivanje:
                        reconRinoActionTypeComboBox.SelectedIndex = 2;
                        break;
                }

                // Setting RINO ID
                rinoIdTextBox.Text = rri.RinoId.ToString();

                // Setting PIBPoverioca
                reconPibTextBox.Text = rri.PIBPoverioca;

                // Setting BrojDokumenta
                reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;

                // Setting Banka
                reconBankTextBox.Text = rri.Banka;

                // Setting ReklPodZaRek
                reconPodZaReklTextBox.Text = rri.ReklPodZaRek;

                // Setting DatumIzmirenja
                if (!rrm.CheckUninitializedDate(rri.DatumIzmirenja))
                {
                    // Setting DatumIzmirenja
                    reconDatumIzmirenjaDateTimePicker.Value = rri.DatumIzmirenja;
                }
                else
                {
                    // Default uninitialized value detected, put today as main date
                    reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
                }

                // Setting iznos
                reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));

                // Setting RazlogIzmene
                if (rri.RazlogIzmene != null && rri.RazlogIzmene.Length > 0)
                {
                    razlogIzmeneTextBox.Text = rri.RazlogIzmene;
                }
                else
                {
                    razlogIzmeneTextBox.Text = null;
                }

                // Enabling save reconcilement changes button
                saveReconcilementChangesBtn.Visible = true;
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
            if (roiList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;

                    // Creating RinoReconcilementManager object
                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                    // Remove item at selected index position
                    rriList = rrm.RemoveItemAt(rriList, reconRowNumber);

                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(rriList);

                    // Setting DataSource property to dataGridView object
                    rinoObligationDataGridView.DataSource = rrm.RinoDataTable;

                    // Setting row index to 0
                    reconRowNumber = 0;

                    // Reset fields
                    ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Clone reconcilement region

        private void cloneReconcilementBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal roiList object is not empty
            if (rriList.Capacity > 0)
            {
                // Get row position
                reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;

                // Creating RinoReconcilementManager object
                RinoReconcilementManager rrm = new RinoReconcilementManager();

                // Declaring RinoObligationItem object
                RinoReconcilementItem rri;

                // Getting RRI object
                rri = rrm.GetRriItemAt(rriList, reconRowNumber);

                // Setting values to all UI fields
                // Setting action type
                switch (rri.Action)
                {
                    case RinoActionType.Unos:
                        reconRinoActionTypeComboBox.SelectedIndex = 0;
                        break;
                    case RinoActionType.Izmena:
                        reconRinoActionTypeComboBox.SelectedIndex = 1;
                        break;
                    case RinoActionType.Otkazivanje:
                        reconRinoActionTypeComboBox.SelectedIndex = 2;
                        break;
                }

                // Setting RINO ID
                rinoIdTextBox.Text = rri.RinoId.ToString();

                // Setting PIBPoverioca
                reconPibTextBox.Text = rri.PIBPoverioca;

                // Setting BrojDokumenta
                reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;

                // Setting Banka
                reconBankTextBox.Text = rri.Banka;

                // Setting ReklPodZaRek
                reconPodZaReklTextBox.Text = rri.ReklPodZaRek;

                // Setting DatumIzmirenja
                if (!rrm.CheckUninitializedDate(rri.DatumIzmirenja))
                {
                    // Setting DatumIzmirenja
                    reconDatumIzmirenjaDateTimePicker.Value = rri.DatumIzmirenja;
                }
                else
                {
                    // Default uninitialized value detected, put today as main date
                    reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
                }
                // Setting iznos
                reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));

                // Setting RazlogIzmene
                if (rri.RazlogIzmene != null && rri.RazlogIzmene.Length > 0)
                {

                    razlogIzmeneTextBox.Text = rri.RazlogIzmene;
                }
                else
                {
                    razlogIzmeneTextBox.Text = null;
                }

                // Enabling save reconcilement changes button
                insertAsNewReconBtn.Visible = true;
            }
        }

        #endregion

        #region Remove empty statements region

        private void removeUnpopReconBtn_Click(object sender, EventArgs e)
        {

            // Checking if internal rriList object is not empty
            if (rriList.Capacity > 0)
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

                    foreach (RinoReconcilementItem rriItem in rriList)
                    {
                        // Checking if requested fields are NOT empty
                        if (rriItem.Banka.Length > 0 && rriItem.ReklPodZaRek.Length > 0
                            && !rrm.CheckUninitializedDate(rriItem.DatumIzmirenja) &&
                            rriItem.Iznos > 0)
                        {
                            // Add RRI item to list
                            fullRriList.Add(rriItem);
                        }
                    }

                    // Set full clean list as new main list
                    rriList = fullRriList;

                    // Performing conversion
                    rrm.ConvertRinoListToDataTable(rriList);

                    // Displaying new "clean" list
                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;

                    // Reset fields
                    ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Removing all reconcilements from list region

        private void removeAllReconcilementsBtn_Click(object sender, EventArgs e)
        {
            // Checking if internal rriList object is not empty
            if (rriList.Capacity > 0)
            {
                DialogResult diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Check if user clicked yes
                if (diagResult == DialogResult.Yes)
                {
                    // Clearing roiList object
                    rriList.Clear();

                    // Creating RinoReconcilementManager object
                    RinoReconcilementManager rrm = new RinoReconcilementManager();

                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(rriList);

                    // Setting DataSource property to dataGridView object
                    rinoObligationDataGridView.DataSource = rrm.RinoDataTable;

                    // Reset fields
                    ResetReconcilementFields();
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
            if (startMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        #endregion

        #region FormClosing event region

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Saving windows size
            if (WindowState == FormWindowState.Maximized)
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
            rinoActionTypeComboBox.SelectedIndex = -1;
            rinoIznosTextBox.Text = "";
            nazivPoveriocaTextBox.Text = "";
            pibTextBox.Text = "";
            mbTextBox.Text = "";
            vrstaPoveriocaComboBox.SelectedIndex = -1;
            nazivDokumentaTextBox.Text = "";
            brojDokumentaTextBox.Text = "";
            datumDokumentaDateTimePicker.Value = DateTime.Now;
            datumNastankaDateTimePicker.Value = DateTime.Now;
            datumRokaIzmirenjaDateTimePicker.Value = DateTime.Now;
            datumRokaIzmirenjaDateTimePicker.Enabled = true;
            noDueDateCheckBox.Checked = false;
            razlogIzmeneTextBox.Text = "";
        }

        private void ResetReconcilementFields()
        {
            reconRinoActionTypeComboBox.SelectedIndex = -1;
            rinoIdTextBox.Text = "";
            reconBrojDokumentaTextBox.Text = "";
            reconPibTextBox.Text = "";
            reconBankTextBox.Text = "";
            reconPodZaReklTextBox.Text = "";
            // DatumIzmirenjaDateTimePicker is not reseted by design, because of faster reconcilation times
            // reconDatumIzmirenjaDateTimePicker.Value = DateTime.Now;
            reconIznosTextBox.Text = "";
            reconRazlogIzmeneTextBox.Text = "";
        }

        #endregion

        #region JBBK info check region

        private bool CheckIfJbbkSettingsExist()
        {
            if (Properties.Settings.Default.jbbk.Length > 3)
            {
                return true;
            }

            return false;
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
