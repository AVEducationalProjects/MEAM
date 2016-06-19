using System;
using System.Collections.Generic;
using System.Linq;
using MEAM.Model;

namespace MEAM
{
    public class Scout : Bee
    {
        public Scout(Hive hive) : base(hive){}

        private Dictionary<DateTime, int> availableTime = new Dictionary<DateTime, int>();

        protected override void Behavior()
        {
            var plan = _hive.Model;
            var days = plan.Items.First().Calendar.Keys.OrderBy(x => x).ToList();
            foreach (var day in days)
            {
                availableTime[day] = 8;

                var objectsPD = plan.Items.Select(x => new { x.Object, PD=x.Days.Single(d=>d.Day==day).RiskViolance}).OrderByDescending(x=>x.PD).ToList();

                foreach (var objPD in objectsPD)
                {
                    if (objPD.PD > rand.NextDouble())
                    {
                        var task = GetTask(plan, day, objPD.Object);
                        if (task != null)
                        {
                            var objectMaintenance = plan.Items.Single(x => x.Object == objPD.Object);
                            objectMaintenance.Calendar[day].Add(task);
                            objectMaintenance.CalculateDays();
                        }
                    }
                }
            }

            _hive.Put(plan);

        }

        private MaintenanceTask GetTask(MaintenancePlan plan, DateTime day, MaintenanceObject maintenanceObject)
        {
            var tasks = _hive.AvailableTasks.Where(t=> t.Time <= availableTime[day]).OrderByDescending(t=>t.PDDecrement).ToList();
            var task = rand.NextLinearRandomItem(tasks);
            if(task!=null)
                availableTime[day] -= task.Time;

            return task;
        }
    }
}