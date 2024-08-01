using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TSI.QueryBuilder.Helpers;
using TSI.QueryBuilder.Models;

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

        public Query Update(object dto, UpdateType updateType = UpdateType.Update)
        {
            UpdateQuerySQL querySQL = new UpdateQuerySQL();

            var valuesList = dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList();

            var dtoEntityProperties = valuesList.Where(t => t.DeclaringType.Name == dto.GetType().Name).ToList();

            var auditedEntityProperties = valuesList.Where(t => t.DeclaringType.Name == "FullAuditedEntityDto").ToList();

            //var fullEntityProperties = valuesList.Where(t => t.DeclaringType.Name == "FullEntityDto").ToList();

            if (auditedEntityProperties.Count > 0)
            {
                if (updateType == UpdateType.Update)
                {
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "CreatorId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "CreationTime"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DeleterId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DeletionTime"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "IsDeleted"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DataOpenStatus"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DataOpenStatusUserId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "Id"));
                }

                if (updateType == UpdateType.ConcurrencyUpdate)
                {
                    foreach (var item in dtoEntityProperties)
                    {
                        valuesList.Remove(item);
                    }

                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "CreatorId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "CreationTime"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DeleterId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "DeletionTime"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "IsDeleted"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "Id"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "LastModifierId"));
                    valuesList.Remove(valuesList.FirstOrDefault(t => t.Name == "LastModificationTime"));
                }
            }

            string[] columns = new string[valuesList.Count];

            int counter = 0;

            foreach (PropertyInfo prop in valuesList)
            {
                columns[counter] = prop.Name;
                counter++;
            }

            string valuesQuery = string.Empty;

            string updateQuery = "update " + TableName + " set ";

            querySQL.ParameterList.Clear();

            for (int i = 0; i < valuesList.Count; i++)
            {
                if (i == 0)
                {
                    object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                    querySQL.ParameterList.Add("@" + columns[i], value);

                    valuesQuery = columns[i] + "=" + "@" + columns[i];
                }
                else
                {

                    object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                    querySQL.ParameterList.Add("@" + columns[i], value);


                    valuesQuery = valuesQuery + "," + columns[i] + "=" + "@" + columns[i];
                }
            }



            updateQuery = updateQuery + valuesQuery;

            querySQL.Sql = updateQuery;

            UpdateHelper.UpdateQueryList(querySQL);

            Sql = querySQL.Sql;

            return this;
        }
    }
}
