using System;

namespace AsyncProgressReporter
{
    /// <summary>
    /// A progress update message.
    /// </summary>
    public class ProgressInfo
    {
        /// <summary>
        /// Create a new progress update message.
        /// </summary>
        /// <param name="currentOperation">The current operation of the many required to accomplish the activity (such as "copying foo.txt").
        /// Normally displayed below its associated progress bar, e.g., "deleting file foo.bar"</param>
        /// <param name="totalItems">The total number of items to be processed.</param>
        /// <param name="completedItems">The number of items that have already been processed.</param>
        /// <param name="verboseOutput">Additional output that should be displayed when the -Verbose parameter is provided.</param>
        public ProgressInfo(string currentOperation, int totalItems, int completedItems, string verboseOutput = "")
        {
            CurrentOperation = currentOperation;
            TotalItems = totalItems;
            CompletedItems = completedItems;
            VerboseOutput = verboseOutput;
        }

        /// <summary>
        /// The current operation of the many required to accomplish the activity (such as "copying foo.txt").
        /// Normally displayed below its associated progress bar, e.g., "deleting file foo.bar"
        /// </summary>
        public string CurrentOperation { get; set; }

        /// <summary>
        /// The total number of items to be processed.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// The number of items that have already been processed.
        /// </summary>
        public int CompletedItems { get; set; }

        /// <summary>
        /// Additional output that should be displayed when the -Verbose parameter is provided.
        /// </summary>
        public string VerboseOutput { get; set; }

        /// <summary>
        /// Returns the current percentage of tasks that have been completed.
        /// </summary>
        public int PercentComplete() => (int)Math.Truncate(CompletedItems / (double)TotalItems * 100);
    }
}