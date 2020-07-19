using System;
using System.Management.Automation;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "AsyncProgressBarWithChild")]
    public class AsyncProgressBarWithChild : AsyncProgressPSCmdlet
    {
        protected override void ProcessRecord()
        {
            ShowProgress("Running FizzBuzz 3 times");

            for (var i = 1; i <= 3; i++)
            {
                UpdateProgress(new ProgressInfo($"Run #{i}", i, 3));

                var reporter = new ProgressReporter();
                var fizzBuzz = new SlowFizzBuzz();

                // Start long running task...
                var task = fizzBuzz.Go(reporter);

                // Show progress bar and wait until task has completed...
                ShowBlockingProgress(reporter, "I can haz fizzbuzz?");
                task.Wait();

                // Dismiss progress bar
                HideBlockingProgress();
            }

            HideProgress();
        }
    }
}