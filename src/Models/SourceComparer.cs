using System.Collections.Generic;

namespace Client.Models
{
    public class SourceComparer
    {
        public string SourceA{get;set;}
        public string SourceB{get;set;}
        public List<Package> Packages{get;set;}
    }
}