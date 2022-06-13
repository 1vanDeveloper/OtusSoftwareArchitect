using System;
using System.Threading;

namespace Notification.Host.Models.SignalR
{
    public class TimerManager
    {
        private readonly Timer _timer;
        private int _updateInterval;
        private readonly Action _action;
        private readonly CancellationTokenSource _cts = new ();

        public TimerManager(Action action, int milliseconds)
        {
            _action = action;
            _updateInterval = milliseconds;
            _timer = new Timer(Execute, _cts.Token, 0, _updateInterval);
        }

        /// <summary>
        /// We are using an Action delegate to execute the passed callback function every X seconds.
        /// The timer will make a X seconds pause before the first execution.
        /// Finally, we just create a sixty seconds time slot for execution, to avoid limitless timer loop.
        /// </summary>
        /// <param name="stateInfo"></param>
        private void Execute(object stateInfo)
        {
            try
            {
                _action();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void UpdateInterval(int interval)
        {
            _updateInterval = interval;
            _timer.Change(0, interval);
        }

        public void Stop()
        {
            _cts.Cancel();
            _timer.Dispose();
        }
    }
}