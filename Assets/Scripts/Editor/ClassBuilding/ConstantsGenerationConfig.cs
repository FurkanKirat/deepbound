namespace Editor.ClassBuilding
{
    public class ConstantsGenerationConfig
    {
        public readonly string[] Folders;
        public readonly string[] Fields;
        public readonly string OutputPath;
        
        public ConstantsGenerationConfig(string[] folders, string[] fields, string outputPath)
        {
            Folders = folders;
            Fields = fields;
            OutputPath = outputPath;
        }
    }

}