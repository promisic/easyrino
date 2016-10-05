namespace GriffinSoft.EasyRino
{
    partial class EasyRinoConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyRinoConfigForm));
            this.jbbkGroupBox = new System.Windows.Forms.GroupBox();
            this.jbbkTextBox = new System.Windows.Forms.TextBox();
            this.saveJbbkBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.jbbkGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // jbbkGroupBox
            // 
            this.jbbkGroupBox.Controls.Add(this.jbbkTextBox);
            this.jbbkGroupBox.Location = new System.Drawing.Point(12, 12);
            this.jbbkGroupBox.Name = "jbbkGroupBox";
            this.jbbkGroupBox.Size = new System.Drawing.Size(201, 85);
            this.jbbkGroupBox.TabIndex = 0;
            this.jbbkGroupBox.TabStop = false;
            this.jbbkGroupBox.Text = "JBBK/DJBK broj:";
            // 
            // jbbkTextBox
            // 
            this.jbbkTextBox.Location = new System.Drawing.Point(33, 41);
            this.jbbkTextBox.Name = "jbbkTextBox";
            this.jbbkTextBox.Size = new System.Drawing.Size(124, 20);
            this.jbbkTextBox.TabIndex = 0;
            this.jbbkTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // saveJbbkBtn
            // 
            this.saveJbbkBtn.Location = new System.Drawing.Point(14, 110);
            this.saveJbbkBtn.Name = "saveJbbkBtn";
            this.saveJbbkBtn.Size = new System.Drawing.Size(198, 22);
            this.saveJbbkBtn.TabIndex = 1;
            this.saveJbbkBtn.Text = "Snimi";
            this.saveJbbkBtn.UseVisualStyleBackColor = true;
            this.saveJbbkBtn.Click += new System.EventHandler(this.saveJbbkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(15, 138);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(198, 22);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Zaboravi";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // EasyRinoConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 172);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveJbbkBtn);
            this.Controls.Add(this.jbbkGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EasyRinoConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JBBK broj";
            this.Load += new System.EventHandler(this.EasyRinoConfigForm_Load);
            this.jbbkGroupBox.ResumeLayout(false);
            this.jbbkGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox jbbkGroupBox;
        private System.Windows.Forms.TextBox jbbkTextBox;
        private System.Windows.Forms.Button saveJbbkBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}