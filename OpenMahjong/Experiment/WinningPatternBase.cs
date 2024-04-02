using OpenMahjong.Equipment;
using OpenMahjong.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMahjong.Games;
using System.Collections.ObjectModel;

namespace OpenMahjong.Experiment
{
    public abstract class WinningPatternBase
    {
        // TODO: out List of winning combinations
        // public abstract bool CanWin(List<Tile> tiles, List<Suit> suits);
        public abstract bool CanWin(TilesStat tilesStat, ReadOnlyCollection<Suit> suits);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiles">Assumption: Count % 3 = 1</param>
        /// <param name="suits"></param>
        /// <returns></returns>
        public abstract List<PlainTileData> GetWinningTiles(TilesStat tilesStat, ReadOnlyCollection<Suit> suits);
    }
}
