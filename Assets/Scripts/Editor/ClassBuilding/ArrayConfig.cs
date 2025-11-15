using System.Collections.Generic;

namespace Editor.ClassBuilding
{
    public class ArrayConfig
    {
        public string ArrayName;
        public IEnumerable<FieldConfig> FieldConfigs;
        public KnownClassType ClassType = KnownClassType.String;
    }
}