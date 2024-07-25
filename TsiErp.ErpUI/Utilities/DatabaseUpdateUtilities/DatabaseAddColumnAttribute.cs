using System;
using System.Collections.Generic;
using System.Text;

namespace TsiErp.ErpUI.Utilities.DatabaseUpdateUtilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DatabaseAddColumnAttribute : Attribute
    {
        public SqlDataType SqlDbType { get; set; }

        public bool Nullable { get; set; } = false;

        public int MaxLength { get; set; } = 0;

        public int Scale { get; set; } = 0;

        public int Precision { get; set; } = 0;

        public bool IsPrimaryKey { get; set; } = false;

        public string TableName { get; set; }
    }

    public enum SqlDataType
    {
        BigInt = 1,
        Binary = 2,
        Bit = 3,
        Char = 4,
        DateTime = 6,
        Decimal = 7,
        Float = 8,
        Image = 9,
        Int = 10,
        Money = 11,
        NChar = 12,
        NText = 13,
        NVarChar = 14,
        NVarCharMax = 0xF,
        Real = 0x10,
        SmallDateTime = 17,
        SmallInt = 18,
        SmallMoney = 19,
        Text = 20,
        Timestamp = 21,
        TinyInt = 22,
        UniqueIdentifier = 23,
        VarBinary = 28,
        VarBinaryMax = 29,
        VarChar = 30,
        VarCharMax = 0x1F,
        Variant = 0x20,
        Numeric = 35,
        Date = 36,
        Time = 37,
        DateTimeOffset = 38,
        DateTime2 = 39
    }
}
