using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KappaQueueCore.Events
{
    public class Event
    {        
        public string Name { get; }

        public Event(string name)
        {
            Name = name;
        }
    }
}
