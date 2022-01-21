using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SharpUpdate
{
    /// <summary>
    /// Performs the required update work, such as XML check, update download, hash verification, and final update tasks.
    /// </summary>
    public class SharpUpdater
    {
        /// <summary>
        /// Contains the information required for updating.
        /// </summary>
        private ISharpUpdatable applicationInfo;
        /// <summary>
        /// Performs the required background tasks asynchronously.
        /// </summary>
        private BackgroundWorker bgWorker;


        /// <summary>
        /// Creates a new update task/object with the required application info.
        /// </summary>
        /// <param name="applicationInfo">The ISharpUpdatabale object with the current application info.</param>
        public SharpUpdater(ISharpUpdatable applicationInfo)
        {
            this.applicationInfo = applicationInfo;
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        /// <summary>
        /// Performs the update work.
        /// </summary>
        public void DoUpdate()
        {
            if (!this.bgWorker.IsBusy)
            {
                this.bgWorker.RunWorkerAsync(this.applicationInfo);
            }
        }

        /// <summary>
        /// Compares current running version with online XML's current/newest version number, and downloads if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                SharpUpdateXml update = (SharpUpdateXml)e.Result;
                if (update != null && update.IsNewerThan(this.applicationInfo.ApplicationAssembly.GetName().Version))
                {
                    this.DownloadUpdate(update);
                }
            }
        }

        /// <summary>
        /// Downloads newest version, calculates and verifies hash, and closes the program to perform the update.
        /// </summary>
        /// <param name="update">The online XML object to analyze.</param>
        private void DownloadUpdate(SharpUpdateXml update)
        {
            Update up = new Update(update.Uri, update.MD5);
            string currentPath = this.applicationInfo.ApplicationAssembly.Location;
            string file = up.TempFilePath;
            string updateMd5 = up.MD5;
            if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
            {
                MessageBox.Show("Cerrando...");
                Application.Exit();
            }
            else
            {
                UpdateApplication(up.TempFilePath, currentPath, update.LaunchArgs);
                Process.GetCurrentProcess().Kill();
            }
            
        }

        /// <summary>
        /// Runs a hidden CMD window that allows this instance of the program to be terminated, copies the new update file
        /// and replaces the old one, and runs the new version.
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <param name="currentPath"></param>
        /// <param name="newPath"></param>
        /// <param name="launchArgs"></param>
        private void UpdateApplication(string tempFilePath, string currentPath, string launchArgs)
        {
            string argument = "/C Choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & Choice /C Y /N /D Y /T 2 & "
                + "Move /Y \"{1}\" \"{2}\" & Start \"\" /D \"{3}\" \"{4}\" {5}";
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = string.Format(argument, currentPath, tempFilePath, currentPath,
                Path.GetDirectoryName(currentPath), Path.GetFileName(currentPath), launchArgs);
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.FileName = "cmd.exe";
            info.UseShellExecute = false;
            Process.Start(info);
        }

        /// <summary>
        /// Downloads the XML update file, if available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ISharpUpdatable application = (ISharpUpdatable)e.Argument;
            if (!SharpUpdateXml.ExistsOnServer(application.UpdateXmlLocation))
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = SharpUpdateXml.Parse(application.UpdateXmlLocation, application.ApplicationID);
            }
        }
    }
}
