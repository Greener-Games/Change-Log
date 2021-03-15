using System.Collections.Generic;

namespace GG.ChangeLog
{
    /// <summary>
    /// A change log entry takes care of holding info about a version
    /// </summary>
 [System.Serializable]
    public class ChangeLogEntry
    {
        /// <summary>
        /// The version ID. Usually it looks like major.minor
        /// </summary>
        public string versionId;

        /// <summary>
        /// The name given to the version. Used to make communication easier.
        /// </summary>
        public string versionName;

        /// <summary>
        /// A list of all the changes that were made for this version
        /// </summary>
        public List<string> changes = new List<string>();
    }
}