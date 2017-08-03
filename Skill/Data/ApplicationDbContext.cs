using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skill.Models;

namespace Skill.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<PersonMasterLabel>().HasKey(t => new { t.PersonID, t.LabelID });
            builder.Entity<ProjectPartakePerson>().HasKey(t => new { t.ProjectID, t.PersonID });
            builder.Entity<ProjectUseLabel>().HasKey(t => new { t.ProjectID, t.LabelID });
        }
        public DbSet<Label> Lable { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<Person> Person { get; set; }

        public DbSet<PersonMasterLabel> PersonMasterLabel { get; set; }
        
    }
}
