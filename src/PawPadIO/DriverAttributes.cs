using System;
using System.Collections.Generic;
using System.Text;

namespace PawPadIO
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DriverActionAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;

        public bool IsMutable { get; set; } = false;
    }
}
