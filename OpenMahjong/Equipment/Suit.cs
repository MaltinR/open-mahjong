using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Equipment
{
    public class Suit
    {
        public string Id;
        public string Name;
        public byte Value; // Basic gameplay only needs 4-5 bits
        public bool CanChow;
        public bool CanPong;
        public bool CanKong;
        /// <summary>
        /// Max number in this suit<br/>
        /// non-honor: 1-9 (0-8), honor: 1-7 (0-6)<br/>
        /// Exclusive
        /// </summary>
        public int MaxNumber; 

        public Suit(string id, string name, byte value, bool canChow, bool canPong, bool canKong, int maxNumber = 9)
        {
            Id = id;
            Name = name;
            Value = value;
            CanChow = canChow;
            CanPong = canPong;
            CanKong = canKong;
            MaxNumber = maxNumber;
        }

        public bool IsSuitMatched(byte value)
        {
            return (value & Value) > 0;
        }
    }
}
