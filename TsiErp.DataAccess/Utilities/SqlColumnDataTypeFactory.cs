using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.DataAccess.Utilities
{
    public class SqlColumnDataTypeFactory
    {
        public static DataType ConvertToDataType(SqlDataType sqlDataType,int maxLength,int precision,int scale)
        {
            switch (sqlDataType)
            {
                case SqlDataType.BigInt:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.BigInt);
                    
                case SqlDataType.Binary:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Binary,maxLength);

                case SqlDataType.Bit:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Bit);

                case SqlDataType.Char:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Char, maxLength);

                case SqlDataType.DateTime:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.DateTime);

                case SqlDataType.Decimal:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Decimal,precision,scale);

                case SqlDataType.Float:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Float);

                case SqlDataType.Image:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Image);

                case SqlDataType.Int:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Int);

                case SqlDataType.Money:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Money);

                case SqlDataType.NChar:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.NChar, maxLength);

                case SqlDataType.NText:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.NText);

                case SqlDataType.NVarChar:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.NVarChar, maxLength);

                case SqlDataType.NVarCharMax:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.NVarCharMax);

                case SqlDataType.Real:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Real);

                case SqlDataType.SmallDateTime:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.SmallDateTime);

                case SqlDataType.SmallInt:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.SmallInt);

                case SqlDataType.SmallMoney:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.SmallMoney);

                case SqlDataType.Text:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Text);

                case SqlDataType.Timestamp:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Timestamp);

                case SqlDataType.TinyInt:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.TinyInt);

                case SqlDataType.UniqueIdentifier:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.UniqueIdentifier);

                case SqlDataType.VarBinary:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.VarBinary, maxLength);

                case SqlDataType.VarBinaryMax:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.VarBinaryMax);

                case SqlDataType.VarChar:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.VarChar, maxLength);

                case SqlDataType.VarCharMax:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.VarCharMax);

                case SqlDataType.Variant:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Variant);

                case SqlDataType.Numeric:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Numeric, precision, scale);

                case SqlDataType.Date:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Date);

                case SqlDataType.Time:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.Time, scale);

                case SqlDataType.DateTimeOffset:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.DateTimeOffset, scale);

                case SqlDataType.DateTime2:
                    return new DataType(Microsoft.SqlServer.Management.Smo.SqlDataType.DateTime2, scale);

                default:
                    break;
            }

            return new DataType();
        }
    }
}
