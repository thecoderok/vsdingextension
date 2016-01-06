namespace VitaliiGanzha.VsDingExtension
{
    partial class SingleSoundSelectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.selectedFileEdit = new System.Windows.Forms.TextBox();
            this.chkUseDifferentSound = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.btnTest);
            this.groupBox.Controls.Add(this.btnBrowse);
            this.groupBox.Controls.Add(this.selectedFileEdit);
            this.groupBox.Controls.Add(this.chkUseDifferentSound);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(323, 66);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Title";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(242, 40);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // selectedFileEdit
            // 
            this.selectedFileEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedFileEdit.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedFileEdit.Location = new System.Drawing.Point(6, 40);
            this.selectedFileEdit.Name = "selectedFileEdit";
            this.selectedFileEdit.Size = new System.Drawing.Size(183, 22);
            this.selectedFileEdit.TabIndex = 1;
            // 
            // chkUseDifferentSound
            // 
            this.chkUseDifferentSound.AutoSize = true;
            this.chkUseDifferentSound.Location = new System.Drawing.Point(6, 19);
            this.chkUseDifferentSound.Name = "chkUseDifferentSound";
            this.chkUseDifferentSound.Size = new System.Drawing.Size(118, 17);
            this.chkUseDifferentSound.TabIndex = 0;
            this.chkUseDifferentSound.Text = "Use different sound";
            this.chkUseDifferentSound.UseVisualStyleBackColor = true;
            this.chkUseDifferentSound.CheckedChanged += new System.EventHandler(this.chkUseDifferentSound_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Wav files|*.wav";
            this.openFileDialog.Title = "Select custom sound";
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(195, 40);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(41, 23);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // SingleSoundSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "SingleSoundSelectControl";
            this.Size = new System.Drawing.Size(323, 66);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.CheckBox chkUseDifferentSound;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox selectedFileEdit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnTest;
    }
}
