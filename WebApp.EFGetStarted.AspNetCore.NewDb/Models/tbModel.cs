using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.EFGetStarted.AspNetCore.NewDb.Models
{
    public class dbBloggingKP2Context : DbContext
    {
        public dbBloggingKP2Context(DbContextOptions<dbBloggingKP2Context> options)
            : base(options)
        { }

        public DbSet<tbBlog> Blogs { get; set; }
        public DbSet<tbPost> Posts { get; set; }
    }

    public class tbBlog
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public ICollection<tbPost> Posts { get; set; }
    }

    public class tbPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public tbBlog Blog { get; set; }
    }
}
