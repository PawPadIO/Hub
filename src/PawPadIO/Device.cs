using System;

namespace PawPadIO
{
    public abstract class Device
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
