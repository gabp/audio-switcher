using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher
{
    public class AudioDevice
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public bool IsDefaultDevice { get; set; }
    }
}
