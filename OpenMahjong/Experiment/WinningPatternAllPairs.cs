using OpenMahjong.Data;
using OpenMahjong.Equipment;
using OpenMahjong.Games;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Experiment
{
    /// <summary>
    /// 七對子、八對子
    /// </summary>
    public class WinningPatternAllPairs : WinningPatternBase
    {
        public override bool CanWin(TilesStat tilesStat, ReadOnlyCollection<Suit> suits)
        {
            if (tilesStat.SameKindsUnique[0] != 0 || tilesStat.SameKindsUnique[2] != 0) return false;
            return true;
        }

        public override List<PlainTileData> GetWinningTiles(TilesStat tilesStat, ReadOnlyCollection<Suit> suits)
        {
            List<PlainTileData> winningTiles = new List<PlainTileData>();
            List<Tile> tiles = tilesStat.GetTiles();
            if (tiles.Count % 2 == 0) return winningTiles;

            // Check 1 and 3
            if (tilesStat.SameKindsUnique[0] + tilesStat.SameKindsUnique[2] != 1) return winningTiles;

            HashSet<PlainTileData> checkedTileData = new HashSet<PlainTileData>();

            winningTiles.AddRange(tilesStat.GetUniqueSameKindTileDatas(1));
            winningTiles.AddRange(tilesStat.GetUniqueSameKindTileDatas(3));
            /*
            List<PlainTileData> possibleWinningTiles = new List<PlainTileData>();
            for (int i = 0; i < tiles.Count; i++)
            {
                PlainTileData tileData = tiles[i].ToPlainTileData();

                if (checkedTileData.Contains(tileData))

                checkedTileData.Add(tileData);
            }
            */

            return winningTiles;
        }
    }
}
