using System;
using System.Net;
using System.Xml;

namespace SharpUpdate
{
    /// <summary>
    /// The object that contains the required information obtained from the online XML update document.
    /// </summary>
    internal class SharpUpdateXml
    {
        /// <summary>
        /// The current most recent version number.
        /// </summary>
        private Version version;
        /// <summary>
        /// The URI of the most recent program version.
        /// </summary>
        private Uri uri;
        /// <summary>
        /// The name that should be applied to the file when downloading.
        /// </summary>
        private string fileName;
        /// <summary>
        /// The MD5 hash of the file.
        /// </summary>
        private string md5;
        /// <summary>
        /// A brief description of the latest update.
        /// </summary>
        private string description;
        /// <summary>
        /// Launch arguments to include, if necessary.
        /// </summary>
        private string launchArgs;

        internal Version Version
        {
            get { return this.version; }
        }

        internal Uri Uri
        {
            get { return this.uri; }
        }
        internal string FileName
        {
            get { return this.fileName; }
        }

        internal string MD5
        {
            get { return this.md5; }
        }

        internal string Description
        {
            get { return this.description; }
        }

        internal string LaunchArgs
        {
            get { return this.launchArgs; }
        }

        /// <summary>
        /// Constructor that creates a new, complete instance of this class.
        /// </summary>
        /// <param name="version">The current most recent version number.</param>
        /// <param name="uri">The URI of the most recent program version.</param>
        /// <param name="fileName">The name that should be applied to the file when downloading.</param>
        /// <param name="md5">The MD5 hash of the file.</param>
        /// <param name="description">A brief description of the latest update.</param>
        /// <param name="launchArgs">Launch arguments to include, if necessary.</param>
        internal SharpUpdateXml(Version version, Uri uri, string fileName, string md5, string description, string launchArgs)
        {
            this.version = version;
            this.uri = uri;
            this.fileName = fileName;
            this.md5 = md5;
            this.description = description;
            this.launchArgs = launchArgs;
        }

        /// <summary>
        /// Compares the current running version with the XML newest version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal bool IsNewerThan(Version version)
        {
            return this.version > version;
        }

        /// <summary>
        /// Ensures file location exists before downloading.
        /// </summary>
        /// <param name="location">The location of the online XML document.</param>
        /// <returns></returns>
        internal static bool ExistsOnServer(Uri location)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(location.AbsoluteUri);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                resp.Close();
                return resp.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtains and parses the pertinent XML information to the object.
        /// </summary>
        /// <param name="location">The location where the online XML document is.</param>
        /// <param name="appID">The Application ID.</param>
        /// <returns></returns>
        internal static SharpUpdateXml Parse(Uri location, string appID)
        {
            Version version = null;
            string url = "", fileName = "", md5 = "", description = "", launchArgs = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(location.AbsoluteUri);
                XmlNode node = doc.DocumentElement.SelectSingleNode("//update[@appId='" + appID + "']");
                if (node == null) return null;
                version = Version.Parse(node["version"].InnerText);
                url = node["url"].InnerText;
                fileName = node["fileName"].InnerText;
                md5 = node["md5"].InnerText;
                description = node["description"].InnerText;
                launchArgs = node["launchArgs"].InnerText;
                return new SharpUpdateXml(version, new Uri(url), fileName, md5, description, launchArgs);
            }
            catch
            {
                return null;
            }
        }
    }
}
