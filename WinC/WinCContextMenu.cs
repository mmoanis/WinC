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
            // currently there is a bug with the sharp shell prompt and actually it can select only
            // 1 file at a time

            string files = string.Empty;
            foreach(string s in SelectedItemPaths)
            {
                files += s + " ";
            }

            EventLog.WriteEntry("WinC", "wincv0.3.0.7 called for files " + files, EventLogEntryType.Information);


            WinC winc = new WinC
            {
                /*CompilerArguments = "",   // read them from configuration file
                CompilerName = "MinGW", //
                PathToCompiler = @"C:\MinGW\bin",*/ // read path from configuration file
                /*RunArguments = runargs,*/
                // arguments are read from input file
                SourceFile = files
            };

            // initialize
            winc.Initialize();

            // run
            winc.Run();

            // write output
            winc.MakeOutputFile();

            // close
            winc.CleanUp();
        }
    }
}
