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

namespace FYPWEB.Views.Doctor
{
    public class DoctorListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public DoctorListsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DoctorLists
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchString, string Type)
        {
            var DoctorList = from m in _context.DoctorList select m;

            IQueryable<string> TypeQuery = from m in _context.DoctorList
                                           orderby m.Country
                                           select m.Country;
            IEnumerable<SelectListItem> items =
                new SelectList(await TypeQuery.Distinct().ToListAsync());

            ViewBag.Type = items;
            if (!String.IsNullOrEmpty(SearchString)) // if got any search word
            {
                DoctorList = DoctorList.Where(s => s.DocName.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(Type)) // if got any search word
            {
                DoctorList = DoctorList.Where(s => s.Country == Type);
            }

            return View(await DoctorList.ToListAsync());
        }

        // GET: DoctorLists/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorList = await _context.DoctorList
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctorList == null)
            {
                return NotFound();
            }

            return View(doctorList);
        }
        public async Task<IActionResult> DoctorProfile(string SearchString, string Type)
        {
            var DoctorList = from m in _context.DoctorList select m;

            IQueryable<string> TypeQuery = from m in _context.DoctorList
                                           orderby m.Country
                                           select m.Country;
            IEnumerable<SelectListItem> items =
                new SelectList(await TypeQuery.Distinct().ToListAsync());

            ViewBag.Type = items;
            if (!String.IsNullOrEmpty(SearchString)) // if got any search word
            {
                DoctorList = DoctorList.Where(s => s.DocName.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(Type)) // if got any search word
            {
                DoctorList = DoctorList.Where(s => s.Country == Type);
            }
            return View(await DoctorList.ToListAsync());
        }

        // GET: DoctorLists/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoctorLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DocName,Email,Address,State,Country")] DoctorList doctorList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctorList);
        }

        // GET: DoctorLists/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorList = await _context.DoctorList.FindAsync(id);
            if (doctorList == null)
            {
                return NotFound();
            }
            return View(doctorList);
        }

        // POST: DoctorLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DocName,Email,Address,State,Country")] DoctorList doctorList)
        {
            if (id != doctorList.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorListExists(doctorList.ID))
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
            return View(doctorList);
        }

        // GET: DoctorLists/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorList = await _context.DoctorList
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctorList == null)
            {
                return NotFound();
            }

            return View(doctorList);
        }

        // POST: DoctorLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorList = await _context.DoctorList.FindAsync(id);
            _context.DoctorList.Remove(doctorList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorListExists(int id)
        {
            return _context.DoctorList.Any(e => e.ID == id);
        }
    }
}
