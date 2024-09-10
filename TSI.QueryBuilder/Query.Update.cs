using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using TSI.QueryBuilder.Constants.Join;
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

            var valuesList = dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList();

            #region Update Type'ına Göre Field Belirleme
            var dtoEntityProperties = valuesList.Where(t => t.DeclaringType.Name == dto.GetType().Name).ToList();

            var auditedEntityProperties = valuesList.Where(t => t.DeclaringType.Name == "FullAuditedEntityDto").ToList();

            var fullEntityProperties = valuesList.Where(t => t.DeclaringType.Name == "FullEntityDto").ToList();

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
            #endregion

            string[] columns = new string[valuesList.Count];

            int counter = 0;

            foreach (PropertyInfo prop in valuesList)
            {
                columns[counter] = prop.Name;
                counter++;
            }

            string valuesQuery = string.Empty;

            string updateQuery = "update " + TableName + " set ";

            string parameterValues = "";

            for (int i = 0; i < valuesList.Count; i++)
            {
                
                string parameterName = "@P" + i;
                
                if (i == 0)
                {
                    object value = null;

                    if (valuesList[i].PropertyType == typeof(Nullable<DateTime>) || valuesList[i].PropertyType == typeof(DateTime))
                    {
                        var date = Convert.ToDateTime(valuesList[i].GetValue(dto, null));

                        if (date == null)
                        {
                            value = new DateTime(1900, 1, 1) + "*dym*";
                        }
                        else if (date.Year == 1)
                        {
                            value = new DateTime(1900, 1, 1) + "*dym*";
                        }
                        else
                        {
                            value = date + "*dym*";
                        }
                    }
                    else if (valuesList[i].PropertyType == typeof(Nullable<Guid>))
                    {
                        var guidValue = valuesList[i].GetValue(dto, null);

                        if (guidValue == null)
                        {
                            value = Guid.Empty;
                        }
                        else
                        {
                            value = guidValue;
                        }
                    }
                    else if (valuesList[i].PropertyType == typeof(Decimal))
                    {
                        value = Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".");
                    }
                    else
                    {
                        value = valuesList[i].GetValue(dto, null);
                    }

                    //object value = (valuesList[i].PropertyType == typeof(decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null));

                    //valuesQuery = columns[i] + "=" + "'" + (valuesList[i].PropertyType == typeof(decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'";


                    valuesQuery = columns[i] + "=" + parameterName;
                    parameterValues = parameterName + "=" + value;

                }
                else
                {
                    //object value = (valuesList[i].PropertyType == typeof(decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null));

                    object value = null;

                    if (valuesList[i].PropertyType == typeof(Nullable<DateTime>) || valuesList[i].PropertyType == typeof(DateTime))
                    {
                        var date = Convert.ToDateTime(valuesList[i].GetValue(dto, null));

                        if (date == null)
                        {
                            value = new DateTime(1900, 1, 1) + "*dym*";
                        }
                        else if (date.Year == 1)
                        {
                            value = new DateTime(1900, 1, 1) + "*dym*";
                        }
                        else
                        {
                            value = date + "*dym*";
                        }
                    }
                    else if (valuesList[i].PropertyType == typeof(Nullable<Guid>))
                    {
                        var guidValue = valuesList[i].GetValue(dto, null);

                        if (guidValue == null)
                        {
                            value = Guid.Empty;
                        }
                        else
                        {
                            value = guidValue;
                        }
                    }
                    else if (valuesList[i].PropertyType == typeof(Decimal))
                    {
                        value = Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".");
                    }
                    else
                    {
                        value = valuesList[i].GetValue(dto, null);
                    }

                    valuesQuery = valuesQuery+","+ columns[i] + "=" + parameterName;
                    parameterValues = parameterValues + "," + parameterName + "=" + value;

                    //valuesQuery = valuesQuery + "," + columns[i] + "=" + "'" + (valuesList[i].PropertyType == typeof(decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'";
                }

            }

            valuesQuery = valuesQuery + QueryConstants.QueryParamsConstant + parameterValues;

            updateQuery = updateQuery + valuesQuery;


            Sql = updateQuery;


            return this;
        }
    }
}
