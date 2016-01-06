using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VitaliiGanzha.VsDingExtension
{
    [Guid("1735471A-D4F2-4385-8AF8-5E9E98409A9C")]
    public class SoundsSelectOptionsPage : DialogPage
    {
        #region Fields

        private SoundSelectControl optionsControl;

        #endregion Fields

        #region Properties

        public Action OnApplyHandler { get; set; }

        public Action StoreOptionsNotifier { get; set; }

        public Action OnActivateHandler { get; set; }

        /// <summary>
        /// Gets the window an instance of DialogPage that it uses as its user interface.
        /// </summary>
        /// <devdoc>
        /// The window this dialog page will use for its UI.
        /// This window handle must be constant, so if you are
        /// returning a Windows Forms control you must make sure
        /// it does not recreate its handle.  If the window object
        /// implements IComponent it will be sited by the 
        /// dialog page so it can get access to global services.
        /// </devdoc>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                if (optionsControl == null)
                {
                    optionsControl = new SoundSelectControl
                    {
                        Location = new Point(0, 0),
                        OptionsPage = this
                    };
                }
                return optionsControl;
            }
        }

        protected override void OnApply(DialogPage.PageApplyEventArgs e)
        {
            if (this.StoreOptionsNotifier != null)
            {
                this.StoreOptionsNotifier();
            }

            if (this.OnApplyHandler != null)
            {
                this.OnApplyHandler();
            }
            base.OnApply(e);
        }

        protected override void OnActivate(CancelEventArgs cancelEventArgs)
        {
            if (this.OnActivateHandler != null)
            {
                this.OnActivateHandler();
            }
            base.OnActivate(cancelEventArgs);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OverrideOnBuildSound { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomOnBuildSoundLocation { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OverrideOnTestCompleteSuccesSound { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomOnTestCompleteSuccesSoundLocation { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OverrideOnTestCompleteFailureSound { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomOnTestCompleteFailureSoundLocation { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OverrideOnBreakpointHitSound { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomOnBreakpointHitSoundLocation { get; set; }

        #endregion Properties

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (optionsControl != null)
                {
                    optionsControl.Dispose();
                    optionsControl = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion Methods

        private void InitializeComponent()
        {

        }
    }
}
