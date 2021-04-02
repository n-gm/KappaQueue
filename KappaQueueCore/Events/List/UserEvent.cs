using System;
using System.Collections.Generic;
using System.Text;

namespace KappaQueueEvents.Events.List
{
    public class UserEvent : TimeEvent
    {
        public int UserId { get; }

        public UserEvent(string name, int timer, int userId, bool repeatable = false)
            :base(name, timer, repeatable)
        {
            UserId = userId;
        }

    }
}
