using OpenMahjong.Equipment;
using OpenMahjong.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    public class Game
    {
        public int PrevalentWind { get; private set; }
        public int SeatWind { get; private set; }
        public bool CanDraw => wallTiles.Count > 0;

        public List<Tile> PlayingTiles { get; private set; }

        public InGamePlayer[] Players { get; private set; }

        public InGamePlayer DealerPlayer { get; private set; }
        public int CurrentTurn { get; private set; } = -1;
        public InGamePlayer TurnPlayer { get; private set; }

        int currentTurnSeat;

        List<Tile> wallTiles;

        /*
         * Turn flow (Normal):
         * Draw
         * while (Flower / Kong) -> Draw
         * Discard
         * Other player claiming
         * 
         * Turn flow (Claim):
         * Claim from other players
         * Discard
         * Other player claiming
         */

        // TODO: Game rules

        public Game(InGamePlayer[] players, List<Tile> playingTiles, int prevalentWind, int seatWind = 0)
        {
            Console.WriteLine($"============New Game (Prev. Wind: {prevalentWind} | Seat Wind: {seatWind})=============");
            Players = players;
            PrevalentWind = prevalentWind;
            SeatWind = seatWind;
            PlayingTiles = playingTiles;
            DealerPlayer = players[seatWind];
            wallTiles = playingTiles.ShuffleNew();
            // TODO: Handle CircularNext -?
            currentTurnSeat = seatWind.CircularNext(4, amount: -1);

            TurnPlayer = players[seatWind];

            for (int i = 0; i < players.Length; i++)
            {
                // TODO: Draw amount should according to game rules
                players[seatWind.CircularNext(4, amount: i)].OnStartingHandReceived(DrawMany(13));
            }

            // TurnPlayer.OnDrawn(Draw());
        }

        public void Start()
        {
            NextNormalTurn();
        }

        public Tile[] DrawMany(int count)
        {
            Tile[] tiles = new Tile[count];
            for (int i = 0; i < count; i++)
            {
                tiles[i] = wallTiles[0];
                wallTiles.RemoveAt(0);
            }

            return tiles;
        }

        public Tile Draw()
        {
            if (wallTiles.Count == 0) return null;
            Tile outTile = wallTiles[0];
            wallTiles.RemoveAt(0);

            return outTile;
        }

        public void Discard(Tile tile, InGamePlayer fromPlayer)
        {
            // Console.WriteLine($"{tile} discarded by {fromPlayer.Data.Name}");
            /*
             * TODO: Flow
             * Check claiming
             * if someone claim -> Pass turn to them
             * else -> Pass to next 
             */

            // TODO: According to the priority
            bool needWait = false;
            // Check 
            for (int i = 1; i < Players.Length; i++)
            {
                if (Players[CurrentTurn.CircularNext(4, amount: i)].CheckClaim(out (bool canChow, bool canPong, bool canKong, bool canWin) claimOption))
                {
                    // TODO: Change to class
                    needWait = true;
                }
            }

            if (needWait)
            {
                // TODO: Inform player to take action
            }
            else
            {
                NextNormalTurn();
            }
        }

        void NextNormalTurn()
        {
            // Console.WriteLine($"NextNormalTurn {wallTiles.Count}");
            // End Game
            if (!CanDraw)
            {
                // TODO: End game
            }
            else
            {
                CurrentTurn++;
                currentTurnSeat = currentTurnSeat.CircularNext(4);
                TurnPlayer = Players[currentTurnSeat];

                TurnPlayer.OnDrawn(Draw());
            }
        }
    }
}
