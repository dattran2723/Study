using Data.Seed;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Configurations;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(AppSettings.ConnectString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new DbInitializer());
        }
    }
}
