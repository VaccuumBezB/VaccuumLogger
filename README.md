# Vaccuum Logger

_Simple logging system for.NET applications and Unity.
This library is suitable for most C# applications, but is aimed at unity games and desktop dotnet-app_

## Installation

you can use this NuGet package
`sudo dotnet add package https://github.com/VaccuumBezB/VaccuumLogger/blob/main/VaccuumLogistics.1.0.0.nupkg`

### _or_

you can use git and pack it yourself

## Usage

### Announcement

var logger = new Logger("namehere"); //Will create namehere.log
Logger.Log("Application Launched");

### Basic methods

-`Log()` - regular message
-`Info()` - information message
-`Success()` - successful execution
-`Warn()` - warning
-`Error()` - error
-`Critical()` - critical error
-`Exception()` - to log exceptions

## Setup

Logger.Configure(
showTimestamp: true,//Show time
showStackTrace: true,//Show call stack
openLogsOnCritical: true //Autopening logs for critical errors
); 

### An example of using 

`try
{
Logger.Info("File download...");
// ... code...
Logger.Success("File loaded");
}
catch (Exception ex)
{
Logger.Exception(ex);
}` 
All logs are saved to the `[name].log` file and duplicated into a color-backed console.
