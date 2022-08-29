# Configurations

## Environment
The environment for the project is set by the ```ASPNETCORE_ENVIRONEMNT``` key stored inside the ```launchsettings.json``` file. By default, there are several profiles generated for you to choose from when running the application on a local machine. The ASPNETCORE_ENVIRONMENT is either set to Local or Development.</br>

The profile to launch the application with can be selected as follows:

Using **dotnet CLI**:
```shell
dotnet run --launch-profile <profile>
```
For Visual Studio, the profile can be chosen by clicking the drop down next to the run button on the top left corner of the window:

<img src="https://user-images.githubusercontent.com/103587065/182316451-d80647c1-b29b-4341-a482-0448c209c943.png">
</br></br>

If the launch profile is not specified when running the application, the "IIS Express" and "<project name>.Api" profiles will be run for Visual Studio and dotnet CLI respectively, both with the "Development" ```ASPNETCORE_ENVIRONEMNT``` environment.

## appsettings.json and launchsettings.json
Configuration settings and keys can be stored in ```appsettings.json``` file and inside ```environmentVariables``` in the ```launchsettings.json``` file. The general convention is to store access keys and secrets inside the ```environmentVariables```  and general settings such as Logging parameters inside the appsettings.json.
</br></br>
    
## In Memory database
The appsettings.jsons will not show any ConnectionString values since in-memory was chosen.</br></br>
