using OpenMahjong.Data;
using OpenMahjong.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Equipment
{
    public class Tile : IComparable<Tile>, IComparer<Tile>
    {
        /// <summary>
        /// 0 - 8 (1 - 9 for human)
        /// </summary>
        public int Number;
        // public Suit Suit;
        public byte SuitValue;
        public string Name;
        /// <summary>
        /// A set of mahjong could have more than one tile containing same id if they represent exactly the same
        /// </summary>
        public string Id;

        Suit plainDataSuit; // If it is plain
        public readonly int Value;

        public Tile(int number, Suit suit, string name, string id = null)
        {
            Number = number;
            // Suit = suit;
            SuitValue = suit.Value;
            plainDataSuit = suit;
            Name = name;
            Id = id ?? $"{SuitValue}_{number}";
            // Idea: 0-15 (2 bytes) for number
            if (plainDataSuit != null) Value = plainDataSuit.Value * 16 + number;
            else Value = 0;
        }

        public Tile(int number, Suit[] suits, string name, string id = null)
        {
            Number = number;
            // Suit = suit;
            SuitValue = 0;
            for (int i = 0; i < suits.Length; i++)
            {
                SuitValue |= suits[i].Value;
            }
            if (suits.Length == 1) plainDataSuit = suits[0];
            Name = name;
            Id = id ?? $"{SuitValue}_{number}";
            // Idea: 0-15 (2 bytes) for number
            if (plainDataSuit != null) Value = plainDataSuit.Value * 16 + number;
            else Value = 0;
        }

        public static Tile[] GetTiles(int number, Suit suit, string name, string id, int count)
        {
            Tile[] tiles = new Tile[count];
            for (int i = 0; i < count; i++)
            {
                tiles[i] = new Tile(number, suit, name, id);
            }

            return tiles;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }

        public bool IsPlainTile()
        {
            return plainDataSuit != null;
            // return SuitValue.CountSetBits() == 1;
        }

        public bool TryToPlainTileData(out PlainTileData tileData)
        {
            if (IsPlainTile())
            {
                tileData = new PlainTileData(Number, plainDataSuit);
                return true;
            }
            else
            {
                tileData = new PlainTileData();
                return false;
            }
        }
        
        public PlainTileData ToPlainTileData()
        {
            return new PlainTileData(Number, plainDataSuit);
        }

        public static bool operator >(Tile a, Tile b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(Tile a, Tile b)
        {
            return a.Value < b.Value;
        }

        public int Compare(Tile a, Tile b)
        {
            if (a.Value == b.Value) return 0;
            else if (a.Value < b.Value) return -1;
            else return 1;
        }

        public int CompareTo(Tile other)
        {
            return Compare(this, other);
        }
    }
}
