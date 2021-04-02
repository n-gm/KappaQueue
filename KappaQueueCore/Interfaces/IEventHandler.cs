using KappaQueueEvents.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KappaQueueEvents.Interfaces
{
    public delegate void EventTrigger(Event triggeredEvent);
    public interface IEventHandler
    {        
        public void AddEvent(TimeEvent newEvent);
    }
}
