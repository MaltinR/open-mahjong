using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Equipment
{
    public abstract class SetBase
    {
        protected List<Tile> Tiles;
        protected HashSet<Suit> Suits;
        public ReadOnlyCollection<Tile> GetTiles() => Tiles.AsReadOnly();
        public ReadOnlyCollection<Suit> GetSuits(byte value)
        {
            List<Suit> matchedSuits = new List<Suit>();
            List<Suit> suitList = Suits.ToList();

            for (int i = 0; i < suitList.Count; i++)
            {
                if (value != (value & suitList[i].Value)) { }
            }

            return matchedSuits.AsReadOnly();
        }
    }
}
