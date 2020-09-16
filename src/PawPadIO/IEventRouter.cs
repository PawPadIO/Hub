using System;
using System.Collections.Generic;

namespace PawPadIO
{
    public interface IEventRouter
    {
        ICollection<string> ListTopics();
        IObservable<APIEvent> Register(string topic);
        void SendEvent(string topic, EventArgs eventArgs, object sender);
    }
}