namespace VitaliiGanzha.VsDingExtension
{
    partial class SoundSelectControl
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
            this.TestsCompletedFailureControl = new VitaliiGanzha.VsDingExtension.SingleSoundSelectControl();
            this.TestCompletedSuccessControl = new VitaliiGanzha.VsDingExtension.SingleSoundSelectControl();
            this.breakPointHitControl = new VitaliiGanzha.VsDingExtension.SingleSoundSelectControl();
            this.buildCompletedControl = new VitaliiGanzha.VsDingExtension.SingleSoundSelectControl();
            this.SuspendLayout();
            // 
            // TestsCompletedFailureControl
            // 
            this.TestsCompletedFailureControl.BoxTitle = "Tests completed, failure";
            this.TestsCompletedFailureControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.TestsCompletedFailureControl.EventType = VitaliiGanzha.VsDingExtension.EventType.TestsCompletedFailure;
            this.TestsCompletedFailureControl.Location = new System.Drawing.Point(0, 234);
            this.TestsCompletedFailureControl.Name = "TestsCompletedFailureControl";
            this.TestsCompletedFailureControl.Size = new System.Drawing.Size(452, 78);
            this.TestsCompletedFailureControl.TabIndex = 3;
            // 
            // TestCompletedSuccessControl
            // 
            this.TestCompletedSuccessControl.BoxTitle = "Tests completed, success";
            this.TestCompletedSuccessControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.TestCompletedSuccessControl.EventType = VitaliiGanzha.VsDingExtension.EventType.TestsCompletedSuccess;
            this.TestCompletedSuccessControl.Location = new System.Drawing.Point(0, 156);
            this.TestCompletedSuccessControl.Name = "TestCompletedSuccessControl";
            this.TestCompletedSuccessControl.Size = new System.Drawing.Size(452, 78);
            this.TestCompletedSuccessControl.TabIndex = 2;
            // 
            // breakPointHitControl
            // 
            this.breakPointHitControl.BoxTitle = "Breakpoint hit";
            this.breakPointHitControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.breakPointHitControl.EventType = VitaliiGanzha.VsDingExtension.EventType.BreakpointHit;
            this.breakPointHitControl.Location = new System.Drawing.Point(0, 78);
            this.breakPointHitControl.Name = "breakPointHitControl";
            this.breakPointHitControl.Size = new System.Drawing.Size(452, 78);
            this.breakPointHitControl.TabIndex = 1;
            // 
            // buildCompletedControl
            // 
            this.buildCompletedControl.BoxTitle = "Build completed";
            this.buildCompletedControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.buildCompletedControl.EventType = VitaliiGanzha.VsDingExtension.EventType.BuildCompleted;
            this.buildCompletedControl.Location = new System.Drawing.Point(0, 0);
            this.buildCompletedControl.Name = "buildCompletedControl";
            this.buildCompletedControl.Size = new System.Drawing.Size(452, 78);
            this.buildCompletedControl.TabIndex = 0;
            // 
            // SoundSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.TestsCompletedFailureControl);
            this.Controls.Add(this.TestCompletedSuccessControl);
            this.Controls.Add(this.breakPointHitControl);
            this.Controls.Add(this.buildCompletedControl);
            this.Name = "SoundSelectControl";
            this.Size = new System.Drawing.Size(452, 371);
            this.ResumeLayout(false);

        }

        #endregion

        private SingleSoundSelectControl buildCompletedControl;
        private SingleSoundSelectControl TestCompletedSuccessControl;
        private SingleSoundSelectControl breakPointHitControl;
        private SingleSoundSelectControl TestsCompletedFailureControl;

    }
}
