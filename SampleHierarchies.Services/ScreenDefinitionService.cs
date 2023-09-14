using Newtonsoft.Json;
using SampleHierarchies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleHierarchies.Services
{
   public class ScreenDefinitionService
    {
        public static ScreenDefinition Load (string jsonFileName)
        {
            string jsonString = File.ReadAllText (jsonFileName);
            ScreenDefinition? screenDefinition = JsonConvert.DeserializeObject<ScreenDefinition>(jsonString);

            return screenDefinition;
        }

        public void Display(string jsonFileName, int linia)
        {
            ScreenDefinition screenDefinition = Load(jsonFileName);
            Console.ForegroundColor = screenDefinition.LineEntries[linia].ForegroundColor;
            Console.BackgroundColor = screenDefinition.LineEntries[linia].BackgroundColor;
            Console.WriteLine(screenDefinition.LineEntries[linia].Text);
        }

    }

}
