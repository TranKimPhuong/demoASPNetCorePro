using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.EFWithRazor.AspNetCore.ContosoUniversity.Models;

namespace WebApp.EFWithRazor.AspNetCore.ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public IndexModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public PaginatedList<Student> Student { get; set; }
        public string nameSort { get; set; }
        public string dateSort { get; set; }
        public string currentFilter { get; set; }
        public string currentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter
                                    ,string searchString, int? pageIndex)
        {
            currentSort = sortOrder;
            nameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            dateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
                pageIndex = 1;
            else
                searchString = currentFilter;

            currentFilter = searchString;

            IQueryable<Student> studentIQ = from s in _context.Student
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.LastName.ToLower().Contains(searchString.ToLower())
                                                || s.FirstMidName.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 3;

            Student = await PaginatedList<Student>.CreateAsync(
                studentIQ.AsNoTracking(), pageIndex?? 1, pageSize);
        }
    }
}
