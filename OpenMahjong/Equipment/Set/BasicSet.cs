using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMahjong.Equipment;

namespace OpenMahjong.Equipment.Set
{
    public class BasicSet : SetBase
    {
        public BasicSet()
        {
            Tiles = new List<Tile>();

            // Bamboo
            // Dot
            // Character
            // Honor
            // 00000001
            Suit bambooSuit = new Suit("bam", "Bamboo", 1 << 0, true, true, true);
            // 00000010
            Suit dotSuit = new Suit("dot", "Dot", 1 << 1, true, true, true);
            // 00000100
            Suit charSuit = new Suit("cha", "Character", 1 << 2, true, true, true);
            // 00001000
            Suit honorSuit = new Suit("hon", "Honor", 1 << 3, false, true, true);

            // Bamboo
            Tiles.AddRange(GetBasicSuitTiles(bambooSuit, 9, new string[]
            {
                "一索", // DEV ONLY, Change to English in prod
                "二索",
                "三索",
                "四索",
                "五索",
                "六索",
                "七索",
                "八索",
                "九索",
            }));
            // Dot
            Tiles.AddRange(GetBasicSuitTiles(dotSuit, 9, new string[]
            {
                "一筒",
                "二筒",
                "三筒",
                "四筒",
                "五筒",
                "六筒",
                "七筒",
                "八筒",
                "九筒",
            }));
            // Character
            Tiles.AddRange(GetBasicSuitTiles(charSuit, 9, new string[]
            {
                "一萬",
                "二萬",
                "三萬",
                "四萬",
                "五萬",
                "六萬",
                "七萬",
                "八萬",
                "九萬",
            }));
            // Honor
            Tiles.AddRange(GetBasicSuitTiles(honorSuit, 7, new string[]
            {
                "東",
                "南",
                "西",
                "北",
                "紅中",
                "白板",
                "發財",
            }));
        }

        /// <param name="suit"></param>
        /// <param name="count"></param>
        /// <param name="names">Name for each</param>
        /// <returns></returns>
        public static IEnumerable<Tile> GetBasicSuitTiles(Suit suit, int count, string[] names)
        {
            int repeatCount = 4;
            Tile[] outTiles = new Tile[count * repeatCount];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < repeatCount; j++)
                {
                    outTiles[i * repeatCount + j] = new Tile(i, suit, names[i]);
                }
            }

            return outTiles;
        }
    }
}
