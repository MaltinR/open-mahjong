using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Games
{
    /// <summary>
    /// Storing data of in-game: chips, position, etc
    /// </summary>
    public class InGamePlayer
    {
        // TODO: Event for control
        public event Action<InGamePlayer, Tile> TileDrawn;
        // TODO: Has param
        public event Action<InGamePlayer> ClaimAppeared;

        public Data.Player Data;
        public int Position;

        GameManager gameManager;
        // public List<Tile> HandTiles;
        GameHand hand;
        /// <summary>
        /// Example: Flower
        /// </summary>
        List<Tile> exposedTiles;
        List<Meld> exposedMelds;

        public InGamePlayer(GameManager gameManager, Data.Player data, int position)
        {
            this.gameManager = gameManager;
            Data = data;
            Position = position;

            exposedTiles = new List<Tile>();
            exposedMelds = new List<Meld>();
            hand = new GameHand();
        }

        public ReadOnlyCollection<Tile> GetExposedTiles()
        {
            return exposedTiles.AsReadOnly();
        }


        public ReadOnlyCollection<Meld> GetExposedMelds()
        {
            return exposedMelds.AsReadOnly();
        }

        public ReadOnlyCollection<Tile> GetHandTiles()
        {
            return hand.GetTiles();
        }

        /// <summary>
        /// System would w
        /// </summary>
        /// <param name="tile"></param>
        public void OnDrawn(Tile tile)
        {
            hand.OnDrawn(tile);

            TileDrawn?.Invoke(this, tile);
        }

        public void OnStartingHandReceived(Tile[] tiles)
        {
            hand.OnStartingHandReceived(tiles);
        }

        public void NewGame()
        {
            exposedMelds.Clear();
            exposedTiles.Clear();
            hand.NewGame();
        }

        // TODO: Change to struct
        public bool CheckClaim(out (bool canChow, bool canPong, bool canKong, bool canWin) claimOption)
        {
            // TODO: Calculate 
            claimOption = (false, false, false, false);
            return false;
        }
    }
}
