using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEAM
{
    public static class RandomExt
    {
        public static T NextLinearRandomItem<T>(this Random rand, IList<T> list)  where T: class
        {
            if (!list.Any())
                return null;

            double maxChance = 0;
            for (int i = 0; i < list.Count; i++)
                maxChance += (1 - i/list.Count);

            var rnd = rand.NextDouble() * maxChance;
            for (int i = 0; i < list.Count; i++)
            {
                rnd -= (1 - i / list.Count);
                if (rnd <= 0)
                    return list[i];
            }
            return null;
        }
    }
}
