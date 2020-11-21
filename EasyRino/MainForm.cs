/*
 *  Main form class of EasyRino
 *  Copyright (C) 2016 - 2020 Dusan Misic <promisic@gmail.com>
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
using GriffinSoft.EasyRino.Properties;
using GriffinSoft.EasyRino.RinoCore;
using GriffinSoft.EasyRino.RinoXmlFilter;

namespace GriffinSoft.EasyRino
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Setting version information in the tittle bar
            Text = $"{Text} {GetEasyRinoVersion()}";
        }

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

        #region Load obligation XML region

        private void loadObligationXmlBtn_Click(object sender, EventArgs e)
        {
            // Clearing existing path if program is already running
            openObligationXmlDialog.FileName = "";

            if (openObligationXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                var xmlPath = openObligationXmlDialog.FileName;

                // Load RINO XML file into memory
                // Creating RINO import object
                IRinoImport rinoObligationXmlImport = new RinoXmlImport();

                // Creating RinoObligationManager object
                var rom = new RinoObligationManager();

                // Populating roiList object
                _roiList = rom.ConvertXmlToRinoList(rinoObligationXmlImport.ImportRinoObligationXml(xmlPath));

                // Checking if XML file is valid reconcilement type
                if (rom.ValidObligation)
                {
                    // Converting ROI list to DataTable for display
                    rom.ConvertRinoListToDataTable(_roiList);

                    // Setting data to DataGrid
                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                }
                else
                {
                    // Display error message
                    var errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za zaduženje.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset fields
                ResetObligationFields();
            }
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
            var jbbkConfigForm = new EasyRinoConfigForm();
            jbbkConfigForm.ShowDialog();
        }

        #endregion

        #region Modify obligation region

        private void modifyObligationBtn_Click(object sender, EventArgs e)
        {
            if (_roiList.Capacity > 0)
            {
                // Get row position
                _rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;
                var rom = new RinoObligationManager();
                var roi = rom.GetRoiItemAt(_roiList, _rowNumber);

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

                rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");
                nazivPoveriocaTextBox.Text = roi.NazivPoverioca;
                pibTextBox.Text = roi.PibPoverioca;
                mbTextBox.Text = roi.MbPoverioca;

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

                nazivDokumentaTextBox.Text = roi.NazivDokumenta;
                brojDokumentaTextBox.Text = roi.BrojDokumenta;
                datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;
                datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                if (!rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje))
                {
                    noDueDateCheckBox.Checked = false;
                    datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    noDueDateCheckBox.Checked = true;
                    datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                razlogIzmeneTextBox.Text = !string.IsNullOrEmpty(roi.RazlogIzmene) ? roi.RazlogIzmene : null;
                saveObligationChangesBtn.Visible = true;
            }
        }

        #endregion

        #region Delete obligation region

        private void deleteObligationBtn_Click(object sender, EventArgs e)
        {
            if (_roiList.Capacity > 0)
            {
                var diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    _rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;
                    var rom = new RinoObligationManager();
                    _roiList = rom.RemoveItemAt(_roiList, _rowNumber);
                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(_roiList);
                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                    // Setting row index to 0
                    _rowNumber = 0;
                    // Reset fields
                    ResetObligationFields();
                }
            }
        }

        #endregion

        #region Delete all obligations region

        private void deleteAllObligationsBtn_Click(object sender, EventArgs e)
        {
            if (_roiList.Capacity > 0)
            {
                var diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (diagResult == DialogResult.Yes)
                {
                    _roiList.Clear();
                    var rom = new RinoObligationManager();
                    // Converting roiList to DataTable object to display data
                    rom.ConvertRinoListToDataTable(_roiList);
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
                if (_roiList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    saveObligationXmlDialog.FileName = "";

                    if (saveObligationXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        var xmlPath = saveObligationXmlDialog.FileName;
                        var jbbk = Settings.Default.jbbk;
                        var rom = new RinoObligationManager { Jbbk = jbbk };
                        IRinoExporter rinoXmlExport = new RinoXmlExport();
                        rinoXmlExport.ExportRinoObligationXml(rom.ConvertRinoListToXml(_roiList),
                            xmlPath);
                        // Reset fields
                        ResetObligationFields();
                    }
                }
                else
                {
                    MessageBox.Show("Nije moguće snimanje prazne liste obaveza.", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    "Niste uneli Vaš JBBK u program. Molimo Vas, unesite ga da bi Vam bilo omogućeno snimanje.",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Save obligation changes region

        private void saveObligationChangesBtn_Click(object sender, EventArgs e)
        {
            var actionType = GetRinoActionType();
            var vrstaPoverioca = GetRinoVrstaPoverioca();

            var dueDate = new DateTime();
            if (!noDueDateCheckBox.Checked) dueDate = datumRokaIzmirenjaDateTimePicker.Value.Date;

            decimal iznos;

            try
            {
                iznos = decimal.Parse(rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                var errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                             "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            if (rinoIznosTextBox.Text.Length > 0)
            {
                var roi = new RinoObligationItem(
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
                                    var rom = new RinoObligationManager();
                                    _roiList = rom.ModifyExistingItem(_roiList, roi, _rowNumber);
                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(_roiList);
                                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                }
                                else
                                {
                                    var errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                var errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (roi.CheckForGeneralValidity())
                            {
                                var rom = new RinoObligationManager();
                                _roiList = rom.ModifyExistingItem(_roiList, roi, _rowNumber);
                                // Converting roiList to DataTable object to display data
                                rom.ConvertRinoListToDataTable(_roiList);
                                rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                            }
                            else
                            {
                                var errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        var errMsg = "Niste uneli validan MB.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Setting row index to 0
                _rowNumber = 0;
                saveObligationChangesBtn.Visible = false;
                // Reset fields
                ResetObligationFields();
            }
            else
            {
                var errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Clone obligation region

        private void cloneObliationBtn_Click(object sender, EventArgs e)
        {
            if (_roiList.Capacity > 0)
            {
                // Get row position
                _rowNumber = rinoObligationDataGridView.CurrentCellAddress.Y;
                var rom = new RinoObligationManager();
                var roi = rom.GetRoiItemAt(_roiList, _rowNumber);

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

                rinoIznosTextBox.Text = roi.Iznos.ToString("#.##");
                nazivPoveriocaTextBox.Text = roi.NazivPoverioca;
                pibTextBox.Text = roi.PibPoverioca;
                mbTextBox.Text = roi.MbPoverioca;

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

                nazivDokumentaTextBox.Text = roi.NazivDokumenta;
                brojDokumentaTextBox.Text = roi.BrojDokumenta;
                datumDokumentaDateTimePicker.Value = roi.DatumDokumenta;
                datumNastankaDateTimePicker.Value = roi.DatumNastanka;

                if (!rom.CheckUninitializedDate(roi.DatumRokaZaIzmirenje))
                {
                    noDueDateCheckBox.Checked = false;
                    datumRokaIzmirenjaDateTimePicker.Enabled = true;
                    datumRokaIzmirenjaDateTimePicker.Value = roi.DatumRokaZaIzmirenje;
                }
                else
                {
                    noDueDateCheckBox.Checked = true;
                    datumRokaIzmirenjaDateTimePicker.Enabled = false;
                }

                razlogIzmeneTextBox.Text = !string.IsNullOrEmpty(roi.RazlogIzmene) ? roi.RazlogIzmene : null;
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
                var xmlPath = openReconcilementXmlDialog.FileName;
                IRinoImport rinoReconcilementXmlImport = new RinoXmlImport();
                var rrm = new RinoReconcilementManager();
                _rriList = rrm.ConvertXmlToRinoList(rinoReconcilementXmlImport.ImportRinoReconcilementXml(xmlPath));

                if (rrm.ValidReconcilement)
                {
                    // Converting ROI list to DataTable for display
                    rrm.ConvertRinoListToDataTable(_rriList);
                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                }
                else
                {
                    var errMsg = "XML fajl koji ste izabrali nije validan RINO XML fajl za razduženje.";
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
                if (_rriList.Capacity > 0)
                {
                    // Clearing existing path if program is already running
                    saveReconcilementXmlDialog.FileName = "";

                    if (saveReconcilementXmlDialog.ShowDialog() == DialogResult.OK)
                    {
                        var xmlPath = saveReconcilementXmlDialog.FileName;
                        var jbbk = Settings.Default.jbbk;
                        var rrm = new RinoReconcilementManager { Jbbk = jbbk };
                        IRinoExporter rinoXmlExport = new RinoXmlExport();
                        rinoXmlExport.ExportRinoReconcilementXml(rrm.ConvertRinoListToXml(_rriList),
                            xmlPath);
                        // Reset fields
                        ResetReconcilementFields();
                    }
                }
                else
                {
                    MessageBox.Show("Nije moguće snimanje prazne liste razduženja.", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    "Niste uneli Vaš JBBK u program. Molimo Vas, unesite ga da bi Vam bilo omogućeno snimanje.",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Insert as new reconcilation region

        private void insertAsNewReconBtn_Click(object sender, EventArgs e)
        {
            var actionType = GetRinoActionType();
            decimal iznos;

            try
            {
                // Trying to parse value from text box
                iznos = decimal.Parse(reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                var errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                             "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Setting iznos to 1 din
                iznos = 1;
            }

            if (reconIznosTextBox.Text.Length > 0)
            {
                var rri = new RinoReconcilementItem(
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
                                    var rrm = new RinoReconcilementManager();
                                    _rriList = rrm.InsertNewItem(_rriList, rri);
                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(_rriList);
                                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                                }
                                else
                                {
                                    var errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                var errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (rri.CheckForGeneralValidity())
                            {
                                var rrm = new RinoReconcilementManager();
                                _rriList = rrm.InsertNewItem(_rriList, rri);
                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(_rriList);
                                rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                            }
                            else
                            {
                                var errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        var errMsg = "Niste uneli podatak za reklamaciju.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    var errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Setting row index to 0
                _reconRowNumber = 0;
                insertAsNewReconBtn.Visible = false;
                // Reset fields
                ResetReconcilementFields();
            }
            else
            {
                var errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Save reconcilement changes region

        private void saveReconcilementChangesBtn_Click(object sender, EventArgs e)
        {
            var actionType = GetRinoActionType();
            decimal iznos;

            try
            {
                // Trying to parse value from text box
                iznos = decimal.Parse(reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                var errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                             "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iznos = 1;
            }

            if (reconIznosTextBox.Text.Length > 0)
            {
                var rri = new RinoReconcilementItem(
                    actionType,
                    long.Parse(rinoIdTextBox.Text),
                    reconBrojDokumentaTextBox.Text,
                    reconPibTextBox.Text,
                    reconBankTextBox.Text,
                    reconPodZaReklTextBox.Text,
                    reconDatumIzmirenjaDateTimePicker.Value.Date,
                    iznos,
                    razlogIzmeneTextBox.Text
                );

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
                                    var rrm = new RinoReconcilementManager();
                                    _rriList = rrm.ModifyExistingItem(_rriList, rri, _reconRowNumber);
                                    // Converting rriList to DataTable object to display data
                                    rrm.ConvertRinoListToDataTable(_rriList);
                                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                                }
                                else
                                {
                                    var errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                var errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (rri.CheckForGeneralValidity())
                            {
                                var rrm = new RinoReconcilementManager();
                                _rriList = rrm.ModifyExistingItem(_rriList, rri, _reconRowNumber);
                                // Converting rriList to DataTable object to display data
                                rrm.ConvertRinoListToDataTable(_rriList);
                                rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                            }
                            else
                            {
                                var errMsg = "Niste uneli sve obavezne podatke";
                                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        var errMsg = "Niste uneli podatak za reklamaciju.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var errMsg = "Niste uneli validan PIB.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                saveReconcilementChangesBtn.Visible = false;
                // Setting row index to 0
                _reconRowNumber = 0;
                // Reset fields
                ResetReconcilementFields();
            }
            else
            {
                var errMsg = "Polje iznos ne može biti prazno.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Modify reconcilement region

        private void modifyReconcilementBtn_Click(object sender, EventArgs e)
        {
            if (_rriList.Capacity > 0)
            {
                _reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;
                var rrm = new RinoReconcilementManager();
                var rri = rrm.GetRriItemAt(_rriList, _reconRowNumber);

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

                rinoIdTextBox.Text = rri.RinoId.ToString();
                reconPibTextBox.Text = rri.PibPoverioca;
                reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;
                reconBankTextBox.Text = rri.Banka;
                reconPodZaReklTextBox.Text = rri.ReklPodZaRek;
                reconDatumIzmirenjaDateTimePicker.Value = !rrm.CheckUninitializedDate(rri.DatumIzmirenja)
                    ? rri.DatumIzmirenja
                    : DateTime.Now;
                reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                razlogIzmeneTextBox.Text = !string.IsNullOrEmpty(rri.RazlogIzmene) ? rri.RazlogIzmene : null;
                saveReconcilementChangesBtn.Visible = true;
            }
        }

        #endregion

        #region Remove selected reconcilement from list region

        private void deleteReconcilementBtn_Click(object sender, EventArgs e)
        {
            if (_roiList.Capacity > 0)
            {
                var diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sledeću stavku?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (diagResult == DialogResult.Yes)
                {
                    // Get row position
                    _reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;
                    var rrm = new RinoReconcilementManager();
                    // Remove item at selected index position
                    _rriList = rrm.RemoveItemAt(_rriList, _reconRowNumber);
                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(_rriList);
                    rinoObligationDataGridView.DataSource = rrm.RinoDataTable;
                    // Setting row index to 0
                    _reconRowNumber = 0;
                    // Reset fields
                    ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Clone reconcilement region

        private void cloneReconcilementBtn_Click(object sender, EventArgs e)
        {
            if (_rriList.Capacity > 0)
            {
                // Get row position
                _reconRowNumber = rinoReconcilementDataGridView.CurrentCellAddress.Y;
                var rrm = new RinoReconcilementManager();
                var rri = rrm.GetRriItemAt(_rriList, _reconRowNumber);

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

                rinoIdTextBox.Text = rri.RinoId.ToString();
                reconPibTextBox.Text = rri.PibPoverioca;
                reconBrojDokumentaTextBox.Text = rri.BrojDokumenta;
                reconBankTextBox.Text = rri.Banka;
                reconPodZaReklTextBox.Text = rri.ReklPodZaRek;
                reconDatumIzmirenjaDateTimePicker.Value = !rrm.CheckUninitializedDate(rri.DatumIzmirenja)
                    ? rri.DatumIzmirenja
                    : DateTime.Now;
                reconIznosTextBox.Text = rri.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                razlogIzmeneTextBox.Text = !string.IsNullOrEmpty(rri.RazlogIzmene) ? rri.RazlogIzmene : null;
                insertAsNewReconBtn.Visible = true;
            }
        }

        #endregion

        #region Remove empty statements region

        private void removeUnpopReconBtn_Click(object sender, EventArgs e)
        {
            if (_rriList.Capacity > 0)
            {
                var diagResult = MessageBox.Show("Da li ste sigurni da želite da uklonite prazne stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (diagResult == DialogResult.Yes)
                {
                    var fullRriList = new List<RinoReconcilementItem>();
                    var rrm = new RinoReconcilementManager();

                    foreach (var rriItem in _rriList)
                        if (rriItem.Banka.Length > 0 && rriItem.ReklPodZaRek.Length > 0
                                                     && !rrm.CheckUninitializedDate(rriItem.DatumIzmirenja) &&
                                                     rriItem.Iznos > 0)
                            fullRriList.Add(rriItem);

                    _rriList = fullRriList;
                    // Performing conversion
                    rrm.ConvertRinoListToDataTable(_rriList);
                    rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
                    ResetReconcilementFields();
                }
            }
        }

        #endregion

        #region Removing all reconcilements from list region

        private void removeAllReconcilementsBtn_Click(object sender, EventArgs e)
        {
            if (_rriList.Capacity > 0)
            {
                var diagResult = MessageBox.Show("Da li ste sigurni da želite da obrišete sve stavke?",
                    "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (diagResult == DialogResult.Yes)
                {
                    _rriList.Clear();
                    var rrm = new RinoReconcilementManager();
                    // Converting rriList to DataTable object to display data
                    rrm.ConvertRinoListToDataTable(_rriList);
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
            var startMaximized = Settings.Default.startMaximized;
            WindowState = startMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        #endregion

        #region FormClosing event region

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Saving windows size
            if (WindowState == FormWindowState.Maximized)
            {
                Settings.Default.startMaximized = true;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.startMaximized = false;
                Settings.Default.Save();
            }
        }

        #endregion

        #region JBBK info check region

        private bool CheckIfJbbkSettingsExist()
        {
            if (Settings.Default.jbbk.Length > 3) return true;

            return false;
        }

        #endregion

        #region About form click region

        private void aboutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var aboutForm = new EasyRinoAboutForm();
            aboutForm.ShowDialog();
        }

        #endregion

        #region Internal lists of RINO obligation and reconcilement objects region

        /// <summary>
        /// Internal list of RinoObligationItem's
        /// </summary>
        private List<RinoObligationItem> _roiList = new List<RinoObligationItem>();

        /// <summary>
        /// Internal list of RinoReconcilementItem's.
        /// </summary>
        private List<RinoReconcilementItem> _rriList = new List<RinoReconcilementItem>();

        #endregion

        #region Internal row number index field's region

        /// <summary>
        /// Internal row number field.
        /// </summary>
        private int _rowNumber;

        /// <summary>
        /// Internal reconciliation row number field.
        /// </summary>
        private int _reconRowNumber;

        #endregion

        #region Insert new obligation region

        private void insertNewObligationBtn_Click(object sender, EventArgs e)
        {
            var actionType = GetRinoActionType();
            var vrstaPoverioca = GetRinoVrstaPoverioca();
            var dueDate = new DateTime();
            if (noDueDateCheckBox.Checked == false) dueDate = datumRokaIzmirenjaDateTimePicker.Value.Date;
            decimal iznos;

            try
            {
                iznos = decimal.Parse(rinoIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                var errMsg = "Podatak koji ste uneli kao iznos nije validan broj ili nije u validnom formatu. \n\n" +
                             "Iznos će biti postavljen na 1 din. Na Vama je da izmenite iznos kroz komandu za izmenu stavke.";
                MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iznos = 1;
            }

            if (rinoIznosTextBox.Text.Length > 0)
            {
                var roi = new RinoObligationItem(
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

                var roiDoesNotAlreadyExist = true;

                foreach (var roiItem in _roiList)
                    if (roi.PibPoverioca == roiItem.PibPoverioca &&
                        roi.BrojDokumenta == roiItem.BrojDokumenta)
                    {
                        roiDoesNotAlreadyExist = false;
                        break;
                    }

                if (roiDoesNotAlreadyExist)
                {
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
                                        var rom = new RinoObligationManager();
                                        _roiList = rom.InsertNewItem(_roiList, roi);
                                        // Converting roiList to DataTable object to display data
                                        rom.ConvertRinoListToDataTable(_roiList);
                                        rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                    }
                                    else
                                    {
                                        var errMsg = "Niste uneli sve obavezne podatke";
                                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    var errMsg = "Morate uneti razlog izmene ili otkazivanja.";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                if (roi.CheckForGeneralValidity())
                                {
                                    var rom = new RinoObligationManager();
                                    _roiList = rom.InsertNewItem(_roiList, roi);
                                    // Converting roiList to DataTable object to display data
                                    rom.ConvertRinoListToDataTable(_roiList);
                                    rinoObligationDataGridView.DataSource = rom.RinoDataTable;
                                }
                                else
                                {
                                    var errMsg = "Niste uneli sve obavezne podatke";
                                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            var errMsg = "Niste uneli validan MB.";
                            MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        var errMsg = "Niste uneli validan PIB.";
                        MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    _rowNumber = 0;
                    // Reset fields
                    ResetObligationFields();
                }
                else
                {
                    var errMsg = "Već ste uneli jedno zaduženje koje ima isti PIB i broj računa u listu obaveza.";
                    MessageBox.Show(errMsg, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var errMsg = "Polje iznos ne može biti prazno.";
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
            reconIznosTextBox.Text = "";
            reconRazlogIzmeneTextBox.Text = "";
        }

        #endregion
    }
}