# Gjallarhorn

A webclient to compare versions of packages on different NuGet sources.

## The idea

Instead of making packages publicly available, you might want to release packages to only a limited audience, such as your organization or workgroup.
This web-client provides a simple way of monitoring the different versions of a package on different sources.

## Technologies

- ASP.NET Core MVC
- Aurelia
- NodeJS
- Yarn
- Webpack

## How to use it

`cd src && npm install && dotnet run`

*Ps: Remember to port forward port : `16604`.*

## API

This also provides a API behind the scenes, see `src/api.http` for more information