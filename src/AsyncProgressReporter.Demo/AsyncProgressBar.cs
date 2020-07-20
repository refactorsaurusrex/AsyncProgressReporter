using System.Management.Automation;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "AsyncProgressBar")]
    public class AsyncProgressBar : AsyncProgressPSCmdlet
    {
        [Parameter]
        public SwitchParameter ThrowException { get; set; }

        protected override void ProcessRecord()
        {
            var reporter = new ProgressReporter();
            var fizzBuzz = new SlowFizzBuzz();

            // Start long running task...
            var task = fizzBuzz.Go(reporter, ThrowException);

            // Show progress bar and wait until task has completed...
            ShowProgressWait(reporter, "Searching for FizzBuzz...");
            task.Wait();
        }
    }
}