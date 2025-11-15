using System.Collections.Generic;

namespace Systems.WorldGeneration.Steps
{
    public class StepsData
    {
        public string Type { get; set; }
        public Dictionary<string, object> Params { get; set; }
    }
}