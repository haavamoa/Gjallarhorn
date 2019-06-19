using System.Linq;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Services;
using Gjallarhorn.Blazor.Client.Services.UserConfiguration;
using Gjallarhorn.Blazor.Client.ViewModels;
using Gjallarhorn.Blazor.Shared;
using Moq;
using Xunit;

namespace Gjallarhorn.Blazor.Client.Tests.ViewModels
{
    public class SettingsViewModelTests
    {
        public SettingsViewModelTests()
        {
            m_userConfigurationServiceMock = new Mock<IUserConfigurationService>();
            m_settingsViewModel = new SettingsViewModel(m_userConfigurationServiceMock.Object);
        }

        private readonly SettingsViewModel m_settingsViewModel;
        private readonly Mock<IUserConfigurationService> m_userConfigurationServiceMock;

        [Fact]
        public async void SaveSettingsCommand_SettingIsValidUserConfiguration_BothSourceAliasesIsCorrect()
        {
            //Arrange
            var json =
                "{\r\n    \"SourceComparers\": [\r\n        {\r\n            \"sourceA\": \"https://api.nuget.org/v3/\",\r\n\t    \"sourceAAlias\": \"nuget\",\r\n            \"sourceB\": \"https://api.nuget.org/v3/\",\r\n\t    \"sourceBAlias\": \"nuget\",\r\n            \"Packages\": [\r\n                {\r\n                    \"name\": \"LightInject\"\r\n                }\r\n            ]\r\n        }\r\n    ]\r\n}";

            //Act
            await ((AsyncCommand)m_settingsViewModel.SaveSettingsCommand).ExecuteAsync(json);

            //Assert
            m_userConfigurationServiceMock.Verify(
                ucs => ucs.SaveUserConfiguration(
                    It.Is<UserConfiguration>(
                        u => u.SourceComparers.First().SourceAAlias.Equals("nuget") && u.SourceComparers.First().SourceBAlias.Equals("nuget"))));
        }
    }
}