using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MEAM.Model
{
    public class MaintenanceDay
    {
        public string Day { get; set; }

        public double RiskViolance { get; set; }

        public SolidColorBrush RiskViolanceColor => new SolidColorBrush(Color.FromRgb(255, (byte)Math.Round(255-150*RiskViolance), (byte)Math.Round(255 -150*RiskViolance)));

        public List<MaintenanceTask> Tasks { get; set; }
    }
}