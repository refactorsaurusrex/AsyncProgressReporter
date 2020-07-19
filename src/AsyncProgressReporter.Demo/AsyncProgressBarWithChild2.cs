using System.Management.Automation;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "AsyncProgressBarWithChild2")]
    public class AsyncProgressBarWithChild2 : AsyncProgressPSCmdlet
    {
        protected override void ProcessRecord()
        {
            // Create your own ProgressRecord
            var progressRecord = new ProgressRecord(12, "Running FizzBuzz Twice", "Initial status...");
            WriteProgress(progressRecord);

            for (var i = 1; i <= 3; i++)
            {
                // Format progress information any way you like.
                progressRecord.CurrentOperation = $"Evaluating the number {i}.";
                progressRecord.PercentComplete = (int)(i / 3d * 100);
                WriteProgress(progressRecord);

                var reporter = new ProgressReporter();
                var fizzBuzz = new SlowFizzBuzz();

                // Start long running task...
                var task = fizzBuzz.Go(reporter);

                // Show progress bar and wait until task has completed. Pass in ActivityId of parent.
                ShowBlockingProgress(reporter, "I can haz fizzbuzz?", parentActivityId: progressRecord.ActivityId);
                task.Wait();

                // Dismiss progress bar
                HideBlockingProgress();
            }

            HideProgress();
        }
    }
}