using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Util
{
    public static class GeneralUtil
    {
        static Random random = new Random();

        public static int RandInt(int minInc, int maxExc)
        {
            return random.Next(minInc, maxExc);
        }

        public static float RandFloat(float minInc, float maxExc)
        {
            float range = maxExc - minInc;
            return minInc + random.NextSingle() * range;
        }

        public static List<T> ShuffleNew<T>(this IEnumerable<T> items)
        {
            // return items.ToList();
            List<T> outList = items.ToList();

            Random rng = new Random();
            int n = outList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = outList[k];
                outList[k] = outList[n];
                outList[n] = value;
            }

            return outList;
        }


        public static List<T> Shuffle<T>(this List<T> items)
        {
            Random rng = new Random();
            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = items[k];
                items[k] = items[n];
                items[n] = value;
            }

            return items;
        }

        public static int CircularNext(this int value, int maxExc, int minInc = 0, int amount = 1)
        {
            int totalRange = maxExc - minInc;
            int modValue = ((value - minInc) + amount) % totalRange;
            return (modValue < 0 ? modValue + totalRange : modValue) + minInc;
        }

        public static int CountSetBits(this byte num)
        {
            int count = 0;
            while (num > 0)
            {
                count += num & 1;
                num >>= 1;
            }
            return count;
        }
    }
}
