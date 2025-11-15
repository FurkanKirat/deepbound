namespace Editor.ClassBuilding
{
    public class FieldConfig
    {
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public ValueMode ValueModes { get; set; } = ValueMode.Literal;
        public KnownClassType ClassType { get; set; } = KnownClassType.String;

        public enum ValueMode : byte
        {
            Literal,
            Reference
        }
    }
    
    
}