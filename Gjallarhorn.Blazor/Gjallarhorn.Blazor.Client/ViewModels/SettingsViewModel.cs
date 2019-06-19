﻿using System.Threading.Tasks;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Services;
using Gjallarhorn.Blazor.Client.Services.UserConfiguration;
using Gjallarhorn.Blazor.Shared;
using Newtonsoft.Json;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserConfigurationService m_userConfigurationService;
        private string m_packagesRaw;
        private UserConfiguration m_userConfiguration;

        public SettingsViewModel(IUserConfigurationService userConfigurationService)
        {
            m_userConfigurationService = userConfigurationService;
            m_userConfiguration = new UserConfiguration();
            m_packagesRaw = string.Empty;
            SaveSettingsCommand = new AsyncCommand(SaveSettings);
        }

        public string PackagesRaw
        {
            get => m_packagesRaw;
            set => SetProperty(ref m_packagesRaw, value);
        }

        public ICommand SaveSettingsCommand { get; }

        private async Task SaveSettings(object o)
        {
            if (!(o is string settings)) return;

            var uc = JsonConvert.DeserializeObject<UserConfiguration>(settings);
            await m_userConfigurationService.SaveUserConfiguration(uc);
        }

        public async Task Initialize()
        {
            var userConfiguration = await m_userConfigurationService.GetUserConfiguration();
            PackagesRaw = JsonConvert.SerializeObject(
                userConfiguration,
                Formatting.Indented,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
        }
    }
}