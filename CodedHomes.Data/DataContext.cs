using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodedHomes.Models;
using System.Data.Entity;
using System.Data;
using System.Configuration;
//using CodedHomes.Data.Configuration;


namespace CodedHomes.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Home> Homes { get; set; }
        public DbSet<User> Users { get; set; }

        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }
        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"] != null)
                {
                    return ConfigurationManager.AppSettings["ConnectionStringName"].ToString();
                }

                return "DefaultConnection";
            }
        }
        public DataContext()
            : base(nameOrConnectionString: DataContext.ConnectionStringName) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
        }

        private void ApplyRules()
        {

            foreach (var entry in this.ChangeTracker.Entries()
                        .Where(
                             e => e.Entity is IAuditInfo &&
                            (e.State == EntityState.Added) ||
                            (e.State == EntityState.Modified)))
            {
                IAuditInfo e = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    e.CreatedOn = DateTime.Now;
                }

                e.ModifiedOn = DateTime.Now;
            }
        }

        public override int SaveChanges()
        {
            this.ApplyRules();

            return base.SaveChanges();
        }





    }
}
