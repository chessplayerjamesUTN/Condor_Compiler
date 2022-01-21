using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace SharpUpdate
{
    /// <summary>
    /// Object used during update process, with required information.
    /// </summary>
    public interface ISharpUpdatable
    {
        /// <summary>
        /// Stores the Application's Name.
        /// </summary>
        string ApplicationName { get; }
        /// <summary>
        /// Stores the Application's ID.
        /// </summary>
        string ApplicationID { get; }
        /// <summary>
        /// Stores the Application Assembly information, including ID.
        /// </summary>
        Assembly ApplicationAssembly { get; }
        /// <summary>
        /// The location where the update XML is.
        /// </summary>
        Uri UpdateXmlLocation { get; }
    }
}
