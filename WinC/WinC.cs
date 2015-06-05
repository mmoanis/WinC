using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace WinC
{
    class WinC
    {
        private string sLogTag = "WinC";
        private string file;
        private string pathToCompiler;
        private string compilerArguments;
        private string compilerName;
        private string runArguments;
        private string outputfile;
        private bool makeInputFile;
        private ArrayList errors;
        private ArrayList output;

        public string SourceFile
        {
            get
            {
                return file;
            }

            set
            {
                file = value;
            }
        }

        public string PathToCompiler
        {
            get
            {
                return pathToCompiler;
            }

            set
            {
                pathToCompiler = value;
            }
        }

        public string CompilerArguments
        {
            get
            {
                return compilerArguments;
            }

            set
            {
                compilerArguments = value;
            }
        }

        public string CompilerName
        {
            get
            {
                return compilerName;
            }

            set
            {
                compilerName = value;
            }
        }

        public string RunArguments
        {
            get
            {
                return runArguments;
            }

            set
            {
                runArguments = value;
            }
        }

        public ArrayList Errors
        {
            get
            {
                return errors;
            }
        }

        public ArrayList Output
        {
            get
            {
                return output;
            }
        }

        public void Initialize()
        {
            errors = new ArrayList();
            output = new ArrayList();

            makeInputFile = false;

            // check the input file
            if(!File.Exists(Path.GetDirectoryName(file) + "\\input.txt"))
            {
                EventLog.WriteEntry(sLogTag, "making input file", EventLogEntryType.Information);
                makeInputFile = true;
                FileStream fs = File.Create(Path.GetDirectoryName(SourceFile) + "\\input.txt");
                fs.Close();
            }

            compilerArguments = file + " " + compilerArguments + " -o temp.exe";
            // check the file language and adjust the compilation string
            if (file[file.Length - 1] == 'c')
            {
                compilerArguments = "gcc " + compilerArguments;
            }
            else
            {
                compilerArguments = "g++ " + compilerArguments;
            }

            outputfile = Path.GetDirectoryName(file) + "\\output.txt";
            string inputFile = Path.GetDirectoryName(file) + "\\input.txt";

            // direct the input and output streams of the program to input/output.txt
            runArguments = "temp " + runArguments + " < " + inputFile;

            EventLog.WriteEntry(sLogTag, "compiling with " + compilerArguments, EventLogEntryType.Information);
            EventLog.WriteEntry(sLogTag, "run with " + runArguments, EventLogEntryType.Information);
        }

        public void Run()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = pathToCompiler

            };

            p.ErrorDataReceived += (s, e) => ErrorDataReceivedHandler(s, e);
            p.OutputDataReceived += (s, e) => OutputDataReceivedHandler(s, e);

            p.Start();

            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            // compile with argus
            p.StandardInput.WriteLine(compilerArguments);

            // run with args
            p.StandardInput.WriteLine(runArguments);

            // exit cmd
            p.StandardInput.WriteLine("exit");

            p.WaitForExit();

            p.Close();
        }

        public void MakeOutputFile()
        {
            FileStream outputFileStream = File.Create(outputfile);
            StreamWriter writer = new StreamWriter(outputFileStream, System.Text.Encoding.ASCII);

            // write output only if no errors
            if (errors.Count != 0)
            {
                EventLog.WriteEntry(sLogTag, "writing errors " + errors.Count, EventLogEntryType.Information);

                // hide the last line from the output file
                for (int i = 0; i < errors.Count - 2; i++)
                {
                    writer.WriteLine(errors[i]);
                }
            }
            else
            {
                EventLog.WriteEntry(sLogTag, "writing output", EventLogEntryType.Information);

                if (output.Count != 0)
                {
                    // hide the first 6 lines from the output file
                    for (int i = 6; i < output.Count - 3; i++)
                    {
                        writer.WriteLine(output[i]);
                    }
                }
            }
            writer.Close();
        }

        /// <summary>
        /// Remove the temp executable file.
        /// </summary>
        public void CleanUp()
        {
            if (File.Exists(pathToCompiler + "\\temp.exe"))
            {
                File.Delete(pathToCompiler + "\\temp.exe");
            }

            // remove input file if it was created by the program
            if (makeInputFile)
                File.Delete(Path.GetDirectoryName(file) + "\\input.txt");
        }

        private void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                errors.Add(e.Data);
        }

        private void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            output.Add(e.Data);
        }
    }
}
