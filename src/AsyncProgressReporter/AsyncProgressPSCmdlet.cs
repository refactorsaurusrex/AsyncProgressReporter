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
        /// Displays an async PowerShell progress bar. Asynchronously waits for additional messages after each one is processed. Note that this method blocks
        /// until CompleteAdding() is called on the ProgressReporter.
        /// </summary>
        /// <param name="reporter">A ProgressReporter instance used to marshal progress messages from a function to a cmdlet.</param>
        /// <param name="activity">The description of the activity for which progress is being reported.</param>
        /// <param name="initialDescription">The current status of the operation, e.g., "35 of 50 items Copied." or "95% completed." or "100 files purged."</param>
        /// <param name="activityId">The ID for the blocking progress record. The default is 0. Only override as necessary. Useful if 0 is already in use.</param>
        /// <param name="parentActivityId">The ID for a parent progress record. This is only necessary if you are manually creating your own parent progress records.
        /// See AsyncProgressBarWithChild2 class in demo project for an example of this.</param>
        protected void ShowProgressWait(ProgressReporter reporter, string activity, string initialDescription = "Getting started...", int activityId = 0, int? parentActivityId = null)
        {
            var blockingProgressRecord = new ProgressRecord(activityId, activity, initialDescription);

            if (parentActivityId.HasValue)
                blockingProgressRecord.ParentActivityId = parentActivityId.Value;
            else if (_progressRecord != null)
                blockingProgressRecord.ParentActivityId = _progressRecord.ActivityId;

            foreach (var progressInfo in reporter.GetConsumingEnumerable())
            {
                Map(progressInfo, blockingProgressRecord);
                WriteProgress(blockingProgressRecord);

                if (!string.IsNullOrEmpty(progressInfo.VerboseOutput))
                    WriteVerbose(progressInfo.VerboseOutput);
            }

            blockingProgressRecord.RecordType = ProgressRecordType.Completed;
            WriteProgress(blockingProgressRecord);
        }

        /// <summary>
        /// Displays a normal, non-blocking, non-asynchronous PowerShell progress bar with a simple pre-configured layout. 
        /// </summary>
        /// <param name="activity">The description of the activity for which progress is being reported.</param>
        /// <param name="initialDescription">The current status of the operation, e.g., "35 of 50 items Copied." or "95% completed." or "100 files purged."</param>
        /// <param name="activityId">The ID for the blocking progress record. The default is 1. Only override as necessary. Useful if 1 is already in use.</param>
        protected void ShowProgress(string activity, string initialDescription = "Getting started...", int activityId = 1)
        {
            _progressRecord = new ProgressRecord(activityId, activity, initialDescription);
            WriteProgress(_progressRecord);
        }

        /// <summary>
        /// Maps the specified ProgressInfo to a ProgressRecord using the Action provided by the Map property, then writes the resulting record to the non-blocking
        /// progress record.
        /// </summary>
        /// <param name="progress"></param>
        protected virtual void UpdateProgress(ProgressInfo progress)
        {
            Map(progress, _progressRecord);
            WriteProgress(_progressRecord);
        }

        /// <summary>
        /// Marks the current non-blocking progress bar as 'completed', which hides it from view.
        /// </summary>
        protected void HideProgress()
        {
            if (_progressRecord != null)
            {
                _progressRecord.RecordType = ProgressRecordType.Completed;
                WriteProgress(_progressRecord);
                _progressRecord = null;
            }
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
