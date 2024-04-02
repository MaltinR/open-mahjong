using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    public class GameHand
    {
        // TODO: Do sorting for hand

        public bool CanWin { get; private set; }
        public Tile LastDrawn { get; private set; }
        // LastDrawn is included in tiles
        List<Tile> tiles;

        public GameHand()
        {
            tiles = new List<Tile>();
        }

        public void NewGame()
        {
            // TODO: Clear other data

            tiles.Clear();
            LastDrawn = null;
            CanWin = false;
        }

        public void OnStartingHandReceived(Tile[] tiles)
        {
            this.tiles.AddRange(tiles);
        }

        public ReadOnlyCollection<Tile> GetTiles() => tiles.AsReadOnly();

        public void OnDrawn(Tile newTile)
        {
            // TODO: Sorting & calculate possibility
            LastDrawn = newTile;
            tiles.Add(newTile);
        }

        public void Discard(Tile tile)
        {
            tiles.Remove(tile);
        }
    }
}
