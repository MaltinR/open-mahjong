using OpenMahjong.Equipment;
using OpenMahjong.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    /// <summary>
    /// 圈 in Chinese
    /// </summary>
    public class Round
    {
        /// <summary>
        /// name from: http://mahjong.wikidot.com/seat-and-prevalent-winds
        /// </summary>
        public int PrevalentWind { get; private set; }
        public int GameIndex { get; private set; }
        public int TotalGames { get; private set; }

        public Game CurrentGame { get; private set; }

        // TODO: Add rules
        public Round(InGamePlayer[] players, List<Tile> playingTiles, int prevalentWind = 0, int gameCount = 4)
        {
            Console.WriteLine($"============New Round (Prevalent Wind {prevalentWind})=============");
            PrevalentWind = prevalentWind; // East first default
            GameIndex = 0;
            TotalGames = gameCount;

            CurrentGame = new Game(players, playingTiles, PrevalentWind);
        }

        public void Start()
        {
            CurrentGame.Start();
        }

        /// <returns>If this round will continue</returns>
        public bool Next()
        {
            if (++GameIndex < TotalGames)
            {
                CurrentGame = new Game(CurrentGame.Players, CurrentGame.PlayingTiles, PrevalentWind, CurrentGame.SeatWind.CircularNext(4));

                return true;
            }
            else return false;
        }
    }
}
