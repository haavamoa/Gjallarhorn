using System;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        private Package m_package;

        public PackageViewModel(Package package)
        {
            m_package = package;
            RemovePackageCommand = new DelegateCommand(async _ =>  await m_packagesHandler?.RemovePackage(this));
        }

        public void Initialize(IHandlePackages packagesHandler)
        {
            m_packagesHandler = packagesHandler;
        }

        public void UpdatePackage(Package package)
        {
            m_package = package;
            OnPropertyChanged();
        }

        public ICommand RemovePackageCommand { get; }

        public string Name => m_package.Name;

        public string SourceAVersion => m_package.SourceAVersion;

        public string SourceBVersion => m_package.SourceBVersion;

        public string SourceA => m_package.SourceA;

        public string SourceB => m_package.SourceB;

        private bool m_isFetching;

        public bool IsFetching
        {
            get => m_isFetching;
            set => SetProperty(ref m_isFetching, value);
        }

        public bool IsLatest => m_package.SourceAVersion == m_package.SourceBVersion;

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