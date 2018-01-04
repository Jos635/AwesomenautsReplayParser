using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class CharacterEntity
    {
        public string Username { get; }

        public string CharacterName { get; }

        public Vot<Position> Position { get; set; }

        public Vot<double> Aim { get; set; }

        public TimeRange VisibleTime { get; set; }

        public uint TeamId { get; internal set; }

        public CharacterEntity(string userName, string characterName)
        {
            Username = userName;
            CharacterName = characterName;
        }
    }
}
