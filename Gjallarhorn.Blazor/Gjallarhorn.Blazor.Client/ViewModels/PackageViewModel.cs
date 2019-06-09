using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class PackageViewModel
    {
        private Package m_package;

        public PackageViewModel(Package package)
        {
            m_package = package;
        }

        public string Name => m_package.Name;

        public string SourceAVersion => m_package.SourceAVersion;

        public string SourceBVersion => m_package.SourceBVersion;

        public string SourceA => m_package.SourceA;

        public string SourceB => m_package.SourceB;
    }
}