using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMahjong.Rules.PointRule
{
    // 牌例
    public abstract class PointRuleBase
    {
        public int Priority { get; private set; } // 計算優次
        public int Point { get; private set; } // 番
        public PointRuleBase[] ExcludedRules { get; private set; } // 當此牌例成立時，會略過計算的牌例

        public void SetExcludedRules(PointRuleBase[] pointRules)
        {
            ExcludedRules = pointRules;

            for (int i = 0; i < ExcludedRules.Length; i++)
            {
                if (ExcludedRules[i].Priority >= Priority) throw new Exception("Excluded rules cannot contain rules that have higher or equal priority to current priority");
            }
        }
    }
}
