using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MEAM.Model
{
    public class MaintenancePlan 
    {
        public List<ObjectMaintenancePlan> Items { get; set; }

        [JsonIgnore]
        public double Revenue {
            get
            {
                return Items.Sum(
                        x=>x.Days.Sum(
                            day => x.Object.Revenue*(1-day.PD) - x.Object.CostOfDeny* day.PD - day.Tasks.Sum(task => task.Cost)));
            } }

        [JsonIgnore]
        public int HRRestriction { get { return 0; } }

        [JsonIgnore]
        public int CIRestriction { get { return 0;  } }

        [JsonIgnore]
        public int TimeRestriction { get { return 0; } }

        [JsonIgnore]
        public int RiskRestriction => Items.Count(x=>x.Days.Any(day => day.RiskViolance >= 1));

        [JsonIgnore]
        public ObjectMaintenancePlan this[MaintenanceObject index]
        {
            get { return Items.SingleOrDefault(x => x.Object.Name == index.Name); }
            set
            {
                var oldValue = this[index];
                if (oldValue != null)
                    Items.Remove(oldValue);
                Items.Add(value);
            }
        }

        public MaintenancePlan()
        {
            Items = new List<ObjectMaintenancePlan>();
        }

        public MaintenancePlan Clone()
        {
            var result = new MaintenancePlan();
            result.Items.AddRange(Items.Select(x=>x.Clone()));

            return result;
        }
    }
}
