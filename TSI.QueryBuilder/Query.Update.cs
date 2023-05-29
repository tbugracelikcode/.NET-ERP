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
                if (i == 0)
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

                if (i == 0)
                {
                    valuesQuery = columns[i] + "=" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'";
                }
                else
                {
                    valuesQuery = valuesQuery + "," + columns[i] + "=" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'";
                }

            }

            updateQuery = updateQuery + valuesQuery;

            Sql = updateQuery;

            return this;
        }
    }
}
