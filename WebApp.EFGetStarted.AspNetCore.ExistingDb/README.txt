1. Create dbBloggingKP database in Server Explorer
2. Create a new project, File -> New Project
3. Reverse engineer your model, run Package Manager Console

Scaffold-DbContext "Server=ezitwk028\sql2017;Database=dbBloggingKP;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

new created files
	Blog.cs
	Post.cs
	dbBloggingKPContext.cs

4. Register your context with dependency injection in Statup.cs
using EFGetStarted.AspNetCore.ExistingDb.Models;
using Microsoft.EntityFrameworkCore;

public void ConfigureServices(IServiceCollection services)
{
    .........

    var connection = @"Server=ezitwk028\sql2017;Database=dbBloggingKP;Trusted_Connection=True;ConnectRetryCount=0";
    services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connection));
}

5. Create a controller and views, select MVC Controller with views, using Entity Framework 