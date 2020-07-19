using System.Management.Automation;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "AsyncProgressBar")]
    public class AsyncProgressBar : AsyncProgressPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var reporter = new ProgressReporter();
            var fizzBuzz = new SlowFizzBuzz();

            // Start long running task...
            var task = fizzBuzz.Go(reporter);

            // Show progress bar and wait until task has completed...
            ShowBlockingProgress(reporter, "Searching for FizzBuzz...");
            task.Wait();

            // Dismiss progress bar
            HideBlockingProgress();
        }
    }
}