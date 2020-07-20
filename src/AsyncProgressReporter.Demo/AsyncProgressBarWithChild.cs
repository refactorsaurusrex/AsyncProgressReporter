using System.Management.Automation;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "AsyncProgressBarWithChild")]
    public class AsyncProgressBarWithChild : AsyncProgressPSCmdlet
    {
        [Parameter]
        public SwitchParameter ThrowException { get; set; }

        protected override void ProcessRecord()
        {
            ShowProgress("Running FizzBuzz 3 times");

            for (var i = 1; i <= 3; i++)
            {
                UpdateProgress(new ProgressInfo($"Run #{i}", i, 3));

                var reporter = new ProgressReporter();
                var fizzBuzz = new SlowFizzBuzz();

                // Start long running task...
                var task = fizzBuzz.Go(reporter, ThrowException);

                // Show progress bar and wait until task has completed...
                ShowProgressWait(reporter, "I can haz fizzbuzz?");
                task.Wait();
            }

            HideProgress();
        }
    }
}   