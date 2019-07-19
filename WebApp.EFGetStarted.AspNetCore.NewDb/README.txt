1. Create a new project
2. Create the model with expected tables in Models folder

3. Register the context with dependency injection in Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    ........

    var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
    services.AddDbContext<BloggingContext>
        (options => options.UseSqlServer(connection));
    // BloggingContext requires
    // using EFGetStarted.AspNetCore.NewDb.Models;
    // UseSqlServer requires
    // using Microsoft.EntityFrameworkCore;
}

4. Create the database
Add-Migration InitialCreate  // pay attention to Primary key
Update-Database

5. Create the controller, select MVC Controller with views, using Entity Framework
6. Run the application