using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MEAM.Model;

namespace MEAMDataGen
{
    public static class ModelGenerator
    {
        private static readonly Random Rand = new Random();


        public static List<Tuple<string, double, double, double>> ObjectTypes = new List<Tuple<string, double, double, double>>();

        public static List<Tuple<string,double>> TaskTypes = new List<Tuple<string, double>>();

        static ModelGenerator()
        {
            using (var objfile = new StreamReader("data\\objects.txt"))
            {
                ObjectTypes.Clear();
                ObjectTypes.AddRange(objfile.ReadToEnd().Split('\n')
                    .Select(x =>
                    {
                        var objarr = x.Trim().Split(';').Select(y => y.Trim()).ToArray();
                        return new Tuple<string, double, double, double>(
                            objarr[0], double.Parse(objarr[1]), double.Parse(objarr[2]), double.Parse(objarr[3]));
                    }
                    ));
            }
            using (var tasksfile = new StreamReader("data\\tasks.txt"))
            {
                TaskTypes.Clear();
                TaskTypes.AddRange(tasksfile.ReadToEnd().Split('\n')
                    .Select(x =>
                    {
                        var taskarr = x.Trim().Split(';').Select(y => y.Trim()).ToArray();
                        return new Tuple<string, double>(taskarr[0], double.Parse(taskarr[1]));
                    }));
            }
        }


        public static MaintenancePlan GenerateMaintenancePlan(DateTime start, DateTime end)
        {
            var result = new MaintenancePlan();

            ObjectTypes.ForEach(x =>
            {
                var obj = new MaintenanceObject { Name = x.Item1, PD = x.Item2, PDIncrement = x.Item3, MaxPD = x.Item4};
                result.ObjectPlans[obj] = GenerateObjectMaintenancePlan(obj, start, end);
            });

            return result;
        }

        public static ObjectMaintenancePlan GenerateObjectMaintenancePlan(MaintenanceObject obj, DateTime start, DateTime end)
        {
            var result = new ObjectMaintenancePlan(obj);

            DateTime day = start.Date;
            while (day <= end)
            {
                result.Calendar[day] = new List<MaintenanceTask>();
                for (int i = 0; i < 10; i++)
                {
                    if (Rand.NextDouble() < 0.02)
                    {
                        result.Calendar[day].Add(GenerateMaintenanceTask());
                    }
                }
                day = day.AddDays(1);
            }

            return result;
        }

        private static MaintenanceTask GenerateMaintenanceTask()
        {
            var r = Rand.Next(TaskTypes.Count);
            return new MaintenanceTask { Name = TaskTypes[r].Item1, PDDecrement = TaskTypes[r].Item2};
        }

        public static MaintenancePlan CreateMaintenancePlan(List<MaintenanceObject> objects)
        {
            throw new NotImplementedException();
        }
    }
}
