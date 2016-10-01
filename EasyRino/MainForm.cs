using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        /// Internal row number field.
        /// </summary>
        private int rowNumber = 0;

        public MainForm()
        {
            InitializeComponent();
        }

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

        private void jbbkConfigLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EasyRinoConfigForm jbbkConfigForm = new EasyRinoConfigForm();
            jbbkConfigForm.ShowDialog();
        }

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

                // Enabling save obligation changes button
                this.saveObligationChangesBtn.Visible = true;
            }
            else
            {
                // Do nothing or possibly pop a message box :)
            }

        }

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

        private void saveObligationXmlBtn_Click(object sender, EventArgs e)
        {
            if (this.saveObligationXmlDialog.ShowDialog() == DialogResult.OK)
            {
                // RINO XML path
                string xmlPath = this.saveObligationXmlDialog.FileName;

                string jbbk = Properties.Settings.Default.jbbk;

                RinoObligationManager rom = new RinoObligationManager();

                rom.Jbbk = jbbk;

                IRinoExporter rinoXmlExport = new RinoXmlExport();

                rinoXmlExport.ExportRinoObligationXml(rom.ConvertRinoListToXml(this.roiList),
                    xmlPath);
            }
        }

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
    }
}
