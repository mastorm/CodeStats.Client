using System.Globalization;
using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CodeStats.Client.OptionsPage
{
    [CLSCompliant(false), ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("5c3c0aa0-0bc8-43c5-afc9-5aa55662794b")]
    public partial class SettingsPage : DialogPage
    {
        private string _machineKey = "";
        [Category("CodeStats Settings")]
        [DisplayName("Machine Key")]
        [Description("Set your machine´s API Key here")]
        public string MachineKey
        {
            get { return _machineKey; }
            set {
                _machineKey = value;
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
