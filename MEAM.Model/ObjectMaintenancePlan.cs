using System;
using System.Collections.Generic;
using System.Linq;

namespace MEAM.Model
{
    public class ObjectMaintenancePlan
    {
        public MaintenanceObject Object { get; set; }
        public Dictionary<DateTime, List<MaintenanceTask>> Calendar { get; }

        public List<MaintenanceDay> Days
        {
            get
            {
                var result = new List<MaintenanceDay>();

                var pd = Object.PD;
                foreach (var day in Calendar.Keys.OrderBy(x => x))
                {
                    result.Add(new MaintenanceDay
                    {
                        Day = day.ToString("dd.MM.yy"),
                        Tasks = Calendar[day],
                        RiskViolance = 1 - (Object.MaxPD - pd) / Object.MaxPD
                    });
                    pd += Object.PDIncrement;
                    Calendar[day].ForEach(task => pd = pd*(1-task.PDDecrement));
                }

                return result;
            }
        }

        public ObjectMaintenancePlan(MaintenanceObject obj)
        {
            Object = obj;
            Calendar = new Dictionary<DateTime, List<MaintenanceTask>>();
        }

    }
}