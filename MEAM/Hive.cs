using System;
using System.Collections.Generic;
using System.Linq;
using MEAM.Model;

namespace MEAM
{
    public class Hive
    {
        private readonly List<Tuple<MaintenancePlan, double>> _data = new List<Tuple<MaintenancePlan, double>>();

        List<Bee> bees = new List<Bee>();

        private Random rand = new Random();
        private readonly MaintenancePlan _model;

        public MaintenancePlan Model
        {
            get { return _model.Clone(); }
        }

        public List<MaintenanceTask> AvailableTasks { get; private set; }

        public int Count { get; private set; }

        public Hive(MaintenancePlan model, List<MaintenanceTask> tasks, int scout, int reduce, int exchange)
        {
            for (int i = 0; i < scout; i++)
                bees.Add(new Scout(this));
            for (int i = 0; i < reduce; i++)
                bees.Add(new WorkerReduce(this));
            for (int i = 0; i < exchange; i++)
                bees.Add(new WorkerExchange(this));

            AvailableTasks = new List<MaintenanceTask>(tasks);

            _model = model.Clone();
            _model.Items.ForEach(x => { x.Calendar.Keys.ToList().ForEach(day=>x.Calendar[day]?.Clear()); x.CalculateDays(); });
        }

        public void Put(MaintenancePlan plan)
        {
            if (plan.CIRestriction == 0 && plan.HRRestriction == 0 && plan.RiskRestriction == 0 &&
                plan.TimeRestriction == 0)
            {
                lock (_data)
                {
                    _data.Add(new Tuple<MaintenancePlan, double>(plan.Clone(), plan.Revenue));
                    if (_data.Count > 20)
                    {
                        _data.Remove(_data.OrderBy(x => x.Item2).First());
                    }

                    Count++;
                }
            }
        }

        public MaintenancePlan Get()
        {
            lock (_data)
            {
                return rand.NextLinearRandomItem(_data.OrderBy(x => x.Item2).ToList())?.Item1?.Clone();
            }

        }

        public MaintenancePlan GetBest()
        {
            lock (_data)
            {
                return _data.OrderBy(x => x.Item2).LastOrDefault()?.Item1;
            }
        }

        public void Run()
        {
            Count = 0;
            bees.ForEach(bee => bee.Run());
        }

        public void Stop()
        {
            bees.ForEach(bee => bee.Stop());
        }


    }
}