using OpenMahjong.Equipment;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Data
{
    /// <summary>
    /// Plain means there's no wild card feature
    /// </summary>
    public struct PlainTileData : IComparable<PlainTileData>, IComparer<PlainTileData>
    {
        public int Number;
        /// <summary>
        /// Should only for one suit
        /// </summary>
        public Suit Suit;
        readonly int order;

        public PlainTileData(int number, Suit suit)
        {
            Number = number;
            Suit = suit;
            order = suit.Value * 16 + number;
        }

        public PlainTileData()
        {
            Number = -1;
            Suit = null;
            order = -1;
        }

        public Tile ToTile(string name = "Dummy")
        {
            return new Tile(Number, Suit, name);
        }

        public override bool Equals(object? obj)
        {
            return obj is PlainTileData tile &&
                   Number == tile.Number &&
                   EqualityComparer<Suit>.Default.Equals(Suit, tile.Suit);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Suit);
        }

        public static bool operator == (PlainTileData data, Tile tile)
        {
            return data.Number == tile.Number && data.Suit.IsSuitMatched(tile.SuitValue);
        }
        public static bool operator == (Tile tile, PlainTileData data)
        {
            return data.Number == tile.Number && data.Suit.IsSuitMatched(tile.SuitValue);
        }

        public static bool operator !=(PlainTileData data, Tile tile)
        {
            return !(data == tile);
        }
        public static bool operator !=(Tile tile, PlainTileData data)
        {
            return !(tile == data);
        }

        public override string ToString()
        {
            return $"{Suit.Name}{Number}";
        }

        public static bool operator >(PlainTileData a, PlainTileData b)
        {
            return a.order > b.order;
        }

        public static bool operator <(PlainTileData a, PlainTileData b)
        {
            return a.order < b.order;
        }

        public int Compare(PlainTileData a, PlainTileData b)
        {
            if (a.order == b.order) return 0;
            else if (a.order < b.order) return -1;
            else return 1;
        }

        public int CompareTo(PlainTileData other)
        {
            return Compare(this, other);
        }
    }
}
