namespace Gjallarhorn.Shared
{
    public class Package
    {
        public string Name { get; set; }
        public string SourceA { get; set; }
        public string SourceAAlias { get; set; }
        public string SourceAVersion { get; set; }
        public string SourceB { get; set; }
        public string SourceBAlias { get; set; }
        public string SourceBVersion { get; set; }
        public bool ComparePreRelease { get; set; }
    }
}