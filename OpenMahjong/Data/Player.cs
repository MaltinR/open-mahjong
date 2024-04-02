using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Data
{
    public class Player
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
