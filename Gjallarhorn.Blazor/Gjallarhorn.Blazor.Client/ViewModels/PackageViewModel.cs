using System;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        public Package Package { get; private set; }

        public PackageViewModel(Package package)
        {
            Package = package;
            RemovePackageCommand = new DelegateCommand(async _ =>  await m_packagesHandler?.RemovePackage(this));
        }

        public void Initialize(IHandlePackages packagesHandler)
        {
            m_packagesHandler = packagesHandler;
        }

        public void UpdatePackage(Package package)
        {
            Package = package;
            OnPropertyChanged();
        }

        public ICommand RemovePackageCommand { get; }

        public string Name => Package.Name;

        public string SourceAVersion => Package.SourceAVersion;

        public string SourceBVersion => Package.SourceBVersion;

        public string SourceA => Package.SourceA;

        public string SourceB => Package.SourceB;

        private bool m_isFetching;

        public bool IsFetching
        {
            get => m_isFetching;
            set => SetProperty(ref m_isFetching, value);
        }

        public bool IsLatest => Package.SourceAVersion == Package.SourceBVersion;

        private DateTime m_fetchDate;

        public DateTime FetchDate
        {
            get => m_fetchDate;
            set => SetProperty(ref m_fetchDate, value);
        }

        private bool m_comparePreRelease;
        private IHandlePackages? m_packagesHandler;

        public bool ComparePreRelease
        {
            get => m_comparePreRelease;
            set => SetProperty(ref m_comparePreRelease, value);
        }
    }
}