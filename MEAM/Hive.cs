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

        public int Count { get; private set; }

        public Hive(MaintenancePlan model)
        {
            bees.Add(new Scout(this));
            bees.Add(new Scout(this));
            bees.Add(new WorkerReduce(this));
            bees.Add(new WorkerExchange(this));

            _model = model.Clone();
            Model.Items.ForEach(x => { x.Calendar.Clear(); x.CalculateDays(); });
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
                if (!_data.Any())
                    return null;

                double maxChance = 0;
                for (int i = 1; i <= _data.Count; i++)
                    maxChance += 1 / i;

                var rnd = rand.NextDouble() * maxChance;
                for (int i = 0; i < _data.Count; i++)
                {
                    rnd -= 1 / (i + 1);
                    if (rnd <= 0)
                        return _data.OrderBy(x => x.Item2).ToArray()[i].Item1.Clone();
                }
                return null;
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