# README

Steps when starting new application on Exam

1. Rebuild and check for errors
2. Set Web as Startup Project with Right Click + Set as Startup Project
3. Change Default Connection to the Database Connection String in appsetings.json (If there is no such file, create it) with the content:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "ConnectionStringFromDb"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```
5. Apply Migrations
6. Rebuild the app
7. Start and check if everything is working
8. Register -> VERIFY YOUR EMAIL -> Login
9. Set IEntityNameRepository and EntityNameRepository, add and implement CRUD operation both in the interface and the repository
10. Register your Repositories and Services in Program.cs
11. Check if Some virtual or references to another model in some model need to be set to be instantiated inside the class to not be null
12. To be continued...
