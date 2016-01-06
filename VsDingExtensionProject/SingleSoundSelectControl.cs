namespace VitaliiGanzha.VsDingExtension
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Media;
    using System.Windows.Forms;

    public partial class SingleSoundSelectControl : UserControl
    {
        private EventType eventType = EventType.None;

        public SoundsSelectOptionsPage optionsPage = null;

        public SoundsSelectOptionsPage OptionsPage
        {
            get { return this.optionsPage; }
            set
            {
                this.optionsPage = value;
                this.ReadOptions();
                this.optionsPage.StoreOptionsNotifier += this.StoreOptions;
                this.optionsPage.OnActivateHandler += this.ReadOptions;
            }
        }

        [Category("Data")]
        [Description("Gets or sets the event type of the sound to override sound for")]
        public EventType EventType
        {
            get { return this.eventType; }
            set { this.eventType = value; }
        }

        [Category("Data")]
        [Description("Gets or sets the title of the group box")]
        public string BoxTitle
        {
            get { return this.groupBox.Text; }
            set { this.groupBox.Text = value; }
        }

        public SingleSoundSelectControl()
        {
            InitializeComponent();
        }

        private void chkUseDifferentSound_CheckedChanged(object sender, System.EventArgs e)
        {
            ValidateProperties();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.ValidateProperties();
            var dialogResult = this.openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            var file = openFileDialog.FileName;
            if (File.Exists(file))
            {
                this.selectedFileEdit.Text = file;
            }
        }

        private void ValidateProperties()
        {
            if (this.EventType == EventType.None)
            {
                throw new Exception("Event type not initialized");
            }

            if (this.OptionsPage == null)
            {
                throw new Exception("Options Page not initialized");
            }
        }

        private void StoreOptions()
        {
            var useDifferentSound = this.chkUseDifferentSound.Checked;
            var pathToFile = this.selectedFileEdit.Text;
            switch (eventType)
            {
                case EventType.BreakpointHit:
                    this.OptionsPage.OverrideOnBreakpointHitSound = useDifferentSound;
                    this.OptionsPage.CustomOnBreakpointHitSoundLocation = pathToFile;
                    break;
                case EventType.BuildCompleted:
                    this.OptionsPage.OverrideOnBuildSound = useDifferentSound;
                    this.OptionsPage.CustomOnBuildSoundLocation = pathToFile;
                    break;
                case EventType.TestsCompletedFailure:
                    this.OptionsPage.OverrideOnTestCompleteFailureSound = useDifferentSound;
                    this.OptionsPage.CustomOnTestCompleteFailureSoundLocation = pathToFile;
                    break;
                case EventType.TestsCompletedSuccess:
                    this.OptionsPage.OverrideOnTestCompleteSuccesSound = useDifferentSound;
                    this.OptionsPage.CustomOnTestCompleteSuccesSoundLocation = pathToFile;
                    break;
                default:
                    throw new Exception("Invalid Event Type");
            }
        }

        public void ReadOptions()
        {
            string pathToFile = null;
            bool useDifferentSound = false;

            switch (eventType)
            {
                case EventType.BreakpointHit:
                    useDifferentSound = this.OptionsPage.OverrideOnBreakpointHitSound;
                    pathToFile = this.OptionsPage.CustomOnBreakpointHitSoundLocation;
                    break;
                case EventType.BuildCompleted:
                    useDifferentSound = this.OptionsPage.OverrideOnBuildSound;
                    pathToFile = this.OptionsPage.CustomOnBuildSoundLocation;
                    break;
                case EventType.TestsCompletedFailure:
                    useDifferentSound = this.OptionsPage.OverrideOnTestCompleteFailureSound;
                    pathToFile = this.OptionsPage.CustomOnTestCompleteFailureSoundLocation;
                    break;
                case EventType.TestsCompletedSuccess:
                    useDifferentSound = this.OptionsPage.OverrideOnTestCompleteSuccesSound;
                    pathToFile = this.OptionsPage.CustomOnTestCompleteSuccesSoundLocation;
                    break;
            }

            this.selectedFileEdit.Text = pathToFile;
            this.chkUseDifferentSound.Checked = useDifferentSound;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            const string title = "Can't play sound";
            if (string.IsNullOrWhiteSpace(this.selectedFileEdit.Text))
            {
                MessageBox.Show("No file selected", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(this.selectedFileEdit.Text))
            {
                MessageBox.Show("File does not exists", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var player = new SoundPlayer(this.selectedFileEdit.Text))
                {
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Environment.NewLine + "Error: " + ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
