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
    public class WinningPatternNormal : WinningPatternBase
    {
        // TODO: Pass stat
        /*
         * Duplicate count: how many of the same tile
         * Sequence count: how seq (from where to where) and how many
         */

        // TODO: Deal with wild card
        public override bool CanWin(TilesStat tilesStat, ReadOnlyCollection<Suit> suits)
        {
            return CanWin(tilesStat.GetTiles(), suits);
        }

        public override List<PlainTileData> GetWinningTiles(TilesStat tilesStat, ReadOnlyCollection<Suit> suits)
        {
            List<PlainTileData> winningTiles = new List<PlainTileData>();
            List<Tile> tiles = tilesStat.GetTiles();
            if (tiles.Count % 2 != 1) return winningTiles;
            HashSet<PlainTileData> possibleWinningTiles = new HashSet<PlainTileData>();
            HashSet<PlainTileData> checkedTiles = new HashSet<PlainTileData>();

            for (int i = 0; i < tiles.Count; i++) 
            {
                Tile tile = tiles[i];
                if (!tile.TryToPlainTileData(out PlainTileData tileData)) continue;

                if (checkedTiles.Contains(tileData)) continue;

                if (tileData.Suit.CanChow)
                {
                    if (tileData.Number > 0) possibleWinningTiles.Add(new PlainTileData(tileData.Number - 1, tileData.Suit));
                    if (tileData.Number < tileData.Suit.MaxNumber - 1) possibleWinningTiles.Add(new PlainTileData(tileData.Number + 1, tileData.Suit));
                }
                if (tileData.Suit.CanPong)
                {
                    possibleWinningTiles.Add(tileData);
                }

                checkedTiles.Add(tileData);
            }

            if (possibleWinningTiles.Count > 0)
            {
                List<Tile> testTileList = tiles.ToList();
                testTileList.Sort();

                foreach (PlainTileData possibleWinningTile in possibleWinningTiles)
                {
                    Tile tile = possibleWinningTile.ToTile();
                    testTileList.Add(tile);
                    // TODO: Use method to sort
                    testTileList.Sort();

                    if (CanWin(testTileList, suits))
                    {
                        winningTiles.Add(possibleWinningTile);
                    }
                    testTileList.Remove(tile);
                }
            }

            return winningTiles;
        }

        bool CanWin(List<Tile> tiles, ReadOnlyCollection<Suit> suits)
        {
            Dictionary<byte, List<Tile>> tilesBySuit = tiles.GroupBy(tile => tile.SuitValue).ToList().ToDictionary(el => el.Key, el => el.ToList());
            Dictionary<byte, Suit> suitByByte = suits.ToDictionary(el => el.Value, el => el);

            // Only one suit cannot be divided by 3

            List<Tile> pairedSuitTiles = null;
            List<List<Tile>> unpairedSuitTileLists = new List<List<Tile>>();
            foreach (var keyValue in tilesBySuit)
            {
                int modValue = keyValue.Value.Count % 3;
                if (modValue == 2)
                {
                    if (pairedSuitTiles != null) return false;
                    pairedSuitTiles = keyValue.Value;
                }
                else if (modValue != 0)
                {
                    return false;
                }
                else
                {
                    unpairedSuitTileLists.Add(keyValue.Value);
                }
            }

            if (pairedSuitTiles == null) return false;

            // TODO: Check those suit tiles moded by 3

            int[] possiblePairs = GetPossiblePairs(pairedSuitTiles);

            // Console.WriteLine($"Possible pairs: {string.Join(",", possiblePairs)}");

            for (int i = 0; i < unpairedSuitTileLists.Count; i++)
            {
                if (!IsUnpairSuitTilesLegal(unpairedSuitTileLists[i], suitByByte[unpairedSuitTileLists[i][0].SuitValue]))
                {
                    return false;
                }
            }

            for (int i = 0; i < possiblePairs.Length; i++)
            {
                int targetNumber = possiblePairs[i];

                for (int j = 0; j < pairedSuitTiles.Count - 1; j++)
                {
                    if (pairedSuitTiles[j].Number == targetNumber && pairedSuitTiles[j + 1].Number == targetNumber)
                    {
                        // TODO: Reduce new

                        List<Tile> pairedSuitTilesWithoutPaired = pairedSuitTiles.ToList();
                        for (int k = 0; k < 2; k++)
                        {
                            pairedSuitTilesWithoutPaired.RemoveAt(j);
                        }

                        if (pairedSuitTilesWithoutPaired.Count == 0) return true;

                        if (IsUnpairSuitTilesLegal(pairedSuitTilesWithoutPaired, suitByByte[pairedSuitTilesWithoutPaired[0].SuitValue]))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// https://www.thenewslens.com/article/100657
        /// </summary>
        /// <returns></returns>
        int[] GetPossiblePairs(List<Tile> tiles)
        {
            List<Tile> list147 = new List<Tile>();
            List<Tile> list258 = new List<Tile>();
            List<Tile> list369 = new List<Tile>();

            for (int i = 0; i < tiles.Count; i++) 
            {
                if (tiles[i].Number % 3 == 0) list147.Add(tiles[i]);
                else if ((tiles[i].Number - 1) % 3 == 0) list258.Add(tiles[i]);
                else list369.Add(tiles[i]);
                // else throw new Exception("Number is not 1-9");
            }

            // Console.WriteLine($"{list147.Count} | {list258.Count} | {list369.Count}");

            int mod147 = list147.Count % 3;
            int mod258 = list258.Count % 3;
            int mod369 = list369.Count % 3;

            if (mod147 == mod258)
            {
                // Pair is in 369
                return new int[] { 2, 5, 8 }; // 0-indexed
            }
            else if (mod147 == mod369)
            {
                // Pair is in 258
                return new int[] { 1, 4, 7 }; // 0-indexed
            }
            else
            {
                // Pair is in 147
                return new int[] { 0, 3, 6 }; // 0-indexed
            }
        }

        bool IsUnpairSuitTilesLegal(List<Tile> tiles, Suit suit)
        {
            // ASUMMPTION: tiles are sorted
            // TODO: Pass stack info
            // TODO: Pass sequence info

            bool canChow = suit.CanChow;
            bool canPong = suit.CanPong;
            tiles = tiles.ToList();

            if (!canChow && !canPong) return false;

            while (tiles.Count > 0)
            {
                int startTileCount = tiles.Count;
                if (canPong)
                {
                    if (tiles.Count - 2 > 0)
                    {
                        int targetTileNumber = tiles[0].Number;
                        int sameTileCount = 0;

                        for (int j = 1; j < tiles.Count; j++)
                        {
                            if (targetTileNumber == tiles[j].Number) sameTileCount++;
                            else break;
                        }

                        if (sameTileCount >= 2)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tiles.RemoveAt(0);
                            }
                            continue;
                        }
                    }
                }

                if (canChow)
                {
                    if (tiles.Count - 2 > 0 && tiles[0].Number != tiles[1].Number)
                    {
                        int targetNumber = tiles[0].Number;
                        int[] indexes = new int[] { 0, -1, -1 };
                        int currentIndex = 1; // The index that you are finding
                        bool isFound = false;

                        for (int j = currentIndex; j < tiles.Count; j++)
                        {
                            int offset = tiles[j].Number - targetNumber;
                            if (offset > 1) break;

                            if (offset == 1)
                            {
                                indexes[currentIndex] = j;

                                if (currentIndex == 2)
                                {
                                    isFound = true;
                                    break;
                                }

                                currentIndex++;
                                targetNumber++;
                            }
                        }

                        if (isFound)
                        {
                            for (int j = 2; j >= 0; j--)
                            {
                                tiles.RemoveAt(indexes[j]);
                            }
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                if (startTileCount == tiles.Count) return false;
            }

            return true;
        }
    }
}
