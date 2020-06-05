using System;
using System.Management.Automation;

namespace AsyncProgressReporter
{
    /// <summary>
    /// Subtype of PSCmdlet with async progress reporting.
    /// </summary>
    public class AsyncProgressPSCmdlet : PSCmdlet
    {
        private ProgressRecord _progressRecord;

        /// <summary>
        /// Displays an async PowerShell progress bar. Asynchronously waits for additional messages after each one is processed.
        /// </summary>
        /// <param name="reporter">A ProgressReporter instance used to marshal progress messages from a function to a cmdlet.</param>
        /// <param name="activity">The description of the activity for which progress is being reported.</param>
        /// <param name="description">The current status of the operation, e.g., "35 of 50 items Copied." or "95% completed." or "100 files purged."</param>
        protected void ShowProgress(ProgressReporter reporter, string activity, string description)
        {
            _progressRecord = new ProgressRecord(0, activity, description);
            foreach (var progressInfo in reporter.GetConsumingEnumerable())
            {
                Map(progressInfo, _progressRecord);
                WriteProgress(_progressRecord);

                if (!string.IsNullOrEmpty(progressInfo.VerboseOutput))
                    WriteVerbose(progressInfo.VerboseOutput);
            }
        }

        /// <summary>
        /// Marks the current progress bar as 'completed', which hides it from view.
        /// </summary>
        protected void HideProgress()
        {
            _progressRecord.RecordType = ProgressRecordType.Completed;
            WriteProgress(_progressRecord);
        }

        /// <summary>
        /// Maps ProgressInfo properties to ProgressRecord properties. Override in a subclass to customize the mappings as desired.
        /// </summary>
        protected virtual Action<ProgressInfo, ProgressRecord> Map
        {
            get
            {
                return (info, record) =>
                {
                    record.CurrentOperation = info.CurrentOperation;
                    record.StatusDescription = $"Completed {info.CompletedItems} of {info.TotalItems} items";
                    record.PercentComplete = info.PercentComplete();
                };
            }
        }
    }
}
