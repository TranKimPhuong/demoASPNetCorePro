using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.EFGetStarted.AspNetCore.NewDb.Models;

namespace WebApp.EFGetStarted.AspNetCore.NewDb.Controllers
{
    public class tbBlogsController : Controller
    {
        private readonly dbBloggingKP2Context _context;

        public tbBlogsController(dbBloggingKP2Context context)
        {
            _context = context;
        }

        // GET: tbBlogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        // GET: tbBlogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbBlog == null)
            {
                return NotFound();
            }

            return View(tbBlog);
        }

        // GET: tbBlogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: tbBlogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url")] tbBlog tbBlog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbBlog);
        }

        // GET: tbBlogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.Blogs.FindAsync(id);
            if (tbBlog == null)
            {
                return NotFound();
            }
            return View(tbBlog);
        }

        // POST: tbBlogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url")] tbBlog tbBlog)
        {
            if (id != tbBlog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tbBlogExists(tbBlog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbBlog);
        }

        // GET: tbBlogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbBlog == null)
            {
                return NotFound();
            }

            return View(tbBlog);
        }

        // POST: tbBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBlog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(tbBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tbBlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
