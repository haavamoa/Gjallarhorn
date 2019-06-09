using System.Runtime.CompilerServices;

namespace Gjallarhorn.Blazor.Shared
{
    public class Package
    {
        public string Name { get; set; }
        public string SourceA { get; set; }
        public string SourceAVersion { get; set; }
        public string SourceB { get; set; }
        public string SourceBVersion { get; set; }
        public bool ComparePrerelease { get; set; }
    }
}