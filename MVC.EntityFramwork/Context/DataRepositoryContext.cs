using System.Data.Entity;

namespace MVC.EntityFramwork.Context
{
    public class DataRepositoryContext : DbContext
    {
        public DataRepositoryContext()
            : base("ConnectionString")
        { }
    }
}
