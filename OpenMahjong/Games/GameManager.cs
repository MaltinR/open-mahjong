using OpenMahjong.Data;
using OpenMahjong.Equipment;
using OpenMahjong.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    public class GameManager
    {
        public event Action<GameManager> GamesFinished;

        /// <summary>
        /// E, S, W, N accordingly
        /// </summary>
        public InGamePlayer[] Players { get; private set; }
        public DiceSet DiceSet { get; private set; }
        SetBase tileSet;
        public Round CurrentRound { get; private set; }
        public Game CurrentGame 
        {
            get => CurrentRound.CurrentGame;
        }

        public int CurrentRoundIndex { get; private set; }
        public int TotalRounds { get; private set; } 
        public int TotalGames { get; private set; }
        public bool IsStarted { get; private set; } = false;

        const int defaultTotalRound = 4;

        public GameManager(Player[] players, int diceCount, SetBase tileSet, bool randomPosition = false, int? totalRounds = null, int? totalGames = null)
        {
            Players = new InGamePlayer[players.Length];

            DiceSet = new DiceSet(diceCount);

            // TODO: Use totalRounds & totalGames
            TotalRounds = totalRounds ?? defaultTotalRound;

            this.tileSet = tileSet;
            if (randomPosition)
            {
                List<Player> playerList = players.ShuffleNew();
                for (int i = 0; i < playerList.Count; i++)
                {
                    Players[i] = new InGamePlayer(this, playerList[i], i);
                }
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    Players[i] = new InGamePlayer(this, players[i], i);
                }
            }
            CurrentRound = new Round(Players, tileSet.GetTiles().ToList());
        }

        public void Start()
        {
            if (IsStarted) throw new Exception("Game has started already");
            IsStarted = true;

            CurrentRound.Start();
        }

        /// <summary>
        /// Next Game
        /// </summary>
        public bool Next()
        {
            if (!CurrentRound.Next())
            {
                if (CurrentRoundIndex == TotalRounds - 1)
                {
                    // End
                    GamesFinished?.Invoke(this);
                    return false;
                }
                else
                {
                    CurrentRound = new Round(Players, CurrentGame.PlayingTiles, CurrentRound.PrevalentWind.CircularNext(4));
                    CurrentRoundIndex++;
                    IsStarted = false;
                    return true;
                }
            }
            IsStarted = false;
            return true;
        }

        // TODO:
        // 

        /// <summary>
        /// TESTING <br/>
        /// Discard the target tile from hand
        /// </summary>
        public void Discard(Tile tile, InGamePlayer fromPlayer)
        {
            // It will communicate with game instance
            CurrentGame.Discard(tile, fromPlayer);
        }

        public Tile Draw()
        {
            // It will communicate with game instance
            return CurrentGame.Draw();
        }
    }
}
