using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VitaliiGanzha.VsDingExtension
{
    public partial class SoundSelectControl : UserControl
    {
        private SoundsSelectOptionsPage optionsPage;

        public SoundsSelectOptionsPage OptionsPage
        {
            get { return this.optionsPage; }
            set
            {
                this.optionsPage = value;
                UpdateOptionPage();
            }
        }

        private void UpdateOptionPage()
        {
            this.TestCompletedSuccessControl.OptionsPage = this.optionsPage;
            this.TestsCompletedFailureControl.OptionsPage = this.optionsPage;
            this.buildCompletedControl.OptionsPage = this.optionsPage;
            this.breakPointHitControl.OptionsPage = this.optionsPage;
        }

        public SoundSelectControl()
        {
            InitializeComponent();
        }
    }
}
