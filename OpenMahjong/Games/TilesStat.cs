using OpenMahjong.Data;
using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenMahjong.Games.TilesStat;

namespace OpenMahjong.Games
{
    // No plan to include non-plain tiles in stat
    public class TilesStat
    {
        #region Class
        public class SameKind
        {
            Dictionary<Tile, SameKindData> dataByTile;
            SameKindData[] sameKindDatas;

            public SameKind()
            {
                int count = 4;

                dataByTile = new Dictionary<Tile, SameKindData>();
                sameKindDatas = new SameKindData[count];
                for (int i = 0; i < count; i++)
                {
                    sameKindDatas[i] = new SameKindData(i + 1);
                }
            }

            SameKind(SameKind sameKind)
            {
                Dictionary<SameKindData, SameKindData> newByOld = new Dictionary<SameKindData, SameKindData>();

                sameKindDatas = new SameKindData[sameKind.sameKindDatas.Length];
                for (int i = 0; i < sameKind.sameKindDatas.Length; i++)
                {
                    SameKindData newSameKindData = sameKind.sameKindDatas[i].Clone();
                    newByOld.Add(sameKind.sameKindDatas[i], newSameKindData);
                    sameKindDatas[i] = newSameKindData;
                }

                dataByTile = sameKind.dataByTile.ToDictionary(el => el.Key, el => el.Value);
                foreach (var data in dataByTile)
                {
                    dataByTile[data.Key] = newByOld[data.Value];
                }
            }

            /// <param name="sameKindCount">0-indexed</param>
            /// <returns></returns>
            public PlainTileData[] GetTileDatasBySameKindCount(int sameKindCount)
            {
                return sameKindDatas[sameKindCount].GetTileDatas();
            }

            /// <param name="sameKindCount">0-indexed</param>
            public int GetSameKindCount(int sameKindCount)
            {
                return sameKindDatas[sameKindCount].Count;
            }

            public void SetTilesCount(Tile[] tiles, int count)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    PlainTileData tileData = tiles[i].ToPlainTileData();
                    if (dataByTile.ContainsKey(tiles[i]))
                    {
                        dataByTile[tiles[i]].Remove(tileData);
                    }
                    else
                    {
                        dataByTile.Add(tiles[i], sameKindDatas[count - 1]);
                    }
                    sameKindDatas[count - 1].Add(tileData);
                }
            }

            public SameKind Clone()
            {
                return new SameKind(this);
            }
        }

        public class SameKindData
        {
            public int SameKindCount { get; private set; }
            /// <summary>
            /// Number of unique tile data
            /// </summary>
            public int Count { get; private set; }

            HashSet<PlainTileData> tileDatas;

            public SameKindData(int sameKindCount)
            {
                SameKindCount = sameKindCount;
                tileDatas = new HashSet<PlainTileData>();
            }

            SameKindData(SameKindData sameKindData)
            {
                SameKindCount = sameKindData.SameKindCount;
                Count = sameKindData.Count;
                tileDatas = sameKindData.tileDatas.ToHashSet();
            }

            public PlainTileData[] GetTileDatas()
            {
                return tileDatas.ToArray();
            }

            public void Add(PlainTileData tileData)
            {
                tileDatas.Add(tileData);
                Count = tileDatas.Count;
            }

            public void Remove(PlainTileData tileData)
            {
                tileDatas.Remove(tileData);
                Count = tileDatas.Count;
            }

            public SameKindData Clone()
            {
                return new SameKindData(this);
            }
        }
        #endregion

        // 副露
        public bool EverExposed { get; private set; }
        /// <summary>
        /// [0]: Unique, [1]: Pair, [2]: Triplet, [3]: Quadruplet <br/> 
        /// In overlapped, a pair would also match the condition of unique
        /// </summary>
        public int[] SameKindsOverlapped { get; private set; }

        /// <summary>
        /// [0]: Unique, [1]: Pair, [2]: Triplet, [3]: Quadruplet <br/> 
        /// In unique, a pair would NOT match the condition of unique
        /// </summary>
        public int[] SameKindsUnique { get; private set; }

        SameKind sameKindOverlapped;
        SameKind sameKindUnique;
        List<Tile> tiles;

        public TilesStat(List<Tile> tiles)
        {
            // Assumption: tiles are sorted
            this.tiles = tiles;
            EverExposed = false;
            SameKindsOverlapped = new int[4];
            SameKindsUnique = new int[4];

            sameKindOverlapped = new SameKind();
            sameKindUnique = new SameKind();

            Initialize();
        }

        TilesStat(TilesStat tilesStat)
        {
            tiles = tilesStat.GetTiles();
            EverExposed = tilesStat.EverExposed;
            SameKindsOverlapped = (int[])tilesStat.SameKindsOverlapped.Clone();
            SameKindsUnique = (int[])tilesStat.SameKindsUnique.Clone();

            sameKindOverlapped = tilesStat.sameKindOverlapped.Clone();
            sameKindUnique = tilesStat.sameKindUnique.Clone();
        }

        public List<Tile> GetTiles()
        {
            return tiles.ToList();
        }

        public void Expose() => EverExposed = true;

        public TilesStat Clone()
        {
            return new TilesStat(this);
        }

        /// <param name="sameKindCount">1-based index</param>
        /// <returns></returns>
        public PlainTileData[] GetUniqueSameKindTileDatas(int sameKindCount)
        {
            return sameKindUnique.GetTileDatasBySameKindCount(sameKindCount - 1);
        }

        /// <param name="sameKindCount">1-based index</param>
        /// <returns></returns>
        public PlainTileData[] GetOverlappedSameKindTileDatas(int sameKindCount)
        {
            return sameKindOverlapped.GetTileDatasBySameKindCount(sameKindCount - 1);
        }

        void Initialize()
        {
            for (int i = 0; i < tiles.Count;)
            {
                // TODO: Handle wild card
                if (!tiles[i].IsPlainTile())
                {
                    i++;
                    continue;
                }

                int sameCount = 0;
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    if (tiles[i].Value == tiles[j].Value)
                    {
                        sameCount++;
                    }
                    else break;
                }

                for (int j = 0; j <= sameCount; j++)
                {
                    Tile[] tileArray = new Tile[j + 1];
                    for (int k = 0; k < tileArray.Length; k++)
                    {
                        tileArray[k] = tiles[i + k];
                    }
                    sameKindOverlapped.SetTilesCount(tileArray, j + 1);
                    // sameKindOverlapped.SameKindDatas[j].tileDatas.Add(tileData);
                    if (j == sameCount)
                    {
                        sameKindUnique.SetTilesCount(tileArray, sameCount + 1);
                        // sameKindUnique.SameKindDatas[sameCount].tileDatas.Add(tileData);
                    }
                }

                i += sameCount + 1;
            }

            for (int i = 0; i < 4; i++)
            {
                SameKindsOverlapped[i] = sameKindOverlapped.GetSameKindCount(i);
                SameKindsUnique[i] = sameKindUnique.GetSameKindCount(i);
            }
        }
    }
}
