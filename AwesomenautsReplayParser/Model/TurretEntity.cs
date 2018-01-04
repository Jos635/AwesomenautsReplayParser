using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class TurretEntity
    {
        public Position Position { get; internal set; }
        public Vot<double> Aim { get; internal set; }
        public uint TeamId { get; internal set; }
    }
}
