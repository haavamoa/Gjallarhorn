using System;
using Gjallarhorn.Shared;

namespace Gjallarhorn.Server.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        private bool m_comparePreRelease;

        private DateTime m_fetchDate;

        private bool m_isFetching;

        public PackageViewModel(Package package)
        {
            Package = package;
        }

        public Package Package { get; private set; }

        public string Name => Package.Name;

        public string SourceAVersion => Package.SourceAVersion;

        public string SourceBVersion => Package.SourceBVersion;

        public string SourceA => Package.SourceA;

        public string SourceAAlias => Package.SourceAAlias;

        public string SourceB => Package.SourceB;
        public string SourceBAlias => Package.SourceBAlias;

        public bool IsFetching
        {
            get => m_isFetching;
            set => SetProperty(ref m_isFetching, value);
        }

        public bool IsLatest => Package.SourceAVersion == Package.SourceBVersion;

        public DateTime FetchDate
        {
            get => m_fetchDate;
            set => SetProperty(ref m_fetchDate, value);
        }

        public bool ComparePreRelease
        {
            get => m_comparePreRelease;
            set => SetProperty(ref m_comparePreRelease, value);
        }

        public void UpdateVersions(string sourceAVersion, string sourceBVersion)
        {
            Package.SourceAVersion = sourceAVersion;
            Package.SourceBVersion = sourceBVersion;
        }
    }
}