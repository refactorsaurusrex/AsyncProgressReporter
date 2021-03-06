# AsyncProgressReporter
Asynchronously pipe progress messages from long running functions to PowerShell cmdlets.

## What is this?

Have you never wanted to asynchronously invoke a long running function from a PowerShell binary cmdlet and report status updates to a progress bar? 

It's not exactly a straight forward thing to do. 

This package makes it easy. 

## Installation

[Install from nuget](https://www.nuget.org/packages/AsyncProgressReporter/):

```powershell
Install-Package AsyncProgressReporter
```

## Example Usage

> There are several more fully-functional examples of how to use the various progress bar functions in [the demo project](https://github.com/refactorsaurusrex/AsyncProgressReporter/tree/master/src/AsyncProgressReporter.Demo). Instructions on how to build and run the project are below.

### Basic Async Progress Bar

```csharp
// Implement your cmdlet by inheriting from AsyncProgressPSCmdlet,
// which derives from PSCmdlet.
[Cmdlet(VerbsLifecycle.Start, "AsyncProgressBar")]
public class AsyncProgressBar : AsyncProgressPSCmdlet
{
    protected override void ProcessRecord()
    {
        // Create a new ProgressReporter.
        var reporter = new ProgressReporter();
        
        // Start your long running function and pass in ProgressReporter instance.
        var fizzBuzz = new SlowFizzBuzz();
        var task = fizzBuzz.Go(reporter);

        // Show progress bar and wait until task finishes.
        // Messages submitted to the ProgressReporter are
        // immediately displayed.
        ShowProgressWait(reporter, "Searching for FizzBuzz...");
        task.Wait();
    }
}

public class SlowFizzBuzz
{
    public async Task Go(IProgressReporter reporter)
    {
        try
        {
            for (var i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    await reporter.UpdateProgress("FIZZBUZZ!!", i, 100);
                    await Task.Delay(1000);
                }
                else if (i % 3 == 0)
                {
                    await reporter.UpdateProgress("FIZZ!", i, 100);
                    await Task.Delay(500);
                }
                else if (i % 5 == 0)
                {
                    await reporter.UpdateProgress("BUZZ!", i, 100);
                    await Task.Delay(500);
                }
                else
                {
                    await reporter.UpdateProgress($"{i} is boring.", i, 100);
                    await Task.Delay(50);
                }
            }
        }
        finally
        {
            // CompleteAdding() must be called or ShowBlockingProgress() will never return.
            reporter.CompleteAdding();
        }
    }
}
```

### Elapsed Time Progress Bar

```powershell
[Cmdlet(VerbsLifecycle.Start, "ElapsedTimeProgressBar")]
public class ElapsedTimeProgressBar : AsyncProgressPSCmdlet
{
    [Parameter]
    [ValidateRange(1, int.MaxValue)]
    public int Seconds { get; set; } = 5;

    protected override void ProcessRecord()
    {
        // Start a long running task...
        var task = Task.Run(() =>
        {
            Task.Delay(Seconds * 1000).Wait();
            return $"The task completed after {Seconds} seconds";
        });
        
        // Show a progress bar that displays the task's elapsed running time.
        // Method blocks until task is finished.
        ShowElapsedTimeProgress(task, "Long Running Task", "Please wait, this will only take a few seconds...");
        WriteObject(task.Result);
        HideProgress();
    }
}
```



## Running and Debugging

1. Clone this repo.
2. Update [the launch settings](https://github.com/refactorsaurusrex/AsyncProgressReporter/blob/master/src/AsyncProgressReporter.Demo/Properties/launchSettings.json) for the demo project. Specifically, change the values for `executablePath` and `workingDirectory` so they are appropriate for your machine.
3. Press F5.

## Questions / Bugs / Suggestions

[Open an issue](https://github.com/refactorsaurusrex/AsyncProgressReporter/issues) and let's chat about it! If you're interested in making a contribution, [start here](https://github.com/refactorsaurusrex/AsyncProgressReporter/blob/master/CONTRIBUTING.MD). 