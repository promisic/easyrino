namespace GriffinSoft.EasyRino
{
    partial class EasyRinoAboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyRinoAboutForm));
            this.uscLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.easyRinoLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.authorLabel = new System.Windows.Forms.Label();
            this.uscBorLabel = new System.Windows.Forms.Label();
            this.uscBorLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.uscLogoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // uscLogoPictureBox
            // 
            this.uscLogoPictureBox.Image = global::GriffinSoft.EasyRino.Properties.Resources.usc_logo;
            this.uscLogoPictureBox.Location = new System.Drawing.Point(247, 106);
            this.uscLogoPictureBox.Name = "uscLogoPictureBox";
            this.uscLogoPictureBox.Size = new System.Drawing.Size(153, 115);
            this.uscLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.uscLogoPictureBox.TabIndex = 1;
            this.uscLogoPictureBox.TabStop = false;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::GriffinSoft.EasyRino.Properties.Resources.easyrino;
            this.logoPictureBox.Location = new System.Drawing.Point(12, 12);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(110, 107);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // easyRinoLabel
            // 
            this.easyRinoLabel.AutoSize = true;
            this.easyRinoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.easyRinoLabel.Location = new System.Drawing.Point(128, 12);
            this.easyRinoLabel.Name = "easyRinoLabel";
            this.easyRinoLabel.Size = new System.Drawing.Size(174, 39);
            this.easyRinoLabel.TabIndex = 2;
            this.easyRinoLabel.Text = "EasyRino";
            this.easyRinoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(134, 51);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(286, 13);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "Softver za generisanje i obradu XML naloga za RINO portal";
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authorLabel.Location = new System.Drawing.Point(133, 83);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(429, 20);
            this.authorLabel.TabIndex = 4;
            this.authorLabel.Text = "Autor programa: Dušan Mišić <promisic@gmail.com>";
            // 
            // uscBorLabel
            // 
            this.uscBorLabel.AutoSize = true;
            this.uscBorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uscBorLabel.Location = new System.Drawing.Point(178, 234);
            this.uscBorLabel.Name = "uscBorLabel";
            this.uscBorLabel.Size = new System.Drawing.Size(293, 24);
            this.uscBorLabel.TabIndex = 5;
            this.uscBorLabel.Text = "Ustanova Sportski centar \"Bor\"";
            // 
            // uscBorLinkLabel
            // 
            this.uscBorLinkLabel.AutoSize = true;
            this.uscBorLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uscBorLinkLabel.Location = new System.Drawing.Point(219, 260);
            this.uscBorLinkLabel.Name = "uscBorLinkLabel";
            this.uscBorLinkLabel.Size = new System.Drawing.Size(203, 20);
            this.uscBorLinkLabel.TabIndex = 6;
            this.uscBorLinkLabel.TabStop = true;
            this.uscBorLinkLabel.Text = "www.sportskicentarbor.com";
            this.uscBorLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.uscBorLinkLabel_LinkClicked);
            // 
            // EasyRinoAboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 291);
            this.Controls.Add(this.uscBorLinkLabel);
            this.Controls.Add(this.uscBorLabel);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.easyRinoLabel);
            this.Controls.Add(this.uscLogoPictureBox);
            this.Controls.Add(this.logoPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EasyRinoAboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "O EasyRino programu ....";
            this.Load += new System.EventHandler(this.EasyRinoAboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uscLogoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.PictureBox uscLogoPictureBox;
        private System.Windows.Forms.Label easyRinoLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label uscBorLabel;
        private System.Windows.Forms.LinkLabel uscBorLinkLabel;
    }
}