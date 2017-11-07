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

namespace CodeStats.Client
{
    class CommandFilter : IOleCommandTarget
    {
        private IWpfTextView _textView;
        internal IOleCommandTarget NextTarget { get; set; }
        internal bool Added { get; set; }
        private IAdornmentLayer _adornmentLayer;

        internal Dictionary<string, int> Experiences = new Dictionary<string, int>();
        private Task _publisher = null;


        public CommandFilter(IWpfTextView textView)
        {
            _textView = textView;
            _adornmentLayer = _textView.GetAdornmentLayer("CodeStatsClientLayer");
        }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (nCmdID != (uint)VSConstants.VSStd2KCmdID.TYPECHAR && nCmdID != (uint)VSConstants.VSStd2KCmdID.BACKSPACE)
                return NextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            incrementOrAdd(_textView.TextBuffer.ContentType.DisplayName);
            MessageBox.Show("Added one to: " + _textView.TextBuffer.ContentType.DisplayName);

            if (_publisher == null)
            {
                _publisher = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(10 * 1000); // Hopefully 10 seconds?

                    string experiences = "";
                    foreach (var experience in Experiences)
                    {
                        experiences += $"In Language {experience.Key}, you earned {experience.Value} xp\n";
                    }
                    MessageBox.Show(experiences);
                    Experiences.Clear();
                    _publisher = null;
                });
            }

            return NextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        private void incrementOrAdd(string lang)
        {
            if (Experiences.ContainsKey(lang))
            {
                Experiences[lang]++;
            }
            else
            {
                Experiences.Add(lang, 1);
            }
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return NextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }

}