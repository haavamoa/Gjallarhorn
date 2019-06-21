# Build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview6-alpine3.9 AS build-stage

COPY src /src

WORKDIR /src/Gjallarhorn.Server

RUN dotnet build -c release

WORKDIR  /src/Gjallarhorn.Server/bin/release/netcoreapp3.0

RUN cat Gjallarhorn.Client.blazor.config

ENV ASPNETCORE_URLS=http://+:1338

EXPOSE 1338

ENTRYPOINT [ "dotnet", "Gjallarhorn.Server.dll" ]