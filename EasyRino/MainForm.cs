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
        /// <summary>
        /// Internal list of RinoObligationItem's
        /// </summary>
        private List<RinoObligationItem> roiList = new List<RinoObligationItem>();

        /// <summary>
        /// Internal list of RinoReconcilementItem's.
        /// </summary>
        private List<RinoReconcilementItem> rriList = new List<RinoReconcilementItem>();

        /// <summary>
        /// Internal row number field.
        /// </summary>
        private int rowNumber = 0;

        /// <summary>
        /// Internal reconcilation row number field.
        /// </summary>
        private int reconRowNumber = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Load obligation XML region

        private void loadObligationXmlBtn_Click(object sender, EventArgs e)
        {
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

                // Converting ROI list to DataTable for display
                rom.ConvertRinoListToDataTable(roiList);

                // Setting data to datagrid
                this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;
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

            // Creating new RinoObligationItem object
            RinoObligationItem roi = new RinoObligationItem(
                actionType,
                Convert.ToDecimal(this.rinoIznosTextBox.Text),
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

            // TODO: Implement data checking capability here


            // Creating RinoObligationManager object
            RinoObligationManager rom = new RinoObligationManager();

            // Inserting new item and setting new value to roiList object
            this.roiList = rom.InsertNewItem(this.roiList, roi);

            // Converting roiList to DataTable object to display data
            rom.ConvertRinoListToDataTable(this.roiList);

            // Setting DataSource property to dataGridView object
            this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;

            // Setting row index to 0
            this.rowNumber = 0;
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
                }
            }
        }

        #endregion

        #region Save obligation XML file region

        private void saveObligationXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.roiList.Capacity > 0)
            {
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
                }
            }
            else
            {
                // TODO: Pop message box that there is nothing to save
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

            // Creating new RinoObligationItem object
            RinoObligationItem roi = new RinoObligationItem(
                actionType,
                Convert.ToDecimal(this.rinoIznosTextBox.Text),
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

            // TODO: Implement data checking capability here


            // Creating RinoObligationManager object
            RinoObligationManager rom = new RinoObligationManager();

            // Inserting new item and setting new value to roiList object
            this.roiList = rom.ModifyExistingItem(this.roiList, roi, this.rowNumber);

            // Converting roiList to DataTable object to display data
            rom.ConvertRinoListToDataTable(this.roiList);

            // Setting DataSource property to dataGridView object
            this.rinoObligationDataGridView.DataSource = rom.RinoDataTable;

            // Setting row index to 0
            this.rowNumber = 0;

            // Hide save obligation changes button
            this.saveObligationChangesBtn.Visible = false;
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

                // Converting ROI list to DataTable for display
                rrm.ConvertRinoListToDataTable(rriList);

                // Setting data to datagrid
                this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;
            }
        }

        #endregion

        #region Save reconcilement XML file region

        private void saveReconcilementXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.rriList.Capacity > 0)
            {
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
                }
            }
            else
            {
                // TODO: Pop message box that there is nothing to save
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

            // Creating new RinoReconcilementItem object
            RinoReconcilementItem rri = new RinoReconcilementItem(
                actionType,
                Convert.ToInt64(this.rinoIdTextBox.Text),
                this.reconBrojDokumentaTextBox.Text,
                this.reconPibTextBox.Text,
                this.reconBankTextBox.Text,
                this.reconPodZaReklTextBox.Text,
                this.reconDatumIzmirenjaDateTimePicker.Value.Date,
                Convert.ToDecimal(this.reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US")),
                this.razlogIzmeneTextBox.Text
                );

            // TODO: Implement data checking capability here


            // Creating RinoReconcilementManager object
            RinoReconcilementManager rrm = new RinoReconcilementManager();

            // Inserting new item and setting new value to rriList object
            this.rriList = rrm.InsertNewItem(this.rriList, rri);

            // Converting rriList to DataTable object to display data
            rrm.ConvertRinoListToDataTable(this.rriList);

            // Setting DataSource property to dataGridView object
            this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;

            // Setting row index to 0
            this.reconRowNumber = 0;

            // Hiding insert as new reconcilation button
            this.insertAsNewReconBtn.Visible = false;
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

            // Creating new RinoReconcilementItem object
            RinoReconcilementItem rri = new RinoReconcilementItem(
                actionType,
                Int64.Parse(this.rinoIdTextBox.Text),
                this.reconBrojDokumentaTextBox.Text,
                this.reconPibTextBox.Text,
                this.reconBankTextBox.Text,
                this.reconPodZaReklTextBox.Text,
                this.reconDatumIzmirenjaDateTimePicker.Value.Date,
                Convert.ToDecimal(this.reconIznosTextBox.Text, CultureInfo.GetCultureInfo("en-US")),
                this.razlogIzmeneTextBox.Text
                );

            // TODO: Implement data checking capability here


            // Creating RinoReconcilementManager object
            RinoReconcilementManager rrm = new RinoReconcilementManager();

            // Inserting new item and setting new value to rriList object
            this.rriList = rrm.ModifyExistingItem(this.rriList, rri, this.reconRowNumber);

            // Converting rriList to DataTable object to display data
            rrm.ConvertRinoListToDataTable(this.rriList);

            // Setting DataSource property to dataGridView object
            this.rinoReconcilementDataGridView.DataSource = rrm.RinoDataTable;

            // Hiding save reconcilation changes button
            this.saveReconcilementChangesBtn.Visible = false;

            // Setting row index to 0
            this.reconRowNumber = 0;
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
                }
            }
        }

        #endregion

    }
}
