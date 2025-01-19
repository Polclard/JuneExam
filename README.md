Steps when starting new application on Exam

1. Rebuild and check for errors
2. Set Web as Startup Project with Right Click + Set as Startup Project
3. Change Default Connection to the Database Connection String in appsetings.json (If there is no such file, create it) with the content:

``appsettings.json``
```json
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
4. Apply Migrations
5. Rebuild the app
6. Start and check if everything is working
7. Register -> VERIFY YOUR EMAIL -> Login
8. Set IEntityNameRepository and EntityNameRepository, add and implement CRUD operation both in the interface and the repository
9. Register your Repositories and Services in Program.cs
10. Check if Some virtual or references to another model in some model need to be set to be instantiated inside the class to not be null
11. If there is no explicit EAGER loading, just do that in Repository.cs by adding the classes that you need to be EAGRLY Loaded. So later
you can escape the problem of Objects or List of objects not loading in the main Model that you are accessing. For example if an Employee has List of HealthExamination inside the model, then include with .Include() in Repository the HealthExaminations before returning the list.

``Repository.cs``
```cs
public IEnumerable<T> GetAll()
        {
            if (typeof(T).IsAssignableFrom(typeof(HealthExamination)))
            {
                return entities
                    .Include("Polyclinic")
                    .Include("Employee")
                    .AsEnumerable();
            }
            if (typeof(T).IsAssignableFrom(typeof(Polyclinic)))
            {
                return entities
                    .Include("HealthExaminations")
                    .AsEnumerable();
            }
            if (typeof(T).IsAssignableFrom(typeof(Employee)))
            {
                return entities
                    .Include("Company")
                    .Include("HealthExaminations")
                    .AsEnumerable();
            }
            else
            {
                return entities.AsEnumerable();
            }
        }

        public T Get(Guid? id)
        {
            if (typeof(T).IsAssignableFrom(typeof(HealthExamination)))
            {
                return entities
                    .Include("Polyclinic")
                    .Include("Employee")
                    .First(s => s.Id == id);
            }
            if (typeof(T).IsAssignableFrom(typeof(Polyclinic)))
            {
                return entities
                    .Include("HealthExaminations")
                    .First(s => s.Id == id);
            }
            /*************************************************/
            if (typeof(T).IsAssignableFrom(typeof(Employee)))
            {
                return entities
                    .Include("Company")
                    .Include("HealthExaminations")
                    .Include("HealthExaminations.Polyclinic")
                    .First(s => s.Id == id);
            }
            /*
            /*************************************************/
            else
            {
                return entities.First(s => s.Id == id);
            }

        }
```
