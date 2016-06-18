using System;
using MEAM.Model;
using MEAMDataGen;

static class MEAMPlanGen
{
    public static void GenerateEmptyYearPlan()
    {
        var start = new DateTime(DateTime.Now.Year, ConsoleUtils.Read("Start month number: ", 1), 1);
        var end = (new DateTime(start.Year + 1, start.Month, start.Day)).AddDays(-1);

        var objects = ModelSerializer.LoadObjects(ConsoleUtils.Read("Objects file name: ", "data\\objects.json"));
        var plan = ModelGenerator.CreateMaintenancePlan(objects, start, end);
        ModelSerializer.SaveMaintenancePlan(plan, ConsoleUtils.Read("Plan file name: ", "data\\plan.json"));
    }
}