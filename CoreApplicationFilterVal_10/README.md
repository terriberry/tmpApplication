<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://user-images.githubusercontent.com/103587065/176515178-464be717-859a-4006-9100-123a926970bc.png">
  <source media="(prefers-color-scheme: light)" srcset="https://user-images.githubusercontent.com/103587065/176515152-3f1ff456-147a-4e5a-8e89-486b375df0a8.png">
  <img alt="The Delta Studio Logo" position="right" align="right" width=400>
</picture>

# CoreApplicationFilterVal_10


  
    
## Introduction
[Here](docs/introduction.md) are the fundamental concepts relevant to the project

## Installing and running the project on .NET

Here is how to get the project up and running on your local machine:

This project uses .NET ```6```. Make sure you have the correct .NET SDK installed in your machine. Run the following command to check the versions:

```shell
dotnet --list-sdks
```
Output similar to the following appears:
```shell
3.1.100 [C:\program files\dotnet\sdk]
5.0.100 [C:\program files\dotnet\sdk]
6.0.100 [C:\program files\dotnet\sdk]
```

Hit F5 if using Visual Studio to build and run the project. If Logging to AWS Cloudwatch sink module was enabled, choose the "<project name> - Local" launch profile to log to console. If using the dotnet CLI, run ```dotnet run```. If Logging to AWS Cloudwatch sink module was enabled, specify the launch profile as follows to log to console: ```dotnet run --launch-profile "<project name> - Local"```. You can find all the launch profiles in the launchsettings.json.</br></br>

The project is by default running on localhost: ```http://localhost:xxxxx/```.
Swagger is pre-configured and will automatically route to ```http://localhost:xxxxx/swagger/index.html``` which displays the swagger doc. Note that if the application is run without the launch profile explicitely specified, swagger will not run automatically and you will have to manually append ```/swagger/index.html``` to the URL.


## Configurations

[Here](docs/configuration.md) is the list of all the configuration options for this project.


## Developing using Intent Architect

### Install and setup Intent Architect
The first thing that is required before any dev work can begin is the installation and setup of Intent Architect. A detailed guide on how to do this is documented [here](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/platform-application-templates/setup).</br>

### Working with an existing project in Intent
In most cases, the project will have already been created, and you are required to continue with the development. [Here](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/platform-application-templates/the-template-runbooks/custom-logic-apis/clean-architecture-cqrs-api/open-a-pre-existing-project) is a detailed guide on how to get you started with an already existing project. Below are the summarized steps:</br>
>**Step 1**: Start by cloning the project from the Github repo into your local machine </br></br>
**Step 2**: Open the ```.isln``` file, this will open Intent Architect with the necessary files for the project. The ```.isln``` file is placed inside the ```intent``` folder. Alternatively, open Intent Architect and click: Open an existing solution and find the .isln file</br></br>
**Step 3**: Open the ```.sln``` file inside the IDE of your choice.  This is the codebase for the project. Now you have the recommended setup in place i.e. the ```.isln``` open in Intent Architect and ```.sln``` open in the IDE of choice</br></br>
**Step 4**: Develop using the designers in the panel on the left. [Here](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/the-delta-code-smith/the-template-runbooks) is the runbook on how to develop using Intent Architect for all the application templates available</br></br>
**Step 5**: After making changes in Intent Architect, save and run the Software Factory and apply the staged changes. More information about Software Factory is given under the ```Running the software factory``` section below

This is the general work flow using the Intent Architect software i.e. make domain and service layer changes using the designers in Intent Architect, run the software factory to implement the changes and run the code inside the IDE of choice.</br>

### Running the software factory
The Software Factory is where the magic happens. When run by pressing ```F5```, or simply by clicking ```Run Software Factory```, the following process is initiated:
1. Examines the changes made in the designers (the entity classes, the business logic etc.)
2. Auto-generates and modifies the necessary C# files to implement the changes
3. Lists all the files generated and modified into a staging environment which you can examine
4. Allows you to choose the files to apply the changes and when you click ```APPLY CHANGES```, the change/s are automatically implemented in the codebase

### Creating a new project using Intent
If you decide to create an entirely new project using Intent Architect, here is the [link](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/platform-application-templates/the-template-runbooks/custom-logic-apis/clean-architecture-cqrs-api/create-and-configure-the-project) to show to exactly how to do this.
  
    
### Migrations
Migrations are done using Entity Framework Core. [Here](docs/migration.md) is a link that details how to perform the migration
            
  
    
### Unit Testing Guide

The XUnit Module has been included in this application. Unit Testing forms an important part of ensuring the quality of any software project. This module aims to make the process of writing and managing tests faster while ensuring a consistency in test formatting.

The two links below will be useful in helping create tests with the XUnit Module:
* The Delta guide for [unit testing standards](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/-MlB4qjufhlVd_bQNyus/development-handbook/testing/standards/unit-tests) with some useful links.
* The [Platform Runbook for creating Unit Tests](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/the-delta-code-smith/the-template-runbooks/custom-logic-apis/core-api/add-your-unit-tests) with Delta Code Smith.
            
