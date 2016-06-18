using System;
using System.IO;
using System.Linq;
using MEAM.Model;
using MEAMDataGen;

static internal class MEAMObjectsGen
{
    private static Random _rand = new Random();

    public static void GenerateObjects()
    {
        using (var objnamesFile = new StreamReader(ConsoleUtils.Read("Object names file: ", "data\\objects.txt")))
        {
            var names = objnamesFile.ReadToEnd().Split('\n').Select(x => x.Trim()).ToList();
            var objects = names.Select(x => new MaintenanceObject
            {
                Name = x,
                PD = _rand.NextDouble() * 0.2,
                PDIncrement = 0.5 / (365 * (_rand.Next(5) + 1)),
                MaxPD = 0.05 + _rand.NextDouble() * 0.1
            });

            ModelSerializer.SaveObjects(objects, ConsoleUtils.Read("Objects file: ", "data\\objects.json"));
        }
    }
}