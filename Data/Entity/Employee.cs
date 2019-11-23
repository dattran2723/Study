using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("Employees")]
    public class Employee
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }
    }
}
