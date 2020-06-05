using System.Management.Automation;
using System.Threading.Tasks;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Invoke, "LongRunningThing")]
    public class LongRunningCmdlet : AsyncProgressPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var reporter = new ProgressReporter();
            var thing = new LongRunningThing();

            // Start long running task...
            var task = thing.Run(reporter);

            int result;
            try
            {
                // Show progress bar and wait until task has completed...
                ShowProgress(reporter, "What is the meaning of life?", "Searching...");
                result = task.Result;
            }
            finally
            {
                // Ensure HideProgress is always called...
                HideProgress();
            }

            // Print result.
            WriteObject($"The meaning of life is {result}!");
        }

    }

    public class LongRunningThing
    {
        public async Task<int> Run(IProgressReporter reporter)
        {
            const int max = 57;
            const int answer = 42;
            for (var i = 1; i < max; i++)
            {
                if (i == answer)
                {
                    await reporter.UpdateProgress("Hmm!", max, i, "More words here");
                    Task.Delay(3000).Wait(); // Simulate a moment of clarity and insight
                }
                else
                {
                    await reporter.UpdateProgress($"Examining scenario {i}....", max, i, "More words here");
                    Task.Delay(100).Wait(); // Simulate a long running task
                }
                
            }

            reporter.CompleteAdding();
            return answer;
        }
    }
}
