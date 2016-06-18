using System;
using System.Collections.Generic;
using System.Linq;

namespace MEAM.Model
{
    public class MaintenancePlan
    {
        public Dictionary<MaintenanceObject, ObjectMaintenancePlan> ObjectPlans { get; private set; }

        public List<MaintenancePlanItem> Items => (from obj in ObjectPlans.Keys
            select new MaintenancePlanItem { Object = obj, Plan = ObjectPlans[obj]}).ToList();


        public MaintenancePlan()
        {
            ObjectPlans = new Dictionary<MaintenanceObject, ObjectMaintenancePlan>();
        }
    }
}
