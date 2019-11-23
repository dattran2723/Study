using System.Data.Entity;

namespace Data.Seed
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {

        public DbInitializer()
        {
        }

        protected override void Seed(AppDbContext context)
        {
        }
    }
}
