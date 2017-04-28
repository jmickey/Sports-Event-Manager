# Sports Event Manager
Olympic/Commonwealth style games and events manager web application written in C#, leveraging off ASP.NET MVC (.NET Framework 4.6.2). Developed for the purposes of a university assignment in semester 2 2016.

Technologies used within this project include:
* Bootstrap CSS Framework - allowing for rapid prototyping of layout and design.
* MS SQL Express LocalDB - lightweight database for data storage.
* Code First Entity Framework - provides a layer of abstration on top of the database. Database entities are represented within code classes.
  * Representing database entities and relationships in code provides greater visibility.
  * Empowers rapid application development via reduction of database management and design overhead.
    * Database creation and migration is handed by entity framework.
    * Database design can occur along side application development.
  * Iterative approach to database design provides flexibility and enables continous improvement.

## How to Run
Clone repo and open soluton in VS2015+. Restore NuGet packages and run Update-Database within the Package Manager Console.
