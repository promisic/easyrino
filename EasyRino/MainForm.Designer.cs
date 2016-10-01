﻿namespace GriffinSoft.EasyRino
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rinoTabControl = new System.Windows.Forms.TabControl();
            this.zaduzenjaTabPage = new System.Windows.Forms.TabPage();
            this.modifyObligationBtn = new System.Windows.Forms.Button();
            this.deleteObligationBtn = new System.Windows.Forms.Button();
            this.deleteAllObligationsBtn = new System.Windows.Forms.Button();
            this.rinoObligationDataGridView = new System.Windows.Forms.DataGridView();
            this.zaduzenjaGroupBox = new System.Windows.Forms.GroupBox();
            this.noDueDateCheckBox = new System.Windows.Forms.CheckBox();
            this.datumRokaIzmirenjaLabel = new System.Windows.Forms.Label();
            this.datumRokaIzmirenjaDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.datumNastankaDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.datumDokumentaLabel = new System.Windows.Forms.Label();
            this.datumDokumentaDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.razlogIzmeneLabel = new System.Windows.Forms.Label();
            this.razlogIzmeneTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.brojDokumentaTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nazivDokumentaLabel = new System.Windows.Forms.Label();
            this.nazivDokumentaTextBox = new System.Windows.Forms.TextBox();
            this.vrstaPoveriocaComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mbTextBox = new System.Windows.Forms.TextBox();
            this.pibLabel = new System.Windows.Forms.Label();
            this.pibTextBox = new System.Windows.Forms.TextBox();
            this.nazivPoveriocaLabel = new System.Windows.Forms.Label();
            this.nazivPoveriocaTextBox = new System.Windows.Forms.TextBox();
            this.rinoIznosLabel = new System.Windows.Forms.Label();
            this.rinoIznosTextBox = new System.Windows.Forms.TextBox();
            this.rinoActionTypeLabel = new System.Windows.Forms.Label();
            this.rinoActionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.insertNewObligationBtn = new System.Windows.Forms.Button();
            this.loadObligationXmlBtn = new System.Windows.Forms.Button();
            this.saveObligationXmlBtn = new System.Windows.Forms.Button();
            this.razduzenjaTabPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.aboutLinkLabel = new System.Windows.Forms.LinkLabel();
            this.openObligationXmlDialog = new System.Windows.Forms.OpenFileDialog();
            this.jbbkConfigLinkLabel = new System.Windows.Forms.LinkLabel();
            this.saveObligationChangesBtn = new System.Windows.Forms.Button();
            this.cloneObliationBtn = new System.Windows.Forms.Button();
            this.saveObligationXmlDialog = new System.Windows.Forms.SaveFileDialog();
            this.rinoActionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoIznosColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoNazivPoveriocaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoPibColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoMbColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoVrstaPoveriocaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoNazivDokumentaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoBrojDokumentaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoDatumDokumentaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoDatumNastankaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoDatumRokaZaIzmirenjeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoRazlogZaIzmenuColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rinoTabControl.SuspendLayout();
            this.zaduzenjaTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rinoObligationDataGridView)).BeginInit();
            this.zaduzenjaGroupBox.SuspendLayout();
            this.razduzenjaTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rinoTabControl
            // 
            this.rinoTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.rinoTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rinoTabControl.Controls.Add(this.zaduzenjaTabPage);
            this.rinoTabControl.Controls.Add(this.razduzenjaTabPage);
            this.rinoTabControl.Location = new System.Drawing.Point(12, 25);
            this.rinoTabControl.Name = "rinoTabControl";
            this.rinoTabControl.SelectedIndex = 0;
            this.rinoTabControl.Size = new System.Drawing.Size(863, 461);
            this.rinoTabControl.TabIndex = 0;
            // 
            // zaduzenjaTabPage
            // 
            this.zaduzenjaTabPage.Controls.Add(this.cloneObliationBtn);
            this.zaduzenjaTabPage.Controls.Add(this.modifyObligationBtn);
            this.zaduzenjaTabPage.Controls.Add(this.deleteObligationBtn);
            this.zaduzenjaTabPage.Controls.Add(this.deleteAllObligationsBtn);
            this.zaduzenjaTabPage.Controls.Add(this.rinoObligationDataGridView);
            this.zaduzenjaTabPage.Controls.Add(this.zaduzenjaGroupBox);
            this.zaduzenjaTabPage.Location = new System.Drawing.Point(4, 4);
            this.zaduzenjaTabPage.Name = "zaduzenjaTabPage";
            this.zaduzenjaTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.zaduzenjaTabPage.Size = new System.Drawing.Size(855, 435);
            this.zaduzenjaTabPage.TabIndex = 0;
            this.zaduzenjaTabPage.Text = "Zaduženja";
            this.zaduzenjaTabPage.UseVisualStyleBackColor = true;
            // 
            // modifyObligationBtn
            // 
            this.modifyObligationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modifyObligationBtn.Location = new System.Drawing.Point(6, 229);
            this.modifyObligationBtn.Name = "modifyObligationBtn";
            this.modifyObligationBtn.Size = new System.Drawing.Size(115, 22);
            this.modifyObligationBtn.TabIndex = 4;
            this.modifyObligationBtn.Text = "Izmeni";
            this.modifyObligationBtn.UseVisualStyleBackColor = true;
            this.modifyObligationBtn.Click += new System.EventHandler(this.modifyObligationBtn_Click);
            // 
            // deleteObligationBtn
            // 
            this.deleteObligationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteObligationBtn.Location = new System.Drawing.Point(127, 229);
            this.deleteObligationBtn.Name = "deleteObligationBtn";
            this.deleteObligationBtn.Size = new System.Drawing.Size(115, 22);
            this.deleteObligationBtn.TabIndex = 3;
            this.deleteObligationBtn.Text = "Obriši";
            this.deleteObligationBtn.UseVisualStyleBackColor = true;
            this.deleteObligationBtn.Click += new System.EventHandler(this.deleteObligationBtn_Click);
            // 
            // deleteAllObligationsBtn
            // 
            this.deleteAllObligationsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAllObligationsBtn.Location = new System.Drawing.Point(734, 229);
            this.deleteAllObligationsBtn.Name = "deleteAllObligationsBtn";
            this.deleteAllObligationsBtn.Size = new System.Drawing.Size(115, 22);
            this.deleteAllObligationsBtn.TabIndex = 2;
            this.deleteAllObligationsBtn.Text = "Obriši sve";
            this.deleteAllObligationsBtn.UseVisualStyleBackColor = true;
            this.deleteAllObligationsBtn.Click += new System.EventHandler(this.deleteAllObligationsBtn_Click);
            // 
            // rinoObligationDataGridView
            // 
            this.rinoObligationDataGridView.AllowUserToAddRows = false;
            this.rinoObligationDataGridView.AllowUserToDeleteRows = false;
            this.rinoObligationDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rinoObligationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rinoObligationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rinoActionColumn,
            this.rinoIznosColumn,
            this.rinoNazivPoveriocaColumn,
            this.rinoPibColumn,
            this.rinoMbColumn,
            this.rinoVrstaPoveriocaColumn,
            this.rinoNazivDokumentaColumn,
            this.rinoBrojDokumentaColumn,
            this.rinoDatumDokumentaColumn,
            this.rinoDatumNastankaColumn,
            this.rinoDatumRokaZaIzmirenjeColumn,
            this.rinoRazlogZaIzmenuColumn});
            this.rinoObligationDataGridView.Location = new System.Drawing.Point(6, 6);
            this.rinoObligationDataGridView.Name = "rinoObligationDataGridView";
            this.rinoObligationDataGridView.RowHeadersVisible = false;
            this.rinoObligationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.rinoObligationDataGridView.Size = new System.Drawing.Size(843, 217);
            this.rinoObligationDataGridView.TabIndex = 1;
            // 
            // zaduzenjaGroupBox
            // 
            this.zaduzenjaGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zaduzenjaGroupBox.Controls.Add(this.noDueDateCheckBox);
            this.zaduzenjaGroupBox.Controls.Add(this.saveObligationChangesBtn);
            this.zaduzenjaGroupBox.Controls.Add(this.datumRokaIzmirenjaLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.datumRokaIzmirenjaDateTimePicker);
            this.zaduzenjaGroupBox.Controls.Add(this.label5);
            this.zaduzenjaGroupBox.Controls.Add(this.datumNastankaDateTimePicker);
            this.zaduzenjaGroupBox.Controls.Add(this.datumDokumentaLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.datumDokumentaDateTimePicker);
            this.zaduzenjaGroupBox.Controls.Add(this.razlogIzmeneLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.razlogIzmeneTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.label4);
            this.zaduzenjaGroupBox.Controls.Add(this.brojDokumentaTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.label3);
            this.zaduzenjaGroupBox.Controls.Add(this.nazivDokumentaLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.nazivDokumentaTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.vrstaPoveriocaComboBox);
            this.zaduzenjaGroupBox.Controls.Add(this.label2);
            this.zaduzenjaGroupBox.Controls.Add(this.mbTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.pibLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.pibTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.nazivPoveriocaLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.nazivPoveriocaTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.rinoIznosLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.rinoIznosTextBox);
            this.zaduzenjaGroupBox.Controls.Add(this.rinoActionTypeLabel);
            this.zaduzenjaGroupBox.Controls.Add(this.rinoActionTypeComboBox);
            this.zaduzenjaGroupBox.Controls.Add(this.insertNewObligationBtn);
            this.zaduzenjaGroupBox.Controls.Add(this.loadObligationXmlBtn);
            this.zaduzenjaGroupBox.Controls.Add(this.saveObligationXmlBtn);
            this.zaduzenjaGroupBox.Location = new System.Drawing.Point(6, 257);
            this.zaduzenjaGroupBox.Name = "zaduzenjaGroupBox";
            this.zaduzenjaGroupBox.Size = new System.Drawing.Size(843, 172);
            this.zaduzenjaGroupBox.TabIndex = 0;
            this.zaduzenjaGroupBox.TabStop = false;
            // 
            // noDueDateCheckBox
            // 
            this.noDueDateCheckBox.AutoSize = true;
            this.noDueDateCheckBox.Location = new System.Drawing.Point(205, 117);
            this.noDueDateCheckBox.Name = "noDueDateCheckBox";
            this.noDueDateCheckBox.Size = new System.Drawing.Size(136, 17);
            this.noDueDateCheckBox.TabIndex = 32;
            this.noDueDateCheckBox.Text = "Dokument nema valutu";
            this.noDueDateCheckBox.UseVisualStyleBackColor = true;
            this.noDueDateCheckBox.CheckedChanged += new System.EventHandler(this.noDueDateCheckBox_CheckedChanged);
            // 
            // datumRokaIzmirenjaLabel
            // 
            this.datumRokaIzmirenjaLabel.AutoSize = true;
            this.datumRokaIzmirenjaLabel.Location = new System.Drawing.Point(3, 99);
            this.datumRokaIzmirenjaLabel.Name = "datumRokaIzmirenjaLabel";
            this.datumRokaIzmirenjaLabel.Size = new System.Drawing.Size(122, 13);
            this.datumRokaIzmirenjaLabel.TabIndex = 31;
            this.datumRokaIzmirenjaLabel.Text = "Datum roka za izmirenje:";
            // 
            // datumRokaIzmirenjaDateTimePicker
            // 
            this.datumRokaIzmirenjaDateTimePicker.Location = new System.Drawing.Point(6, 115);
            this.datumRokaIzmirenjaDateTimePicker.Name = "datumRokaIzmirenjaDateTimePicker";
            this.datumRokaIzmirenjaDateTimePicker.Size = new System.Drawing.Size(173, 20);
            this.datumRokaIzmirenjaDateTimePicker.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(649, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Datum nastanka:";
            // 
            // datumNastankaDateTimePicker
            // 
            this.datumNastankaDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.datumNastankaDateTimePicker.Location = new System.Drawing.Point(652, 76);
            this.datumNastankaDateTimePicker.Name = "datumNastankaDateTimePicker";
            this.datumNastankaDateTimePicker.Size = new System.Drawing.Size(173, 20);
            this.datumNastankaDateTimePicker.TabIndex = 28;
            // 
            // datumDokumentaLabel
            // 
            this.datumDokumentaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.datumDokumentaLabel.AutoSize = true;
            this.datumDokumentaLabel.Location = new System.Drawing.Point(457, 60);
            this.datumDokumentaLabel.Name = "datumDokumentaLabel";
            this.datumDokumentaLabel.Size = new System.Drawing.Size(97, 13);
            this.datumDokumentaLabel.TabIndex = 27;
            this.datumDokumentaLabel.Text = "Datum dokumenta:";
            // 
            // datumDokumentaDateTimePicker
            // 
            this.datumDokumentaDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.datumDokumentaDateTimePicker.Location = new System.Drawing.Point(460, 76);
            this.datumDokumentaDateTimePicker.Name = "datumDokumentaDateTimePicker";
            this.datumDokumentaDateTimePicker.Size = new System.Drawing.Size(175, 20);
            this.datumDokumentaDateTimePicker.TabIndex = 26;
            this.datumDokumentaDateTimePicker.Value = new System.DateTime(2016, 10, 1, 11, 44, 31, 0);
            // 
            // razlogIzmeneLabel
            // 
            this.razlogIzmeneLabel.AutoSize = true;
            this.razlogIzmeneLabel.Location = new System.Drawing.Point(359, 99);
            this.razlogIzmeneLabel.Name = "razlogIzmeneLabel";
            this.razlogIzmeneLabel.Size = new System.Drawing.Size(79, 13);
            this.razlogIzmeneLabel.TabIndex = 25;
            this.razlogIzmeneLabel.Text = "Razlog izmene:";
            // 
            // razlogIzmeneTextBox
            // 
            this.razlogIzmeneTextBox.Location = new System.Drawing.Point(362, 115);
            this.razlogIzmeneTextBox.Name = "razlogIzmeneTextBox";
            this.razlogIzmeneTextBox.Size = new System.Drawing.Size(187, 20);
            this.razlogIzmeneTextBox.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(305, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Broj dokumenta:";
            // 
            // brojDokumentaTextBox
            // 
            this.brojDokumentaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brojDokumentaTextBox.Location = new System.Drawing.Point(308, 76);
            this.brojDokumentaTextBox.Name = "brojDokumentaTextBox";
            this.brojDokumentaTextBox.Size = new System.Drawing.Size(130, 20);
            this.brojDokumentaTextBox.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Vrsta poverioca:";
            // 
            // nazivDokumentaLabel
            // 
            this.nazivDokumentaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nazivDokumentaLabel.AutoSize = true;
            this.nazivDokumentaLabel.Location = new System.Drawing.Point(159, 59);
            this.nazivDokumentaLabel.Name = "nazivDokumentaLabel";
            this.nazivDokumentaLabel.Size = new System.Drawing.Size(93, 13);
            this.nazivDokumentaLabel.TabIndex = 20;
            this.nazivDokumentaLabel.Text = "Naziv dokumenta:";
            // 
            // nazivDokumentaTextBox
            // 
            this.nazivDokumentaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nazivDokumentaTextBox.Location = new System.Drawing.Point(162, 75);
            this.nazivDokumentaTextBox.Name = "nazivDokumentaTextBox";
            this.nazivDokumentaTextBox.Size = new System.Drawing.Size(123, 20);
            this.nazivDokumentaTextBox.TabIndex = 19;
            // 
            // vrstaPoveriocaComboBox
            // 
            this.vrstaPoveriocaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vrstaPoveriocaComboBox.FormattingEnabled = true;
            this.vrstaPoveriocaComboBox.Items.AddRange(new object[] {
            "Pravna lica",
            "Javni sektor",
            "Polj. gazdinstva",
            "Kompenzacija"});
            this.vrstaPoveriocaComboBox.Location = new System.Drawing.Point(7, 75);
            this.vrstaPoveriocaComboBox.Name = "vrstaPoveriocaComboBox";
            this.vrstaPoveriocaComboBox.Size = new System.Drawing.Size(131, 21);
            this.vrstaPoveriocaComboBox.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(725, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "MB poverioca:";
            // 
            // mbTextBox
            // 
            this.mbTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mbTextBox.Location = new System.Drawing.Point(728, 35);
            this.mbTextBox.Name = "mbTextBox";
            this.mbTextBox.Size = new System.Drawing.Size(97, 20);
            this.mbTextBox.TabIndex = 16;
            // 
            // pibLabel
            // 
            this.pibLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pibLabel.AutoSize = true;
            this.pibLabel.Location = new System.Drawing.Point(598, 19);
            this.pibLabel.Name = "pibLabel";
            this.pibLabel.Size = new System.Drawing.Size(77, 13);
            this.pibLabel.TabIndex = 15;
            this.pibLabel.Text = "PIB poverioca:";
            // 
            // pibTextBox
            // 
            this.pibTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pibTextBox.Location = new System.Drawing.Point(601, 35);
            this.pibTextBox.Name = "pibTextBox";
            this.pibTextBox.Size = new System.Drawing.Size(97, 20);
            this.pibTextBox.TabIndex = 14;
            // 
            // nazivPoveriocaLabel
            // 
            this.nazivPoveriocaLabel.AutoSize = true;
            this.nazivPoveriocaLabel.Location = new System.Drawing.Point(328, 19);
            this.nazivPoveriocaLabel.Name = "nazivPoveriocaLabel";
            this.nazivPoveriocaLabel.Size = new System.Drawing.Size(87, 13);
            this.nazivPoveriocaLabel.TabIndex = 13;
            this.nazivPoveriocaLabel.Text = "Naziv poverioca:";
            // 
            // nazivPoveriocaTextBox
            // 
            this.nazivPoveriocaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nazivPoveriocaTextBox.Location = new System.Drawing.Point(331, 35);
            this.nazivPoveriocaTextBox.Name = "nazivPoveriocaTextBox";
            this.nazivPoveriocaTextBox.Size = new System.Drawing.Size(249, 20);
            this.nazivPoveriocaTextBox.TabIndex = 12;
            // 
            // rinoIznosLabel
            // 
            this.rinoIznosLabel.AutoSize = true;
            this.rinoIznosLabel.Location = new System.Drawing.Point(161, 19);
            this.rinoIznosLabel.Name = "rinoIznosLabel";
            this.rinoIznosLabel.Size = new System.Drawing.Size(35, 13);
            this.rinoIznosLabel.TabIndex = 11;
            this.rinoIznosLabel.Text = "Iznos:";
            // 
            // rinoIznosTextBox
            // 
            this.rinoIznosTextBox.Location = new System.Drawing.Point(164, 35);
            this.rinoIznosTextBox.Name = "rinoIznosTextBox";
            this.rinoIznosTextBox.Size = new System.Drawing.Size(137, 20);
            this.rinoIznosTextBox.TabIndex = 10;
            // 
            // rinoActionTypeLabel
            // 
            this.rinoActionTypeLabel.AutoSize = true;
            this.rinoActionTypeLabel.Location = new System.Drawing.Point(6, 18);
            this.rinoActionTypeLabel.Name = "rinoActionTypeLabel";
            this.rinoActionTypeLabel.Size = new System.Drawing.Size(62, 13);
            this.rinoActionTypeLabel.TabIndex = 9;
            this.rinoActionTypeLabel.Text = "Vrsta posla:";
            // 
            // rinoActionTypeComboBox
            // 
            this.rinoActionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rinoActionTypeComboBox.FormattingEnabled = true;
            this.rinoActionTypeComboBox.Items.AddRange(new object[] {
            "Unos",
            "Izmena",
            "Otkazivanje"});
            this.rinoActionTypeComboBox.Location = new System.Drawing.Point(6, 34);
            this.rinoActionTypeComboBox.Name = "rinoActionTypeComboBox";
            this.rinoActionTypeComboBox.Size = new System.Drawing.Size(133, 21);
            this.rinoActionTypeComboBox.TabIndex = 8;
            // 
            // insertNewObligationBtn
            // 
            this.insertNewObligationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.insertNewObligationBtn.Location = new System.Drawing.Point(601, 144);
            this.insertNewObligationBtn.Name = "insertNewObligationBtn";
            this.insertNewObligationBtn.Size = new System.Drawing.Size(115, 22);
            this.insertNewObligationBtn.TabIndex = 7;
            this.insertNewObligationBtn.Text = "Ubaci novi";
            this.insertNewObligationBtn.UseVisualStyleBackColor = true;
            this.insertNewObligationBtn.Click += new System.EventHandler(this.insertNewObligationBtn_Click);
            // 
            // loadObligationXmlBtn
            // 
            this.loadObligationXmlBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadObligationXmlBtn.Location = new System.Drawing.Point(6, 144);
            this.loadObligationXmlBtn.Name = "loadObligationXmlBtn";
            this.loadObligationXmlBtn.Size = new System.Drawing.Size(115, 22);
            this.loadObligationXmlBtn.TabIndex = 6;
            this.loadObligationXmlBtn.Text = "Učitaj XML fajl";
            this.loadObligationXmlBtn.UseVisualStyleBackColor = true;
            this.loadObligationXmlBtn.Click += new System.EventHandler(this.loadObligationXmlBtn_Click);
            // 
            // saveObligationXmlBtn
            // 
            this.saveObligationXmlBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveObligationXmlBtn.Location = new System.Drawing.Point(722, 144);
            this.saveObligationXmlBtn.Name = "saveObligationXmlBtn";
            this.saveObligationXmlBtn.Size = new System.Drawing.Size(115, 22);
            this.saveObligationXmlBtn.TabIndex = 5;
            this.saveObligationXmlBtn.Text = "Snimi XML fajl";
            this.saveObligationXmlBtn.UseVisualStyleBackColor = true;
            this.saveObligationXmlBtn.Click += new System.EventHandler(this.saveObligationXmlBtn_Click);
            // 
            // razduzenjaTabPage
            // 
            this.razduzenjaTabPage.Controls.Add(this.button1);
            this.razduzenjaTabPage.Controls.Add(this.button2);
            this.razduzenjaTabPage.Controls.Add(this.button3);
            this.razduzenjaTabPage.Controls.Add(this.dataGridView2);
            this.razduzenjaTabPage.Controls.Add(this.groupBox1);
            this.razduzenjaTabPage.Location = new System.Drawing.Point(4, 4);
            this.razduzenjaTabPage.Name = "razduzenjaTabPage";
            this.razduzenjaTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.razduzenjaTabPage.Size = new System.Drawing.Size(855, 435);
            this.razduzenjaTabPage.TabIndex = 1;
            this.razduzenjaTabPage.Text = "Razduženja";
            this.razduzenjaTabPage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 22);
            this.button1.TabIndex = 9;
            this.button1.Text = "Izmeni";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(127, 229);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 22);
            this.button2.TabIndex = 8;
            this.button2.Text = "Obriši";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(734, 229);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 22);
            this.button3.TabIndex = 7;
            this.button3.Text = "Obriši sve";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(6, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(843, 217);
            this.dataGridView2.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Location = new System.Drawing.Point(6, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(843, 172);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(601, 144);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(115, 22);
            this.button4.TabIndex = 7;
            this.button4.Text = "Ubaci novi";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point(6, 144);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(115, 22);
            this.button5.TabIndex = 6;
            this.button5.Text = "Učitaj XML fajl";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(722, 144);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(115, 22);
            this.button6.TabIndex = 5;
            this.button6.Text = "Snimi XML fajl";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // aboutLinkLabel
            // 
            this.aboutLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutLinkLabel.AutoSize = true;
            this.aboutLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.aboutLinkLabel.Location = new System.Drawing.Point(798, 9);
            this.aboutLinkLabel.Name = "aboutLinkLabel";
            this.aboutLinkLabel.Size = new System.Drawing.Size(77, 13);
            this.aboutLinkLabel.TabIndex = 1;
            this.aboutLinkLabel.TabStop = true;
            this.aboutLinkLabel.Text = "O programu ....";
            // 
            // openObligationXmlDialog
            // 
            this.openObligationXmlDialog.Filter = "RINO XML zaduzenje|*.xml";
            // 
            // jbbkConfigLinkLabel
            // 
            this.jbbkConfigLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jbbkConfigLinkLabel.AutoSize = true;
            this.jbbkConfigLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.jbbkConfigLinkLabel.Location = new System.Drawing.Point(704, 9);
            this.jbbkConfigLinkLabel.Name = "jbbkConfigLinkLabel";
            this.jbbkConfigLinkLabel.Size = new System.Drawing.Size(88, 13);
            this.jbbkConfigLinkLabel.TabIndex = 2;
            this.jbbkConfigLinkLabel.TabStop = true;
            this.jbbkConfigLinkLabel.Text = "Podesi JBBK broj";
            this.jbbkConfigLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.jbbkConfigLinkLabel_LinkClicked);
            // 
            // saveObligationChangesBtn
            // 
            this.saveObligationChangesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveObligationChangesBtn.Location = new System.Drawing.Point(480, 144);
            this.saveObligationChangesBtn.Name = "saveObligationChangesBtn";
            this.saveObligationChangesBtn.Size = new System.Drawing.Size(115, 22);
            this.saveObligationChangesBtn.TabIndex = 5;
            this.saveObligationChangesBtn.Text = "Snimi izmenu";
            this.saveObligationChangesBtn.UseVisualStyleBackColor = true;
            this.saveObligationChangesBtn.Visible = false;
            this.saveObligationChangesBtn.Click += new System.EventHandler(this.saveObligationChangesBtn_Click);
            // 
            // cloneObliationBtn
            // 
            this.cloneObliationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneObliationBtn.Location = new System.Drawing.Point(248, 229);
            this.cloneObliationBtn.Name = "cloneObliationBtn";
            this.cloneObliationBtn.Size = new System.Drawing.Size(115, 22);
            this.cloneObliationBtn.TabIndex = 6;
            this.cloneObliationBtn.Text = "Kloniraj stavku";
            this.cloneObliationBtn.UseVisualStyleBackColor = true;
            this.cloneObliationBtn.Click += new System.EventHandler(this.cloneObliationBtn_Click);
            // 
            // saveObligationXmlDialog
            // 
            this.saveObligationXmlDialog.Filter = "RINO XML zaduzenje|*.xml";
            // 
            // rinoActionColumn
            // 
            this.rinoActionColumn.DataPropertyName = "rinoAction";
            this.rinoActionColumn.HeaderText = "Vrsta posla";
            this.rinoActionColumn.Name = "rinoActionColumn";
            // 
            // rinoIznosColumn
            // 
            this.rinoIznosColumn.DataPropertyName = "rinoIznos";
            this.rinoIznosColumn.HeaderText = "Iznos";
            this.rinoIznosColumn.Name = "rinoIznosColumn";
            // 
            // rinoNazivPoveriocaColumn
            // 
            this.rinoNazivPoveriocaColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rinoNazivPoveriocaColumn.DataPropertyName = "rinoNazivPoverioca";
            this.rinoNazivPoveriocaColumn.HeaderText = "Naziv poverioca";
            this.rinoNazivPoveriocaColumn.MinimumWidth = 100;
            this.rinoNazivPoveriocaColumn.Name = "rinoNazivPoveriocaColumn";
            // 
            // rinoPibColumn
            // 
            this.rinoPibColumn.DataPropertyName = "rinoPIB";
            this.rinoPibColumn.HeaderText = "PIB";
            this.rinoPibColumn.Name = "rinoPibColumn";
            this.rinoPibColumn.Width = 75;
            // 
            // rinoMbColumn
            // 
            this.rinoMbColumn.DataPropertyName = "rinoMB";
            this.rinoMbColumn.HeaderText = "MB";
            this.rinoMbColumn.Name = "rinoMbColumn";
            this.rinoMbColumn.Width = 75;
            // 
            // rinoVrstaPoveriocaColumn
            // 
            this.rinoVrstaPoveriocaColumn.DataPropertyName = "rinoVrstaPoverioca";
            this.rinoVrstaPoveriocaColumn.HeaderText = "Vrsta poverioca";
            this.rinoVrstaPoveriocaColumn.Name = "rinoVrstaPoveriocaColumn";
            // 
            // rinoNazivDokumentaColumn
            // 
            this.rinoNazivDokumentaColumn.DataPropertyName = "rinoNazivDokumenta";
            this.rinoNazivDokumentaColumn.HeaderText = "Naziv dokumenta";
            this.rinoNazivDokumentaColumn.Name = "rinoNazivDokumentaColumn";
            // 
            // rinoBrojDokumentaColumn
            // 
            this.rinoBrojDokumentaColumn.DataPropertyName = "rinoBrojDokumenta";
            this.rinoBrojDokumentaColumn.HeaderText = "Broj dokumenta";
            this.rinoBrojDokumentaColumn.Name = "rinoBrojDokumentaColumn";
            // 
            // rinoDatumDokumentaColumn
            // 
            this.rinoDatumDokumentaColumn.DataPropertyName = "rinoDatumDokumenta";
            this.rinoDatumDokumentaColumn.HeaderText = "Datum dokumenta";
            this.rinoDatumDokumentaColumn.Name = "rinoDatumDokumentaColumn";
            // 
            // rinoDatumNastankaColumn
            // 
            this.rinoDatumNastankaColumn.DataPropertyName = "rinoDatumNastanka";
            this.rinoDatumNastankaColumn.HeaderText = "Datum nastanka";
            this.rinoDatumNastankaColumn.Name = "rinoDatumNastankaColumn";
            // 
            // rinoDatumRokaZaIzmirenjeColumn
            // 
            this.rinoDatumRokaZaIzmirenjeColumn.DataPropertyName = "rinoDatumRokaZaIzmirenje";
            this.rinoDatumRokaZaIzmirenjeColumn.HeaderText = "Datum roka za izmirenje";
            this.rinoDatumRokaZaIzmirenjeColumn.Name = "rinoDatumRokaZaIzmirenjeColumn";
            // 
            // rinoRazlogZaIzmenuColumn
            // 
            this.rinoRazlogZaIzmenuColumn.DataPropertyName = "rinoRazlogIzmene";
            this.rinoRazlogZaIzmenuColumn.HeaderText = "Razlog za izmenu";
            this.rinoRazlogZaIzmenuColumn.Name = "rinoRazlogZaIzmenuColumn";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 498);
            this.Controls.Add(this.jbbkConfigLinkLabel);
            this.Controls.Add(this.aboutLinkLabel);
            this.Controls.Add(this.rinoTabControl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyRino";
            this.rinoTabControl.ResumeLayout(false);
            this.zaduzenjaTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rinoObligationDataGridView)).EndInit();
            this.zaduzenjaGroupBox.ResumeLayout(false);
            this.zaduzenjaGroupBox.PerformLayout();
            this.razduzenjaTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl rinoTabControl;
        private System.Windows.Forms.TabPage zaduzenjaTabPage;
        private System.Windows.Forms.TabPage razduzenjaTabPage;
        private System.Windows.Forms.LinkLabel aboutLinkLabel;
        private System.Windows.Forms.GroupBox zaduzenjaGroupBox;
        private System.Windows.Forms.DataGridView rinoObligationDataGridView;
        private System.Windows.Forms.Button deleteAllObligationsBtn;
        private System.Windows.Forms.Button modifyObligationBtn;
        private System.Windows.Forms.Button deleteObligationBtn;
        private System.Windows.Forms.Button insertNewObligationBtn;
        private System.Windows.Forms.Button loadObligationXmlBtn;
        private System.Windows.Forms.Button saveObligationXmlBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.OpenFileDialog openObligationXmlDialog;
        private System.Windows.Forms.ComboBox rinoActionTypeComboBox;
        private System.Windows.Forms.Label rinoActionTypeLabel;
        private System.Windows.Forms.Label rinoIznosLabel;
        private System.Windows.Forms.TextBox rinoIznosTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mbTextBox;
        private System.Windows.Forms.Label pibLabel;
        private System.Windows.Forms.TextBox pibTextBox;
        private System.Windows.Forms.Label nazivPoveriocaLabel;
        private System.Windows.Forms.TextBox nazivPoveriocaTextBox;
        private System.Windows.Forms.ComboBox vrstaPoveriocaComboBox;
        private System.Windows.Forms.CheckBox noDueDateCheckBox;
        private System.Windows.Forms.Label datumRokaIzmirenjaLabel;
        private System.Windows.Forms.DateTimePicker datumRokaIzmirenjaDateTimePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker datumNastankaDateTimePicker;
        private System.Windows.Forms.Label datumDokumentaLabel;
        private System.Windows.Forms.DateTimePicker datumDokumentaDateTimePicker;
        private System.Windows.Forms.Label razlogIzmeneLabel;
        private System.Windows.Forms.TextBox razlogIzmeneTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox brojDokumentaTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label nazivDokumentaLabel;
        private System.Windows.Forms.TextBox nazivDokumentaTextBox;
        private System.Windows.Forms.LinkLabel jbbkConfigLinkLabel;
        private System.Windows.Forms.Button saveObligationChangesBtn;
        private System.Windows.Forms.Button cloneObliationBtn;
        private System.Windows.Forms.SaveFileDialog saveObligationXmlDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoActionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoIznosColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoNazivPoveriocaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoPibColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoMbColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoVrstaPoveriocaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoNazivDokumentaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoBrojDokumentaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoDatumDokumentaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoDatumNastankaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoDatumRokaZaIzmirenjeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rinoRazlogZaIzmenuColumn;
    }
}

