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
        public bool BreakpointBeep { get; set; }

        [Category("Beeps")]
        [DisplayName("Build")]
        [Description("Beep when a build is completed")]
        public bool BuildBeep { get; set; }

        [Category("Beeps")]
        [DisplayName("Tests")]
        [Description("Beep when a test run is completed")]
        public bool TestBeep { get; set; }

        [DisplayName("Only when in background")]
        [Description("Beep only when Visual Studio does not have focus")]
        public bool BeepOnUnfocus { get; set; }

        public OptionsDialog()
        {
            BreakpointBeep = true;
            BuildBeep = true;
            TestBeep = true;
            BeepOnUnfocus = false;
        }
    }
}
