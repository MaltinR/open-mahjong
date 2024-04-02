using OpenMahjong.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Equipment
{
    public class Dice
    {
        public int Value { get; private set; }

        public int Roll()
        {
            return Value = GeneralUtil.RandInt(1, 7);
        }
    }
}
