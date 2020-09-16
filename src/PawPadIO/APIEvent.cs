using System;

namespace PawPadIO
{
    public struct APIEvent
    {
        public readonly string Topic;
        public readonly object Sender;
        public readonly EventArgs EventArgs;

        public APIEvent(object sender, string topic, EventArgs eventArgs)
        {
            Sender = sender;
            Topic = topic;
            EventArgs = eventArgs;
        }
    }
}