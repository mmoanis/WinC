using System;
using SharpShell.SharpContextMenu;
using System.Diagnostics;
using SharpShell.Attributes;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WinC
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".c", ".cpp", ".cc", ".cxx")]
    public class WinCContextMenu : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return true;
        }

        /// <summary>
        /// Creates the context menu.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            // for debuging
            if (!EventLog.SourceExists("WinC"))
                EventLog.CreateEventSource("WinC", "Application");

            var menu = new ContextMenuStrip();

            // default execute item
            var itemRun = new ToolStripMenuItem
            {
                Text = "Run!"
            };

            var itemRunWithArgs = new ToolStripMenuItem
            {
                Text = "Run with args"
            };

            itemRun.Click += (sender, args) => RunFile("");
            itemRunWithArgs.Click += (sender, args) =>
                {
                    FormInputPrompt frmInputPrompt = new FormInputPrompt("Run arguments", "arguments:");
                    if (frmInputPrompt.ShowDialog() == DialogResult.OK)
                        RunFile(frmInputPrompt.InputText);
                };

            menu.Items.Add(itemRun);
            menu.Items.Add(itemRunWithArgs);
            return menu;
        }

        /// <summary>
        /// Compile and run the selected file.
        /// </summary>
        /// <returns></returns>
        private void RunFile(string runargs)
        {
            foreach(string s in SelectedItemPaths)
            {
                EventLog.WriteEntry("WinC", "wincv0.2 called to file " + s, EventLogEntryType.Information);
                WinC winc = new WinC
                {
                    CompilerArguments = "",
                    CompilerName = "MinGW",
                    PathToCompiler = @"C:\MinGW\bin",
                    RunArguments = runargs,
                    SourceFile = s
                };

                winc.Initialize();
                winc.Run();
                winc.MakeOutputFile();
                winc.CleanUp();
            }
        }
    }
}
