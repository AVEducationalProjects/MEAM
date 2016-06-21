using System;
using System.Linq;

namespace MEAM
{
    public class WorkerExchange : Bee
    {
        private const int K = 10;

        public WorkerExchange(Hive hive) : base(hive)
        {
        }

        protected override void Behavior()
        {
            var exchangedPlan = _hive.Get();
            if (exchangedPlan == null)
                return;

            var daysToExchange = exchangedPlan.Items
                .SelectMany(x => x.Days)
                .Where(x => x.Tasks.Any())
                .OrderByDescending(x => x.Tasks.Sum(t => t.Cost) / x.RiskViolance)
                .Take(K).ToList();

            daysToExchange.ForEach(day =>
            {
                var task = day.Tasks.OrderByDescending(t => t.Cost).First();
                var taskToChange =
                    _hive.AvailableTasks.OrderBy(t => t.PDDecrement).FirstOrDefault(t => t.Cost < task.Cost);
                if (taskToChange != null)
                {
                    day.Tasks.Remove(task);
                    day.Tasks.Add(taskToChange);
                }
            });

            exchangedPlan.Items.ForEach(x => x.CalculateDays());
            _hive.Put(exchangedPlan);
        }
    }
}