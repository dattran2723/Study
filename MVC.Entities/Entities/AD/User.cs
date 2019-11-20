using MVC.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Entities.Entities.AD
{
    [Table("ADUsers")]
    public class User : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ADUserID")]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
