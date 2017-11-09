using System.Globalization;
using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CodeStats.Client.OptionsPage
{
    #pragma warning disable CS3021 // Im really sure i know what im doing here. Do not try this at home! :)
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

        private string _pulseApiUrl = "";
        [Category("CodeStats Settings")]
        [DisplayName("API Url")]
        [Description("The Pulse API that requests get sent to. DO NOT CHANGE if you dont know what you are doing!")]
        public string PulseApiUrl
        {
            get { return _pulseApiUrl; }
            set
            {
                _pulseApiUrl = value;
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            if(PulseApiUrl == "")
                PulseApiUrl = "https://codestats.net/api/my/pulses";

        }
    }
}
