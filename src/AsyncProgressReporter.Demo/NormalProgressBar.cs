using System.Management.Automation;
using System.Threading.Tasks;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "NormalProgressBar")]
    public class NormalProgressBar : AsyncProgressPSCmdlet
    {
        protected override void ProcessRecord()
        {
            ShowProgress("Normal Progress Bar");

            for (var i = 1; i <= 25; i++)
            {
                UpdateProgress(new ProgressInfo($"Now on number {i}...", i, 25));
                Task.Delay(500).Wait();
            }

            HideProgress();
        }
    }
}
