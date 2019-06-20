using System.Collections.Generic;
using FluentAssertions;
using Gjallarhorn.Client.Services.Http;
using Gjallarhorn.Client.Services.LocalStorage;
using Gjallarhorn.Client.Services.UserConfiguration;
using Gjallarhorn.Client.Storage;
using Gjallarhorn.Shared;
using Moq;
using Xunit;

namespace Gjallarhorn.Client.Tests.Services
{
    public class UserConfigurationServiceTests
    {
        private UserConfigurationService m_userConfigurationService;
        private readonly Mock<IHttpClientFactory> m_httpClientFactoryMock;
        private readonly Mock<ILocalStorage> m_localStorageMock;

        public UserConfigurationServiceTests()
        {
            m_httpClientFactoryMock = new Mock<IHttpClientFactory>();
            m_localStorageMock = new Mock<ILocalStorage>();

            m_userConfigurationService = new UserConfigurationService(m_httpClientFactoryMock.Object, m_localStorageMock.Object);
        }

        [Fact]
        public async void GetPackages_ValidUserConfigurationWithAliases_PackagesGetsAliases()
        {
            //Arrange
            var expectedAlias = "nuget";
            var fetchedUserConfiguration = new UserConfiguration()
            {
            SourceComparers = new List<SourceComparer>()
                    {
                        new SourceComparer()
                        {
                            SourceA = "https://api.nuget.org/v3/",
                            SourceAAlias = expectedAlias,
                            SourceB = "https://api.nuget.org/v3/",
                            SourceBAlias = expectedAlias,
                            Packages = new List<Package>()
                            {
                                new Package()
                                {
                                    Name = "LightInject"
                                }
                            }
                        }
                    }
            };
            m_localStorageMock.Setup(l => l.GetItem<UserConfiguration>(StorageConstants.Key)).ReturnsAsync(fetchedUserConfiguration);

            //Act
            var packages = await m_userConfigurationService.GetPackages();

            //Assert
            packages.Should().Contain(p => p.SourceAAlias.Equals(expectedAlias));
            packages.Should().Contain(p => p.SourceBAlias.Equals(expectedAlias));
        }
    }
}