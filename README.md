# Stored Procedure Task Scheduler
Stored-Procedure-Task-Scheduler is a robust Windows service designed to automate the execution of SQL stored procedures at configurable intervals. Whether you need to run procedures weekly, monthly, or at fixed times, this service provides a flexible and reliable solution for managing scheduled database tasks.  
## Features
* **Customizable Scheduling**: Define the frequency and specific times for running stored procedures, including daily, weekly, monthly, or at fixed intervals..
* **Multiple Task Management**: Easily configure and manage multiple stored procedures with distinct schedules within a single service instance.
* **Logging and Monitoring**: Comprehensive logging of task execution and status to ensure visibility and troubleshooting capabilities.
* **Configurable Intervals**: Supports various scheduling options, from simple daily tasks to complex monthly schedules.
* **Ease of Deployment**: Simple installation and configuration process to integrate seamlessly into your existing environment.


## How to approach
* Install .NET SDK v4.8
* Clone the repository in local File System.
* Run `dotnet restore` command to generate object files and download dependencies for the project.
* Now run `dotnet build` command to generate build files in `bin/` folder.
* Install the exe as Service using Windows Service Manager( [Link](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create)) or Create EXE Setup file [Link](https://www.advancedinstaller.com/create-setup-exe-visual-studio.html). The project for EXE Setup is already present in repository 
* Edit the Configuration file `BaseDirectory/SchedulerConfig.json`
* Start the service using Service Manager.

And all set for the day : thumbsup:

 *If you encounter any issues or have any questions, please feel free to reach out.*
 ## \#FeelFreetoConnect
   
