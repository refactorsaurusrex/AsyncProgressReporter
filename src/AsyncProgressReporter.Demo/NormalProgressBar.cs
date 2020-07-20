using System;
using System.Management.Automation;
using System.Threading.Tasks;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "NormalProgressBar")]
    public class NormalProgressBar : AsyncProgressPSCmdlet
    {
        [Parameter]
        public SwitchParameter ThrowException { get; set; }

        protected override void ProcessRecord()
        {
            var boom = new Random().Next(3, 20);
            ShowProgress("Normal Progress Bar");

            for (var i = 1; i <= 25; i++)
            {
                if (ThrowException && i == boom)
                    throw new Exception("An error was encountered.");

                UpdateProgress(new ProgressInfo($"Now on number {i}...", i, 25));
                Task.Delay(500).Wait();
            }

            HideProgress();
        }
    }
}
