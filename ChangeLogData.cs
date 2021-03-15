using System.Collections.Generic;
using System.Text;
using GG.ScriptableDataAsset;

namespace GG.ChangeLog
{
    public class ChangeLogTemp
    {
        public List<ChangeLogEntry> versions = new List<ChangeLogEntry>();
    }

    /// <summary>
    /// A change log contains all the versions created so far.
    /// ReSharper disable once ClassNeverInstantiated.Global
    /// </summary>
    public class ChangeLogData : ScriptableDataAsset<ChangeLogData>
    {
        /// <summary>
        /// An asset name
        /// </summary>
        protected override string AssetName { get; } = "Changelog";

        /// <summary>
        /// The list of versions
        /// </summary>
        public List<ChangeLogEntry> versions = new List<ChangeLogEntry>();
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ChangeLogEntry changeLogEntry in versions)
            {
                sb.Append(changeLogEntry.versionId);
                sb.AppendLine();
                foreach (string change in changeLogEntry.changes)
                {
                    sb.Append($"    {change}");
                    sb.AppendLine();
                }
            }
            return base.ToString();
        }
    }
}