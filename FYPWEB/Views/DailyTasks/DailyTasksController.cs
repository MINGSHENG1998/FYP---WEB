using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYPWEB.Data;
using FYPWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FYPWEB.Views.DailyTasks
{
    public class DailyTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public DailyTasksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DailyTasks
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchString, string Type)
        {
            var DailyTask = from m in _context.DailyTask select m;

            IQueryable<string> TypeQuery = from m in _context.DailyTask
                                           orderby m.Day
                                           select m.Day;
            IEnumerable<SelectListItem> items =
                new SelectList(await TypeQuery.Distinct().ToListAsync());

            ViewBag.Type = items;
            if (!String.IsNullOrEmpty(SearchString)) // if got any search word
            {
                DailyTask = DailyTask.Where(s => s.TaskName.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(Type)) // if got any search word
            {
                DailyTask = DailyTask.Where(s => s.Day == Type);
            }


            return View(await DailyTask.ToListAsync());
        }


        // GET: DailyTasks/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTask
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dailyTask == null)
            {
                return NotFound();
            }

            return View(dailyTask);
        }

        // GET: DailyTasks/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        



        // POST: DailyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TaskName,Day,Difficulty,Point")] DailyTask dailyTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dailyTask);
        }

        // GET: DailyTasks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTask.FindAsync(id);
            if (dailyTask == null)
            {
                return NotFound();
            }
            return View(dailyTask);
        }

        // POST: DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TaskName,Day,Difficulty,Point")] DailyTask dailyTask)
        {
            if (id != dailyTask.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyTaskExists(dailyTask.ID))
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
            return View(dailyTask);
        }

        // GET: DailyTasks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTask
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dailyTask == null)
            {
                return NotFound();
            }

            return View(dailyTask);
        }

        // POST: DailyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyTask = await _context.DailyTask.FindAsync(id);
            _context.DailyTask.Remove(dailyTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyTaskExists(int id)
        {
            return _context.DailyTask.Any(e => e.ID == id);
        }
    }
}

