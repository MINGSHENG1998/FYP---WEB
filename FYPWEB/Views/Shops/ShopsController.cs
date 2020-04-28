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

namespace FYPWEB.Views.Shops
{
    public class ShopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public ShopsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Shops
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shop.ToListAsync());
        }

        public async Task<IActionResult> Landing()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT COUNT (*) FROM [Points] WHERE [User] = '" + user.Email + "' ";
            cmd.Connection = con;
            con.Open();
            int rowamount = (int)cmd.ExecuteScalar();
            con.Close();
            //return Content("gg u again lmao" + rowamount);
            if (rowamount == 0)
            {
                cmd.CommandText = "Insert into [Points] values('" + user.Email + "','" + 0 + "')";
                cmd.Connection = con;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception es)
                {
                    throw es;
                }

                await _context.SaveChangesAsync();
            }
            else if (rowamount != 0)
            {
                cmd.CommandText = "SELECT [TotalPoint] FROM [Points] Where [User] = '" + user.Email + "'";
                cmd.Connection = con;
                con.Open();
                int dbpoint = (int)cmd.ExecuteScalar();
                con.Close();
                ViewBag.point = dbpoint;
            }
            ViewBag.Goal = TempData["Goal"].ToString();
            if (TempData["Code"] !=null)
            {
                ViewBag.Code = TempData["Code"].ToString();
            }
            
            return View(await _context.Shop.ToListAsync());
        }

        [Authorize(Roles = "Member, Admin")]
        public async Task<IActionResult> Redeem()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT COUNT (*) FROM [Points] WHERE [User] = '" + user.Email + "' ";
            cmd.Connection = con;
            con.Open();
            int rowamount = (int)cmd.ExecuteScalar();
            con.Close();
            //return Content("gg u again lmao" + rowamount);
            if (rowamount == 0)
            {
                cmd.CommandText = "Insert into [Points] values('" + user.Email + "','" + 0 + "')";
                cmd.Connection = con;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception es)
                {
                    throw es;
                }

                await _context.SaveChangesAsync();
            }
            else if (rowamount != 0)
            {
                cmd.CommandText = "SELECT [TotalPoint] FROM [Points] Where [User] = '" + user.Email + "'";
                cmd.Connection = con;
                con.Open();
                int dbpoint = (int)cmd.ExecuteScalar();
                con.Close();
                ViewBag.point = dbpoint;
            }


            return View(await _context.Shop.ToListAsync());
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Purchase(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
                cmd.CommandText = "SELECT [TotalPoint] FROM [Points] Where [User] = '" + user.Email + "'";
                cmd.Connection = con;
                con.Open();
                int dbpoint = (int)cmd.ExecuteScalar();
                con.Close();
                ViewBag.point = dbpoint;
            
            
            cmd.CommandText = "SELECT [TotalPoint] FROM [Points] Where [User] = '" + user.Email + "'";
            cmd.Connection = con;
            con.Open();
            int tpoint = (int)cmd.ExecuteScalar();
            con.Close();

            //return Content("gg u again lmao" + tpoint);
            cmd.CommandText = "SELECT [Cost] FROM [Shop] Where [ID] = '" + id + "'";
            cmd.Connection = con;
            con.Open();
            int spoint = (int)cmd.ExecuteScalar();
            con.Close();
            //return Content("gg u again lmao" + tpoint + "   " + spoint);

            int npoint = tpoint - spoint;
            if (npoint < 0)
            {
                string fail = "Transaction failed! Insufficient Credit!";
                //ViewBag.Goal = fail;
                TempData["goal"] = fail;
                return Redirect("/Shops/Landing");
            }
            else
            {
                cmd.CommandText = "UPDATE [Points] SET [TotalPoint] = ('" + npoint + "') WHERE [User] = '" + user.Email + "'";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[12];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                string done = "Transaction Completed! Here is your E-Voucher code: ";
                //ViewBag.Goal = done;
                //ViewBag.Code = finalString;
                TempData["Goal"] = done;
                TempData["Code"] = finalString;
                return Redirect("/Shops/Landing");
            }           

            

            //return Redirect("/Shops/Redeem");
            //return Content("gg u again lmao" + tpoint + "   " + spoint + "   " + npoint);
        }



        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductName,Info,Cost,Pic")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductName,Info,Cost,Pic")] Shop shop)
        {
            if (id != shop.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.ID))
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
            return View(shop);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shop.FindAsync(id);
            _context.Shop.Remove(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
            return _context.Shop.Any(e => e.ID == id);
        }
    }
}
