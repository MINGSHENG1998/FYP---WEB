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

namespace FYPWEB.Views.Appointments
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: Appointments
        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appointment.ToListAsync());
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Indexdoc()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT [Email] FROM [DoctorList] WHERE [Email] = '" + user.Email + "' ";
            cmd.ExecuteNonQuery();
            string doid = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "SELECT [ID] FROM [DoctorList] WHERE [Email] = '" + doid + "' ";
            cmd.ExecuteNonQuery();
            int doid2 = (int)cmd.ExecuteScalar();
            ViewBag.did = doid2;
            con.Close();
            return View(await _context.Appointment.ToListAsync());
        }


        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Book(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "Insert into [Appointment] ([ConsultationID]) SELECT ([ID]) FROM [Consultation] WHERE [ID] NOT IN (SELECT [ConsultationID] FROM [Appointment]) AND [ID]  = '" + id + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [DocName] = [Consultation].[DocName] FROM [Appointment] INNER JOIN [Consultation] ON [ConsultationID] = [Consultation].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [DocID] = [Consultation].[DocID] FROM [Appointment] INNER JOIN [Consultation] ON [ConsultationID] = [Consultation].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [Date] = [Consultation].[Date] FROM [Appointment] INNER JOIN [Consultation] ON [ConsultationID] = [Consultation].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [Time] = [Consultation].[Time] FROM [Appointment] INNER JOIN [Consultation] ON [ConsultationID] = [Consultation].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [Appointment].[UserName] = ('" + user.Email + "') WHERE [ConsultationID]  = '" + id + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Appointment] SET [UserID] = [AspNetUsers].[ID] FROM [Appointment] INNER JOIN [AspNetUsers] ON [Appointment].[UserName] = [AspNetUsers].[UserName]";
            cmd.ExecuteNonQuery();            
            await _context.SaveChangesAsync();

            cmd.CommandText = "DELETE FROM [Consultation] WHERE [ID]  = '" + id + "'";
            cmd.ExecuteNonQuery();
            await _context.SaveChangesAsync();

            con.Close();
            return Redirect("/Appointments/Index");
            //return View(await _context.Appointment.ToListAsync());
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT [DocID] FROM [Appointment] WHERE [ConsultationID] = '" + id + "' ";
            int realid = (int)cmd.ExecuteScalar();
            cmd.CommandText = "Insert into [Consultation] ([Consultation].[DocID]) Values ('"+ realid +"')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Consultation] SET [DocName] = [Appointment].[DocName] FROM [Consultation] INNER JOIN [Appointment] ON [Consultation].[DocID] = [Appointment].[DocID] WHERE [Consultation].[DocName] IS NULL";
            cmd.ExecuteNonQuery();            
            cmd.CommandText = "UPDATE [Consultation] SET [Date] = [Appointment].[Date] FROM [Consultation] INNER JOIN [Appointment] ON [Consultation].[DocID] = [Appointment].[DocID] WHERE [Consultation].[Date] IS NULL";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [Consultation] SET [Time] = [Appointment].[Time] FROM [Consultation] INNER JOIN [Appointment] ON [Consultation].[DocID] = [Appointment].[DocID] WHERE [Consultation].[Time] IS NULL";
            cmd.ExecuteNonQuery();
            
            await _context.SaveChangesAsync();

            cmd.CommandText = "DELETE FROM [Appointment] WHERE [ConsultationID]  = '" + id + "'";
            cmd.ExecuteNonQuery();
            await _context.SaveChangesAsync();

            con.Close();
            return Redirect("/Appointments");
            //return View(await _context.Appointment.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        //// GET: Appointments/Create
        //public async Task<IActionResult> Create(string Type)
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);





        //    var Consultation = from m in _context.Consultation select m;

        //    IQueryable<string> TypeQuery = from m in _context.Consultation
        //                                   orderby m.DocName
        //                                   select m.DocName;
        //    IEnumerable<SelectListItem> items =
        //        new SelectList(await TypeQuery.Distinct().ToListAsync());

        //    ViewBag.Type = items;

        //    if (!string.IsNullOrEmpty(Type)) // if got any search word
        //    {
        //        Consultation = Consultation.Where(s => s.DocName == Type);

        //        ViewBag.docna = Consultation;

        //        return Content("gg u again lmao" + Consultation);
        //    }

        //    //return Content("gg u again lmao" + items);
        //    return View();
        //}

        //// POST: Appointments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,DocID,DocName,UserID,UserName,Date,Time")] Appointment appointment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(appointment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(appointment);
        //}

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DocID,DocName,UserID,UserName,Date,Time")] Appointment appointment)
        {
            if (id != appointment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.ID))
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
            return View(appointment);
        }




        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.ID == id);
        }
    }
}
