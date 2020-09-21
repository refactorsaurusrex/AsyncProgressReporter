using System.Management.Automation;
using System.Threading.Tasks;

namespace AsyncProgressReporter.Demo
{
    [Cmdlet(VerbsLifecycle.Start, "ElapsedTimeProgressBar")]
    public class ElapsedTimeProgressBar : AsyncProgressPSCmdlet
    {
        [Parameter]
        [ValidateRange(1, int.MaxValue)]
        public int Seconds { get; set; } = 5;

        protected override void ProcessRecord()
        {
            var task = Task.Run(() =>
            {
                Task.Delay(Seconds * 1000).Wait();
                return $"The task completed after {Seconds} seconds";
            });
            ShowElapsedTimeProgress(task, "Long Running Task", "Please wait, this will only take a few seconds...");
            WriteObject(task.Result);
            HideProgress();
        }
    }
}