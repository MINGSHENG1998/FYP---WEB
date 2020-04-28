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

namespace FYPWEB.Views.Consultations
{
    public class ConsultationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public ConsultationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Consultations
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT [ID] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            cmd.Connection = con;
            con.Open();
            int doid = (int)cmd.ExecuteScalar();
            ViewBag.did = doid;

            //return Content("gg u again lmao " + doid);




            return View(await _context.Consultation.ToListAsync());
        }

        

        public async Task<IActionResult> AppointmentView(int id)
        {
            cmd.CommandText = "SELECT [ID] FROM [DoctorList] WHERE [ID] = '" + id + "' ";
            cmd.Connection = con;
            con.Open();
            int doid = (int)cmd.ExecuteScalar();
            ViewBag.did = doid;
            con.Close();

            return View(await _context.Consultation.ToListAsync());
        }

        // GET: Consultations/Details/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT [ID] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            cmd.Connection = con;
            con.Open();
            int doid = (int)cmd.ExecuteScalar();
            ViewBag.did = doid;

            //return Content("gg u again lmao " + doid);

            
            cmd.CommandText = "SELECT [DocName] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            string getName = cmd.ExecuteScalar().ToString();
            ViewBag.dname = getName;
            con.Close();

            //return Content("gg u again lmao " + getName);


            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DocID,DocName,Date,Time")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT [ID] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            cmd.Connection = con;
            con.Open();
            int doid = (int)cmd.ExecuteScalar();
            ViewBag.did = doid;

            //return Content("gg u again lmao " + doid);


            cmd.CommandText = "SELECT [DocName] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            string getName = cmd.ExecuteScalar().ToString();
            ViewBag.dname = getName;
            con.Close();

            //return Content("gg u again lmao " + getName);
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DocID,DocName,Date,Time")] Consultation consultation)
        {
            if (id != consultation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultation.ID))
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
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultation = await _context.Consultation.FindAsync(id);
            _context.Consultation.Remove(consultation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultation.Any(e => e.ID == id);
        }
    }
}
