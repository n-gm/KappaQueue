using KappaQueueEvents.Events.List;
using KappaQueueEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KappaQueueEvents.Events 
{
    public class CoreEventHandler : IEventHandler
    {
        public List<TimeEvent> Events = new List<TimeEvent>();

        public CoreEventHandler()
        {

        }

        public void AddEvent(TimeEvent newEvent)
        {

        }        
    }
}
