using System;
using System.Threading.Tasks;

namespace AsyncProgressReporter.Demo
{
    public class SlowFizzBuzz
    {
        public async Task Go(IProgressReporter reporter, bool throwException)
        {
            var boom = new Random().Next(15, 90);

            try
            {
                for (var i = 1; i <= 100; i++)
                {
                    if (throwException && i == boom)
                        throw new Exception("SlowFizzBuzz encountered an error.");

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
}