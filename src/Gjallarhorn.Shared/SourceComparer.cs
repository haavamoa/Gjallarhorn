using System.Collections.Generic;

namespace Gjallarhorn.Shared
{
    public class SourceComparer
    {
        public string SourceA{get;set;}
        public string SourceAAlias { get; set; }
        public string SourceB{get;set;}
        public string SourceBAlias { get; set; }
        public List<Package> Packages{get;set;}
    }
}