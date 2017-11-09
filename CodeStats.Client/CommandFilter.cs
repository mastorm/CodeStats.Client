using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using CodeStats.Client.OptionsPage;

namespace CodeStats.Client
{
    public class CommandFilter : IOleCommandTarget
    {
        private IWpfTextView _textView;
        internal IOleCommandTarget NextTarget { get; set; }
        internal bool Added { get; set; }
        private IAdornmentLayer _adornmentLayer;
        private Task _publisher = null;
        private readonly ConfigurationRetriever _config = new ConfigurationRetriever();
        private Pulse pulse = new Pulse();


        public CommandFilter(IWpfTextView textView)
        {
            _textView = textView;
            _adornmentLayer = _textView.GetAdornmentLayer("CodeStatsClientLayer");
        }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (nCmdID != (uint)VSConstants.VSStd2KCmdID.TYPECHAR && nCmdID != (uint)VSConstants.VSStd2KCmdID.BACKSPACE)
                return NextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            pulse.IncrementExperience(_textView.TextBuffer.ContentType.DisplayName);

            if (_publisher == null)
            {
                _publisher = Task.Factory.StartNew(async () =>
                {
                    Thread.Sleep(10 * 1000); // Hopefully 10 seconds?

                    await pulse.Execute();
                    _publisher = null;
                });
            }

            return NextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return NextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }

}