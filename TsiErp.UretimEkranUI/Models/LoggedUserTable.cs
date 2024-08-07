using SQLite;

namespace TsiErp.UretimEkranUI.Models
{
    [Table("LoggedUserTable")]
    public class LoggedUserTable
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }


        [Column("UserName")]
        public string UserName { get; set; }
    }
}
