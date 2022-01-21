using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SharpUpdate
{
    /// <summary>
    /// Performs many of the update functions.
    /// </summary>
    internal class Update
    {
        /// <summary>
        /// The string name of a temporary file in the %temp% folder.
        /// </summary>
        private string tempFile;
        /// <summary>
        /// The hash of the program, obtained from the online XML document.
        /// </summary>
        private string md5;
        /// <summary>
        /// Used to download the new program file.
        /// </summary>
        private WebClient webClient;
        /// <summary>
        /// Used to do background tasks.
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// The string path to a temporary file location in the %temp% folder.
        /// </summary>
        internal string TempFilePath
        {
            get { return this.tempFile; }
        }

        /// <summary>
        /// The hash of the program, obtained from the online XML document.
        /// </summary>
        internal string MD5
        {
            get { return this.md5; }
        }


        /// <summary>
        /// Starts the update process, and downloads the new executable program file.
        /// </summary>
        /// <param name="location">The link where the file is found online.  Obtained from the online XML document.</param>
        /// <param name="md5">The MD5 hash of the program.  Obtained from the online XML document.</param>
        internal Update(Uri location, string md5)
        {
            tempFile = Path.GetTempFileName();
            this.md5 = md5;
            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_Dowork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            try
            {
                webClient.DownloadFile(location, this.tempFile);
            }
            catch
            {

            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        /// <summary>
        /// Calculates the newly downloaded file's hash, and compares with the hash obtained from the XML document.
        /// If they don't coincide, the program closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_Dowork(object sender, DoWorkEventArgs e)
        {
            string file = ((string[])e.Argument)[0];
            string updateMd5 = ((string[])e.Argument)[1];
            e.Result = DialogResult.OK;
            if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
            {

            }
            else
            {
                MessageBox.Show("Cerrando...");
                Application.Exit();
            }
        }

        /// <summary>
        /// Ensures the file downloaded completely.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Error
            }
            else if (e.Cancelled)
            {
                //Aborted
            }
            else
            {
                bgWorker.RunWorkerAsync(new string[] { this.tempFile, this.md5 });
            }
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
        }
    }
}
