using System.Threading.Tasks;

namespace AsyncProgressReporter
{
    /// <summary>
    /// Enables thread-safe piping of progress updates to PowerShell cmdlets from functions running outside the cmdlet scope.
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// Updates the information currently displayed in the PowerShell progress bar.
        /// </summary>
        /// <param name="currentOperation">The current operation of the many required to accomplish the activity (such as "copying foo.txt").
        ///     Normally displayed below its associated progress bar, e.g., "deleting file foo.bar"</param>
        /// <param name="completedItems">The number of items that have already been processed.</param>
        /// <param name="totalItems">The total number of items to be processed.</param>
        /// <param name="verboseOutput">Additional output that should be displayed when the -Verbose parameter is provided.</param>
        Task UpdateProgress(string currentOperation, int completedItems, int totalItems, string verboseOutput = "");

        /// <summary>
        /// Marks the progress reporter instance as not accepting any more additions.
        /// </summary>
        /// <remarks>After a reporter has been marked as complete for adding, adding to the collection is not permitted and attempts to remove
        /// from the collection will not wait when the collection is empty.</remarks>
        void CompleteAdding();
    }
}