# Gjallarhorn

A blazing website to compare versions of packages on different NuGet sources.

> Currently working on shipping this with Docker

## The idea

Instead of making packages publicly available, you might want to release packages to only a limited audience, such as your organization or workgroup.
This web-client provides a simple way of monitoring the different versions of a package on different sources.

## Technologies

- ASP.NET Core 3.0
- Blazor

## How to use it

- Open port `1337`
- `cd src/Gjallarhorn.Server`
- `dotnet run`


# Demonstration
When first starting the web-site you will enter a blank *status*-page. What you need to do is to go to *edit*-page and start writing JSON.

Here is a example of a potential JSON where we compare *LightInject* with two sources: ``https://api.nuget.org/v3`` and `<MyFakeFeedUrl>`.
We also set `aliases` to the feeds, which is a display message in the *status* page.
![BeforeFetch]

## Here is the JSON

```json
{
  "SourceComparers": [
    {
      "SourceA": "https://api.nuget.org/v3/",
      "SourceAAlias": "NuGet",
      "SourceB": "<MyFakeFeedUrl>",
      "SourceBAlias": "My fake feed",
      "Packages": [
        {
          "Name": "LightInject",
          "ComparePreRelease": true
        }
      ]
    }
  ]
}
```

When we press *Save* we jump back to the *status*-page, and we start fetching versions from the two feeds:
![DuringFetch]

After the fetching has finished, we get a simplified view of the package status:
![AfterFetch]


If we want to add more packages to this two comparing sources, we do the following:
```json
{
  "SourceComparers": [
    {
      "SourceA": "https://api.nuget.org/v3/",
      "SourceAAlias": "NuGet",
      "SourceB": "<MyFakeFeedUrl>",
      "SourceBAlias": "My fake feed",
      "Packages": [
        {
          "Name": "LightInject",
          "ComparePreRelease": true
        },
        {
          "Name" : "Newtonsoft.Json",
          "ComparePreRelease": true
        }
      ]
    }
  ]
}
```

If we want to add different comparing sources, we do the following:
```json
{
  "SourceComparers": [
    {
      "SourceA": "https://api.nuget.org/v3/",
      "SourceAAlias": "NuGet",
      "SourceB": "<MyFakeFeedUrl>",
      "SourceBAlias": "My fake feed",
      "Packages": [
        {
          "Name": "LightInject",
          "ComparePreRelease": true
        },
        {
          "Name" : "Newtonsoft.Json",
          "ComparePreRelease": true
        }
      ]
    },
    {
      "SourceA": "https://api.nuget.org/v3/",
      "SourceAAlias": "NuGet",
      "SourceB": "<AnotherFakeUrl>",
      "SourceBAlias": "My other fake feed",
      "Packages": [
        {
          "Name": "LightInject",
          "ComparePreRelease": true
        },
        {
          "Name" : "Newtonsoft.Json",
          "ComparePreRelease": true
        }
      ]
    }

  ]
}
```
## Options when creating JSON in edit-mode

*ComparePrerelease* is optional, and is set to false if left empty.

[BeforeFetch]: doc/img/BeforeFetch.png
[DuringFetch]: doc/img/DuringFetch.png
[AfterFetch]: doc/img/AfterFetch.png
