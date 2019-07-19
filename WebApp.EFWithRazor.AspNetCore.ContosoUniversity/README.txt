1. Create a new web app, select web apllication
2. Setup the site style: 
	2.1 update Pages/Shared/_Layout.cshtml
		...
		<title>@ViewData["Title"] : Contoso University</title>
		.....
       		<a asp-page="/Index" class="navbar-brand">Contoso University</a>
				......
						<li><a asp-page="/Index">Home</a></li>
						<li><a asp-page="/About">About</a></li>
						<li><a asp-page="/Students/Index">Students</a></li>
						<li><a asp-page="/Courses/Index">Courses</a></li>
						<li><a asp-page="/Instructors/Index">Instructors</a></li>
						<li><a asp-page="/Departments/Index">Departments</a></li>
		.......
		<p>&copy; 2018 : Contoso University</p>
	
	2.2 update Pages/Index.cshtml
		@page
		@model IndexModel
		@{
			ViewData["Title"] = "Home page";
		}

		<div class="jumbotron">
			<h1>Contoso University</h1>
		</div>
		<div class="row">
			<div class="col-md-4">
				<h2>Welcome to Contoso University</h2>
				<p>
					Contoso University is a sample application that
					demonstrates how to use Entity Framework Core in an
					ASP.NET Core Razor Pages web app.
				</p>
			</div>
			<div class="col-md-4">
				<h2>Build it from scratch</h2>
				<p>You can build the application by following the steps in a series of tutorials.</p>
				<p>
					<a class="btn btn-default"
					   href="https://docs.microsoft.com/aspnet/core/data/ef-rp/intro">
						See the tutorial &raquo;
					</a>
				</p>
			</div>
			<div class="col-md-4">
				<h2>Download it</h2>
				<p>You can download the completed project from GitHub.</p>
				<p>
					<a class="btn btn-default"
					   href="https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/data/ef-rp/intro/samples/cu-final">
						See project source code &raquo;
					</a>
				</p>
			</div>
		</div>

3. Create the data model
	create Models folder-> create Student.cs, Enrollment.cs, Course.cs

4. Scaffold the student model
	In Solution Explorer, right click on the Pages/Students folder > Add > New Scaffolded Item.
	In the Add Scaffold dialog, select Razor Pages using Entity Framework (CRUD) > ADD.

	The Add Razor Pages using Entity Framework (CRUD) dialog:
	In the Model class drop-down, select Student (ContosoUniversity.Models).
	In the Data context class row, select the + (plus) sign and change the generated name to ContosoUniversity.Models.SchoolContext.
	In the Data context class drop-down, select ContosoUniversity.Models.SchoolContext
	Select Add.

	Files created
	Pages/Students Create, Delete, Details, Edit, Index.
	Data/SchoolContext.cs

	File updates
	Startup.cs : Changes to this file are detailed in the next section.
	appsettings.json : The connection string used to connect to A LOCAL DATABASE IS ADDED.

5. Examine the context registered with dependency injection, Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    .........
    services.AddDbContext<SchoolContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("SchoolContext")));
}

6.Update main in Program.cs
 public static void Main(string[] args)
{
    var host = CreateWebHostBuilder(args).Build();
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<SchoolContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
    host.Run();
}

*******pay attention to EnsureCreated();

7. Examine the SchoolContext DB context
public class SchoolContext : DbContext
{
    ......................
	//each line = table in Db
    public DbSet<Student> Student { get; set; } 
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<Course> Course { get; set; }
}

8.Add code to initialize the DB with test data
Add Data\DbInitializer.cs
******Pay attention to name space Data and Models

In Program.cs, modify the Main method to call Initialize() from DBInitializer instead of EnsureCreated()

9. Run test app or View the DB

10. Customize the scaffolded CRUD(create, read, update and delete)
10.1 Details Page
a.  Pages/Students/Details.cshtml
Change the page directive for each of these pages from 
	@page to @page "{id:int?}" => not clear yet

b. Pages/Students/Details.cshtml.cs
Add related data
public async Task<IActionResult> OnGetAsync(int? id)
{
    ...................

    Student = await _context.Student
                        .Include(s => s.Enrollments)
                            .ThenInclude(e => e.Course)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.ID == id);
	..................
}
c. Pages/Students/Details.cshtml
Display related enrollments on the Details page
....
<dt>
	@Html.DisplayNameFor(model => model.Student.Enrollments)
</dt>
<dd>
    <table class="table">
        <tr>
            <th>Course Title</th>
            <th>Grade</th>
        </tr>
        @foreach (var item in Model.Student.Enrollments)
        {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.Course.Title)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Grade)
                </td>
            </tr>
        }
    </table>
</dd>

10.2 Create Page

public async Task<IActionResult> OnPostAsync()
{
    ........
    var emptyStudent = new Student();

    if (await TryUpdateModelAsync<Student>(
        emptyStudent,
        "student",   // Prefix for form value.
        s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
    {
        _context.Student.Add(emptyStudent);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    return null;
}


View model ??????????

10.3 Edit Page
public async Task<IActionResult> OnGetAsync(int? id)
    {
        ................

        Student = await _context.Student.FindAsync(id);

        ..........................
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
       ......................

        var studentToUpdate = await _context.Student.FindAsync(id);

        if (await TryUpdateModelAsync<Student>(
            studentToUpdate,
            "student",
            s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }

10.4 Delete Page
public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
{
    .............

    Student = await _context.Student
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.ID == id);

    ............................

    if (saveChangesError.GetValueOrDefault())
    {
        ErrorMessage = "Delete failed. Try again";
    }

    return Page();
}
public async Task<IActionResult> OnPostAsync(int? id)
{
    ...............

    var student = await _context.Student
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ID == id);

    if (student == null)
    {
        return NotFound();
    }

    try
    {
        _context.Student.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
    catch (DbUpdateException /* ex */)
    {
        //Log the error (uncomment ex variable name and write a log.)
        return RedirectToAction("./Delete",
                             new { id, saveChangesError = true });
    }
}
****pay attention to : EntiyState: add, update, remove, detach

11. Sort, Filter and paging
11.1 
Add sorting to the Index page on LastName and EnrollmentDate
 Students/Index.cshtml.cs PageModel
 Students/Index.cshtml.cs OnGetAsync
Add column heading hyperlinks to the student index page
Students/Index.cshtml

11.2 Add a Search Box to the Students Index page
Add filtering functionality to the Index method, 
Students/Index.cshtml.cs OnGetAsync
Add a Search Box to the Student Index page
Pages/Students/Index.cshtml

11.3 Add paging functionality to the Students Index page
In the project folder, create PaginatedList.cs
Add paging links to the student Razor Page
	-Students/Index.cshtml.cs OnGetAsync
	-Update the Students/Index.cshtml.cs OnGetAsync
	tại sao pageIndex = 2 khi click next button????????
	nó tự generate và gán vào <a> trên view, inspect nut Next sẽ thấy cái link ntn


12. Update the About page to show student statistics
12.1 Create the view model, add code
12.2 Update the About page model
12.3 Modify the About Razor Page