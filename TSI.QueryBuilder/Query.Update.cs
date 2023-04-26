using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Update(IEnumerable<string> columns, IEnumerable<object> values)
        {
            // update Employees set FirstName = 'Hüseyin', LastName='Özsüzer'

            var columnsList = columns.ToList();
            var valuesList = values.ToList();

            if (columnsList.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(columns)} null veya boş olamaz.");
            }

            if (valuesList.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(values)} null veya boş olamaz.");
            }

            if (columnsList.Count != valuesList.Count)
            {
                throw new InvalidOperationException("");
            }

            string valuesQuery = string.Empty;


            string updateQuery = "update " + TableName + " set ";

            for (int i = 0; i < columnsList.Count; i++)
            {
                if(i == 0)
                {
                    valuesQuery = columnsList[i] + "=" + "'" + valuesList[i] + "'";
                }
                else
                {
                    valuesQuery = valuesQuery + "," + columnsList[i] + "=" + "'" + valuesList[i] + "'";
                }
                
            }

            updateQuery = updateQuery + valuesQuery;

            Sql = updateQuery;

            return this;
        }

        public Query Update(object dto)
        {
            // update Employees set FirstName = 'Hüseyin', LastName='Özsüzer'

            var valuesList = dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList();

            string[] columns = new string[valuesList.Count];

            int counter = 0;

            foreach (PropertyInfo prop in dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList())
            {
                columns[counter] = prop.Name;
                counter++;
            }

            string valuesQuery = string.Empty;

            string updateQuery = "update " + TableName + " set ";

            for (int i = 0; i < valuesList.Count; i++)
            {
                TypeCode type = Type.GetTypeCode(valuesList[i].PropertyType);
                string nullValue = ValidateProperty(type);
                var value = valuesList[i].GetValue(dto,null);

                if(value.ToString() != nullValue)
                {
                    if (i == 0)
                    {
                        valuesQuery = columns[i] + "=" + "'" + valuesList[i].GetValue(dto, null) + "'";
                    }
                    else
                    {
                        valuesQuery = valuesQuery + "," + columns[i] + "=" + "'" + valuesList[i].GetValue(dto, null) + "'";
                    }
                }
            }

            updateQuery = updateQuery + valuesQuery;

            Sql = updateQuery;

            return this;
        }

        private string ValidateProperty(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.String:
                    return default(string);
                    break;

                case TypeCode.DateTime:
                    return default(DateTime).ToString();
                    break;

                case TypeCode.Object:
                    return default(object).ToString();
                    break;

                case TypeCode.SByte:
                    return default(sbyte).ToString();
                    break;

                case TypeCode.Boolean:
                    return default(bool).ToString();
                    break;

                case TypeCode.Byte:
                    return default(byte).ToString();
                    break;

                case TypeCode.Decimal:
                    return default(decimal).ToString();
                    break;

                case TypeCode.Double:
                    return default(double).ToString();
                    break;
                default:
                    return "";
                    break;
            }
        }
    }
}
