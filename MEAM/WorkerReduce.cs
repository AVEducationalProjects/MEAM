using System;
using System.Linq;

namespace MEAM
{
    public class WorkerReduce : Bee
    {
        private const int K = 10;

        public WorkerReduce(Hive hive) : base(hive)
        {
        }

        protected override void Behavior()
        {
            var reducedPlan = _hive.Get();
            if (reducedPlan == null)
                return;

            var daysToReduce = reducedPlan.Items
                .SelectMany(x => x.Days)
                .Where(x => x.Tasks.Any())
                .OrderBy(x => x.RiskViolance)
                .Take(K).ToList();
            daysToReduce.ForEach(day => day.Tasks.Clear());
            reducedPlan.Items.ForEach(x=>x.CalculateDays());

            _hive.Put(reducedPlan);
        }
    }
}