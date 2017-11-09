using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using CodeStats.Client.OptionsPage;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace CodeStats.Client
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Guid("5c3c0aa0-0bc8-43c5-afc9-5aa55662794b")]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(SettingsPage), "CodeStats", "General", 0, 0, true)]
    public  class FilterProvider : Package, IVsTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("CodeStatsClientLayer")]
        [TextViewRole(PredefinedTextViewRoles.Editable)]
        internal AdornmentLayerDefinition m_multieditAdornmentLayer = null;

        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView textView = editorFactory.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            AddCommandFilter(textViewAdapter, new CommandFilter(textView));
        }

        void AddCommandFilter(IVsTextView viewAdapter, CommandFilter commandFilter)
        {
            if (commandFilter.Added)
            {
                return;
            }

            IOleCommandTarget next;
            int hr = viewAdapter.AddCommandFilter(commandFilter, out next);

            if (hr == VSConstants.S_OK)
            {
                commandFilter.Added = true;
                //you'll need the next target for Exec and QueryStatus
                if (next != null)
                    commandFilter.NextTarget = next;
            }
        }

        public string MachineKey
        {
            get
            {
                SettingsPage page = (SettingsPage)GetDialogPage(typeof(SettingsPage));
                return page.MachineKey;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

    }
}
