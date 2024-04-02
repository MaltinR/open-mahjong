using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    public class DiscardData
    {
        public Tile DiscardTile { get; private set; }
        public InGamePlayer DiscardPlayer { get; private set; }

        public DiscardData(Tile tile, InGamePlayer from)
        {
            DiscardTile = tile;
            DiscardPlayer = from;
        }
    }
}
