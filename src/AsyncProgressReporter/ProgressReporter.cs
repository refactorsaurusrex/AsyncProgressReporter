using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgressReporter
{
    /// <inheritdoc cref="IProgressReporter" />
    public class ProgressReporter : BlockingCollection<ProgressInfo>, IProgressReporter
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <inheritdoc />
        public async Task UpdateProgress(string currentOperation, int completedItems, int totalItems, string verboseOutput = "")
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() => Add(new ProgressInfo(currentOperation, completedItems, totalItems, verboseOutput)));
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}