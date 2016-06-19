using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MEAM.Model
{
    public class ModelSerializer
    {
        public static List<MaintenanceTask> LoadTasks(string file)
        {
            using (var tasksfile = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<List<MaintenanceTask>>(tasksfile.ReadToEnd());
            }
        }

        public static List<MaintenanceObject> LoadObjects(string file)
        {
            using (var objfile = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<List<MaintenanceObject>>(objfile.ReadToEnd());
            }
        }

        public static void SaveObjects(IEnumerable<MaintenanceObject> data, string file)
        {
            using (var objfile = new StreamWriter(file))
            {
                objfile.WriteLine(JsonConvert.SerializeObject(data));
            }
        }

        public static MaintenancePlan LoadMaintenancePlan(string file)
        {
            using (var planfile = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<MaintenancePlan>(planfile.ReadToEnd());
            }
        }

        public static void SaveMaintenancePlan(MaintenancePlan plan, string file)
        {
            using (var planfile = new StreamWriter(file))
            {
                planfile.WriteLine(JsonConvert.SerializeObject(plan));
            }
        }

        
    }
}
