using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FYPWEB.Models;

namespace FYPWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FYPWEB.Models.DailyTask> DailyTask { get; set; }
        public DbSet<FYPWEB.Models.Points> Points { get; set; }
        public DbSet<FYPWEB.Models.TaskDone> TaskDone { get; set; }
        public DbSet<FYPWEB.Models.DoctorList> DoctorList { get; set; }
        public DbSet<FYPWEB.Models.Consultation> Consultation { get; set; }
        public DbSet<FYPWEB.Models.Appointment> Appointment { get; set; }
        public DbSet<FYPWEB.Models.Shop> Shop { get; set; }

        //public DbSet<FYPWEB.Models.TaskDone> TaskDone { get; set; }
    }
}
