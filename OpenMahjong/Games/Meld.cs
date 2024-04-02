using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    public class Meld
    {

        public Tile[] Tiles { get; private set; }

        /// <summary>
        /// Could be null if not exposed
        /// </summary>
        public DiscardData DiscardData { get; private set; }


        public bool IsExposed { get; private set; }

        public Meld(Tile[] tiles, bool isExposed, DiscardData discardData = null) 
        { 
            Tiles = tiles;
            IsExposed = isExposed;
            DiscardData = discardData;
        }
    }
}
