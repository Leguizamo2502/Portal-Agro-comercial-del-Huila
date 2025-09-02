using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.QR.Options
{
    public sealed class QrOptions
    {
        public string EccLevel { get; set; } = "Q"; // L/M/Q/H
        public int PixelsPerModule { get; set; } = 22;
        public bool DrawQuietZones { get; set; } = true;
    }
}
