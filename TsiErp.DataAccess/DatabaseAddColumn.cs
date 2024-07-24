
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.DataAccess.Utilities;

namespace TsiErp.DataAccess
{
    public class DatabaseAddColumn
    {
        #region SQL Data Type'lara göre kullanım örnekleri

        #region INT
        //[DatabaseAddColumn(Nullable = true, SqlDbType = SqlDataType.Int, TableName = "CurrentAccountCards",Default_ ="(0)")]
        //public int Deneme { get; set; } 
        #endregion

        #region DECIMAL
        //[DatabaseAddColumn(Nullable = true, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18, TableName = "CurrentAccountCards", Default_ = "(0.0)")]
        //public decimal Deneme2 { get; set; } 
        #endregion

        #region STRING
        //[DatabaseAddColumn(MaxLength = 17, Nullable = true, SqlDbType = SqlDataType.NVarChar, TableName = "CurrentAccountCards", Default_ = "NULL")]
        //public string Deneme3 { get; set; } 
        #endregion

        #region GUID
        //[DatabaseAddColumn(IsPrimaryKey = false, Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier, TableName = "Cities", Default_ = "('00000000-0000-0000-0000-000000000000')")]
        //public Guid Deneme4 { get; set; } 
        #endregion

        #region BOOL
        //[DatabaseAddColumn(Nullable = true, SqlDbType = SqlDataType.Bit, TableName = "Cities", Default_ = "(0)")]
        //public bool Deneme5 { get; set; }  
        #endregion
        #endregion
    }
}
