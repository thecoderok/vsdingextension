using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace VitaliiGanzha.VsDingExtension
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class OptionsDialog : DialogPage
    {
        [Category("Beeps")]
        [DisplayName("Breakpoint")]
        [Description("Beep when a breakpoint is hit")]
        public bool IsBeepOnBreakpointHit { get; set; }

        [Category("Beeps")]
        [DisplayName("Build")]
        [Description("Beep when a build is completed")]
        public bool IsBeepOnBuildComplete { get; set; }

        [Category("Beeps")]
        [DisplayName("Tests")]
        [Description("Beep when a test run is completed")]
        public bool IsBeepOnTestComplete { get; set; }

        [Category("Beeps")]
        [DisplayName("Failed Tests")]
        [Description("Beep only when a test failed")]
        public bool IsBeepOnTestFailed { get; set; }

        [DisplayName("Only when in background")]
        [Description("Beep only when Visual Studio does not have focus")]
        public bool IsBeepOnlyWhenVisualStudioIsInBackground { get; set; }

        [DisplayName("Show tray notifications")]
        [Description("Show tray notifications for enabled events")]
        public bool ShowTrayNotifications { get; set; }

        [DisplayName("Tray notifications message")]
        [Description("Show message how to disable tray notifications")]
        public bool ShowTrayDisableMessage { get; set; }

        public OptionsDialog()
        {
            IsBeepOnBreakpointHit = true;
            IsBeepOnBuildComplete = true;
            IsBeepOnTestComplete = true;
            IsBeepOnTestFailed = false;
            ShowTrayNotifications = true;
            IsBeepOnlyWhenVisualStudioIsInBackground = false;
            ShowTrayDisableMessage = true;
        }
    }
}
