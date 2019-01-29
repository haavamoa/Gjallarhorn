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

## Example-JSON to use when subscribing to packages

```json
{
    "SourceComparers": [
        {
            "sourceA": "https://api.nuget.org/v3/",
            "sourceB": "https://api.nuget.org/v3/",
            "Packages": [
                {
                    "name": "LightInject"
                }
            ]
        }
    ]
}
```


## Options when creating JSON in edit-mode


#### Compare prerelease
```json
{
    "SourceComparers": [
        {
            "sourceA": "https://api.nuget.org/v3/",
            "sourceB": "https://api.nuget.org/v3/",
            "Packages": [
                {
                    "name": "LightInject",
                    "comparePrerelease" : "true"
                }
            ]
        }
    ]
}
```

## Optional query strings

To **opt-in** functionality that can be useful, you can provide queries with values to the URI.

### `?minimized=true`

Get a minimized version of the view. Shows only the compared packages.

> Note: You have to go back to a non-minimzed view in order to turn off functionallity that normally is added with buttons.
