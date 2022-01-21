using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Compilador.Classes
{
    /// <summary>
    /// This class is used to write compiled code, compile it (with .NET compiler), and then execute
    /// the compiled program.
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Specifies if the code was successfully recompiled (with the .NET compiler) or not.
        /// </summary>
        public static bool successfulCompilation;
        /// <summary>
        /// Specifies if the code is attempting to recover from errors (true) or not (false).
        /// -The first compile will always be attempted with this value as true.
        /// -During the second attempt, this value will be false.
        /// It will be known that the code is attempting to recompile a second time if this value is false.
        /// </summary>
        private static bool errorRecovery;
        

        /// <summary>
        /// Attempts to write, recompile, and execute source code that has been initially compiled.
        /// </summary>
        /// <param name="code">The compiled source code.</param>
        public static void Execute(string code)
        {
            bool continueCompilation = !errorRecovery;
            if (Semantic.errorCount > 0 && errorRecovery)
            {
                DialogResult dr = MessageBox.Show("Se ha detectado errores semánticos.  ¿Desea continuar con la compilación?",
                    "Errores detectados", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                continueCompilation = dr == DialogResult.Yes;
                if (continueCompilation == false)
                {
                    successfulCompilation = false;
                }
            }
            if ((Semantic.errorCount == 0) || continueCompilation)
            {
                string path;
                if (Semantic.XNAMode)
                {
                    path = ".\\Archivos\\WindowsGame1\\WindowsGame1\\WindowsGame1\\Game1.cs";
                }
                else if (Semantic.debugging)
                {
                    path = ".\\Archivos\\TreeApp\\TreeApp\\Form1.cs";
                }
                else
                {
                    path = "program.cs";
                }
                StreamWriter write = new StreamWriter(path);
                write.Write(code);
                write.Close();
                Thread.Sleep(200);
                ProcessStartInfo info = new ProcessStartInfo();
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.CreateNoWindow = true;
                info.FileName = "cmd.exe";
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                if (!Semantic.XNAMode && !Semantic.debugging)
                {
                    ConsoleCompile(info);
                }
                else if (Semantic.XNAMode)
                {
                    XNACompile(info);
                }
                else
                {
                    DebugCompile(info);
                }
            }
        }

        /// <summary>
        /// Performs the necessary steps to compile a console application.
        /// </summary>
        /// <param name="info">The ProcessStartInfo object required to execute the recompilation process.</param>
        private static void ConsoleCompile(ProcessStartInfo info)
        {
            info.Arguments = "/C C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\csc.exe program.cs";
            string result = ProcessExecute(info);
            successfulCompilation = !result.Contains("error");
            if (successfulCompilation)
            {
                Process.Start("program.exe");
            }
            else
            {
                if (!errorRecovery)
                {
                    MessageBox.Show(result.Substring(result.IndexOf("error")));
                }
            }
        }

        /// <summary>
        /// Performs the necessary steps to compile a graphical XNA application.
        /// </summary>
        /// <param name="info">The ProcessStartInfo object required to execute the recompilation process.</param>
        private static void XNACompile(ProcessStartInfo info)
        {
            info.Arguments =
                        "/C C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\msbuild.exe .\\Archivos\\WindowsGame1"
                        + "\\WindowsGame1.sln";
            string result = ProcessExecute(info);
            successfulCompilation = !result.Contains("Build FAILED");
            if (successfulCompilation)
            {
                Process.Start(".\\Archivos\\WindowsGame1\\WindowsGame1\\WindowsGame1\\bin\\x86\\Debug"
                    + "\\WindowsGame1.exe");
            }
            else
            {
                if (!errorRecovery)
                {
                    MessageBox.Show(result.Substring(result.IndexOf("Build FAILED")));
                }
            }
        }

        /// <summary>
        /// Performs the necessary steps to compile a console application with a TreeView debugging interface.
        /// </summary>
        /// <param name="info">The ProcessStartInfo object required to execute the recompilation process.</param>
        private static void DebugCompile(ProcessStartInfo info)
        {
            info.Arguments = "/C C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\msbuild.exe .\\Archivos\\TreeApp"
                        + "\\TreeApp.sln";
            string result = ProcessExecute(info);
            successfulCompilation = !result.Contains("Build FAILED");
            if (successfulCompilation)
            {
                Process.Start(".\\Archivos\\TreeApp\\TreeApp\\bin\\Debug\\TreeApp.exe");
            }
            else
            {
                if (!errorRecovery)
                {
                    MessageBox.Show(result.Substring(result.IndexOf("Build FAILED")));
                }
            }
        }

        /// <summary>
        /// Executes the recompilation process through CMD.
        /// </summary>
        /// <param name="info">The ProcessStartInfo object with the required parameters to recompile code.</param>
        /// <returns>Returns the resulting message after recompilation.</returns>
        private static string ProcessExecute(ProcessStartInfo info)
        {
            string result;
            using (Process process = Process.Start(info))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// Sets the error recovery status.
        /// </summary>
        /// <param name="errorRecovery">Specifies if the code is attempting to recover from errors (true) or not (false).
        /// -The first compile will always be attempted with this value as true.
        /// -During the second attempt, this value will be false.
        /// It will be known that the code is attempting to recompile a second time if this value is false.</param>
        public static void SetErrorRecovery(bool errorRecovery)
        {
            Executor.errorRecovery = errorRecovery;
        }
    }
}
