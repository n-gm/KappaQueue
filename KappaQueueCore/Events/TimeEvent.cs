using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KappaQueueEvents.Events
{
    public class TimeEvent : Event
    {
        public delegate void TimeEventCallback(TimeEvent timeEvent);
        private Timer _timer;
        public bool Repeatable { get; }
        public TimeEventCallback OnEventTriggered;

        public TimeEvent(string name, int eventTime, bool repeatable = false)
            :base(name)
        {
            _timer = new Timer(OnTimer, this, 0, eventTime);
            Repeatable = repeatable;
        }

        private void OnTimer(Object timerInfo)
        {
            OnEventTriggered?.Invoke(this);
            if (!Repeatable)
                _timer.Dispose();
        }
        
        public void StopTimer()
        {
            _timer.Dispose();
        }
    }
}
