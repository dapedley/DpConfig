## DpConfig - Sets up console configuration and logging

Namespace: DpUtilities

Nuget Package Name: DpConfig

Dependencies: DpUtilities, Serilog

### Introduction

May be called at the head of a console application to create a configuration environment and (optionally) a Serilog logger. The configuration gives access to environment variables and to an appsettings.json file, which is assumed to be at the application's root.

If a logger is specified, the appsettings.json file must contain a valid `Serilog` section.

The logger can be accessed by the global static instance `Log`.

### Constructor

`public DpConfig (bool withLogging = true)`

Logging is assumed by default. Set `withLogging` to false to set up configuration without logging.

### Property

#### Property Config - Returns the configuration instance

`public IConfigurationRoot Config { get; private set; }`

Once the configuration has been constructed, a reference to it is available to the main program via this property.

### Sample `Serilog` section in `appsettings.json`

```
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
        "Override": {
          "Microsoft.EntityFrameworkCore.Database.Command": "Error",
          "Microsoft.EntityFrameworkCore": "Error",
          "Microsoft": "Warning",
          "System": "Information"
        }
    },

    "WriteTo": [
    {
      "Name": "Console",
      "Args": {
        "outputTemplate": "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
      }
    },
    {
      "Name": "File",
      "Args": {
        "path": "d:\\Temp\\LogTest.log",
        "rollingInterval": "Day",
        "retainedFileCountLimit": 31,
        "outputTemplate": "{Timestamp:yy-MMM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
      }
    }
    ]
  },

  "TestSection": {
    "TestString": "My Test String"
  }
}
```
