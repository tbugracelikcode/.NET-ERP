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

        [Column("UserID")]
        public Guid UserID { get; set; }

        [Column("IsAuthorizedUser")]
        public bool IsAuthorizedUser { get; set; }
    }
}
