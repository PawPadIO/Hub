using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PawPadIO
{
    public class LightBulb : Device
    {
        public string Name { get; set; }
        public bool Reachable { get; set; }
        public bool On { get; set; }
        [Range(0, 255, ErrorMessage = "Brightness must be between 0 and 255.")]
        public int Brightness { get; set; }
        [Range(153, 500, ErrorMessage = "Brightness must be between 153 and 500 mirek.")]
        public int? Warmth { get; set; }
        [Range(0, 65535, ErrorMessage = "Hue must be between 0 and 65535.")]
        public int? Hue { get; set; }
        [Range(0 , 255, ErrorMessage = "Saturation must be between 0 and 255.")]
        public int? Saturation { get; set; }
    }
}
