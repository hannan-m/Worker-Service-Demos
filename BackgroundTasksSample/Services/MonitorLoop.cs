using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTasksSample.Services
{
    #region snippet_Monitor

    public class MonitorLoop
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;

        public MonitorLoop(IBackgroundTaskQueue taskQueue,
            ILogger<MonitorLoop> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void StartMonitorLoop()
        {
            _logger.LogInformation("Monitor Loop is starting.");

            // Run a console user input loop in a background thread
            Task.Run(Monitor, _cancellationToken);
        }

        public void Monitor()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var keyStroke = Console.ReadKey();

                if (keyStroke.Key == ConsoleKey.W)
                {
                    // Enqueue a background work item
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        // Simulate three 5-second tasks to complete
                        // for each enqueued work item

                        var delayLoop = 0;
                        var guid = Guid.NewGuid().ToString();

                        _logger.LogInformation(
                            "Queued Background Task {Guid} is starting.", guid);

                        while (!token.IsCancellationRequested && delayLoop < 3)
                        {
                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(5), token);
                            }
                            catch (OperationCanceledException)
                            {
                                // Prevent throwing if the Delay is cancelled
                            }

                            delayLoop++;

                            _logger.LogInformation(
                                "Queued Background Task {Guid} is running. " +
                                "{DelayLoop}/3", guid, delayLoop);
                        }

                        _logger.LogInformation(
                            delayLoop == 3
                                ? "Queued Background Task {Guid} is complete."
                                : "Queued Background Task {Guid} was cancelled.", guid);
                    });
                }
            }
        }
    }

    #endregion
}