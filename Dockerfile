#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.100-preview6-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview6-nanoserver-1903 AS build
WORKDIR /src
COPY ["src/Gjallarhorn.Server/Gjallarhorn.Server.csproj", "Gjallarhorn.Server/"]
COPY ["src/Gjallarhorn.Shared/Gjallarhorn.Shared.csproj", "Gjallarhorn.Shared/"]
COPY ["src/Gjallarhorn.Client/Gjallarhorn.Client.csproj", "Gjallarhorn.Client/"]
RUN dotnet restore "Gjallarhorn.Server/Gjallarhorn.Server.csproj"
COPY . .
WORKDIR "/src/Gjallarhorn.Server"
RUN dotnet build "Gjallarhorn.Server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Gjallarhorn.Server.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gjallarhorn.Server.dll"]