namespace MEAM.Model
{
    public class MaintenanceObject
    {
        public string Name { get; set; }
        public double PD { get; set; }
        public double PDIncrement { get; set; }
        public double MaxPD { get; set; }
        public double Revenue { get; set; }
        public double CostOfDeny { get; set; }
    }
}