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

            itemRun.Click += (sender, args) => RunFile();
            menu.Items.Add(itemRun);
            return menu;
        }

        /// <summary>
        /// Compile and run the selected file.
        /// </summary>
        /// <returns></returns>
        private void RunFile()
        {
            foreach(string s in SelectedItemPaths)
            {
                EventLog.WriteEntry("WinC", "winc called to file " + s, EventLogEntryType.Information);
                WinC winc = new WinC
                {
                    CompilerArguments = "",
                    CompilerName = "MinGW",
                    PathToCompiler = @"C:\MinGW\bin",
                    RunArguments = "",
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
