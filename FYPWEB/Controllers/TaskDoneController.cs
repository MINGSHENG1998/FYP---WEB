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

namespace FYPWEB.Controllers
{
    public class TaskDoneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-FYPWEB-A0E98CD9-B6EA-40DC-9FD9-731D33BEDE28;Trusted_Connection=True;MultipleActiveResultSets=true");
        SqlCommand cmd = new SqlCommand();

        public IActionResult Index()
        {
            return View();
        }
        public TaskDoneController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Event()
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
            DateTime dt = DateTime.Now;
            var theday = dt.DayOfWeek.ToString();
            cmd.CommandText = "DELETE FROM [TaskDone] WHERE [TaskDay] != '" + theday + "' ";
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Connection = con;
            con.Open();

            cmd.CommandText = "SELECT COUNT (User) FROM [TaskDone] WHERE [User] = '" + user.Email + "' ";
            int tu = (int)cmd.ExecuteScalar();

            if(tu > 0) { 

            cmd.CommandText = "Insert into [TaskDone] ([TaskID]) SELECT ([ID]) FROM [DailyTask] WHERE [ID] NOT IN (SELECT [TaskID] FROM [TaskDone]) AND [Day] = '" + theday + "'";
            
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [TaskDone] SET [TaskDay] = [Day] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [TaskDone] SET [TaskDifficulty] = [Difficulty] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [TaskDone] SET [TaskDone].[TaskName] = [DailyTask].[TaskName] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [TaskDone] SET [TaskPoint] = [Point] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE [TaskDone] SET [User] = ('" + user.Email + "') WHERE [User] IS NULL";
            cmd.ExecuteNonQuery();

            }
            
            else if (tu == 0)
            {
                cmd.CommandText = "Insert into [TaskDone] ([TaskID]) SELECT ([ID]) FROM [DailyTask] WHERE [Day] = '" + theday + "'";   
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE [TaskDone] SET [TaskDay] = [Day] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE [TaskDone] SET [TaskDifficulty] = [Difficulty] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE [TaskDone] SET [TaskDone].[TaskName] = [DailyTask].[TaskName] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE [TaskDone] SET [TaskPoint] = [Point] FROM [TaskDone] INNER JOIN [DailyTask] ON [TaskID] = [DailyTask].[ID]";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE [TaskDone] SET [User] = ('" + user.Email + "') WHERE [User] IS NULL";
                cmd.ExecuteNonQuery();
            }



            await _context.SaveChangesAsync();
            con.Close();

           
            return View(await _context.TaskDone.ToListAsync());
        }

        public async Task<IActionResult> AddPoint(int id)
        {
            int dnflag = 1;
            cmd.CommandText = "UPDATE [TaskDone] SET [Done] = ('" + dnflag + "') WHERE [TaskID] = '" + id + "'";
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            DateTime dt = DateTime.Now;
            var theday = dt.DayOfWeek.ToString();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cmd.CommandText = "SELECT [TotalPoint] FROM [Points] Where [User] = '" + user.Email + "'";
            cmd.Connection = con;
            con.Open();
            int tpoint = (int)cmd.ExecuteScalar();
            con.Close();
            
            cmd.CommandText = "SELECT [Point] FROM [DailyTask] Where [ID] = '" + id + "'";
            cmd.Connection = con;
            con.Open();
            int spoint = (int)cmd.ExecuteScalar();
            con.Close();

            int npoint = tpoint + spoint;
            cmd.CommandText = "UPDATE [Points] SET [TotalPoint] = ('" + npoint + "') WHERE [User] = '" + user.Email + "'";
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            
            cmd.CommandText = "SELECT [TaskID] FROM [TaskDone] Where [TaskID] = '" + id + "'";
            cmd.Connection = con;
            con.Open();
            int taskdoneid = (int)cmd.ExecuteScalar();
            con.Close();
            ViewBag.task = taskdoneid;

            return Redirect("/TaskDone/Event");
        }
    }
}