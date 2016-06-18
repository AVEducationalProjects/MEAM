using System;
using System.Linq;
using MEAM.Model;

namespace MEAM
{
    public class Scout : Bee
    {
        public Scout(Hive hive) : base(hive){}

        protected override void Behavior()
        {
            var plan = _hive.Model;
            var days = plan.Items.First().Calendar.Keys.OrderBy(x => x).ToList();
            foreach (var day in days)
            {
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
            return new MaintenanceTask {Name = "рн", Cost = 2000, PDDecrement = 0.2};
        }
    }
}