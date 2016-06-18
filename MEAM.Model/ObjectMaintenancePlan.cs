using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MEAM.Model
{
    public class ObjectMaintenancePlan
    {
        public MaintenanceObject Object { get; set; }
        public Dictionary<DateTime, List<MaintenanceTask>> Calendar { get; }

        public List<MaintenanceDay> _days;

        [JsonIgnore]
        public List<MaintenanceDay> Days
        {
            get {
                if (_days == null)
                {
                    CalculateDays();
                }
                return _days;
            }
        }

        public void CalculateDays()
        {
            if (_days == null)
                _days = new List<MaintenanceDay>();
            _days.Clear();

            var pd = Object.PD;
            foreach (var day in Calendar.Keys.OrderBy(x => x))
            {
                _days.Add(new MaintenanceDay
                {
                    Day = day,
                    Tasks = Calendar[day],
                    PD = pd,
                    RiskViolance = Math.Min(1, 1 - (Object.MaxPD - pd)/Object.MaxPD)
                });
                pd += Object.PDIncrement;
                Calendar[day].ForEach(task => pd = pd*(1 - task.PDDecrement));
            }
        }

        public ObjectMaintenancePlan(MaintenanceObject obj)
        {
            Object = obj;
            Calendar = new Dictionary<DateTime, List<MaintenanceTask>>();
        }

        public ObjectMaintenancePlan Clone()
        {
            var result = new ObjectMaintenancePlan(Object);
            foreach (var day in Calendar.Keys)
            {
                result.Calendar[day] = new List<MaintenanceTask>(Calendar[day]);
            }

            return result;
        }
    }
}