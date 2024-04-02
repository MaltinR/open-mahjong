using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Equipment
{
    public class DiceSet
    {
        public Dice[] Dices { get; private set; }

        public DiceSet(int diceCount) 
        {
            Dices = new Dice[diceCount];
            for (int i = 0; i < diceCount; i++)
            {
                Dices[i] = new Dice();
            }
        }

        public int Roll()
        {
            int totalValue = 0;
            for (int i = 0; i < Dices.Length; i++)
            {
                totalValue += Dices[i].Roll();
            }

            return totalValue;
        }
    }
}
