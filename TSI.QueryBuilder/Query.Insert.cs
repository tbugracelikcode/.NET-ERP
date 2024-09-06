using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.MappingAttributes;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Insert(IEnumerable<string> columns, IEnumerable<object> values)
        {
            // insert into Employees (FirstName,LastName) values ('Hüseyin','Özsüzer')

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

            string columnsQuery = string.Empty;

            string valuesQuery = string.Empty;


            string insertQuery = "insert into " + TableName;

            for (int i = 0; i < columnsList.Count; i++)
            {
                if (columnsList.Count == 1)
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columnsList[i] + ")";
                        valuesQuery = " (" + "'" + valuesList[i] + "'" + ")";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columnsList[i] + ",";
                        valuesQuery = " (" + "'" + valuesList[i] + "'" + ",";
                    }
                    else
                    {
                        columnsQuery = columnsQuery + columnsList[i] + ",";
                        valuesQuery = valuesQuery + "'" + valuesList[i] + "'" + ",";
                    }
                }
            }

            columnsQuery = columnsQuery.Substring(0, columnsQuery.Length - 1);
            valuesQuery = valuesQuery.Substring(0, valuesQuery.Length - 1);

            insertQuery = insertQuery + columnsQuery + ")" + " values " + valuesQuery + ")";

            Sql = insertQuery;

            return this;
        }

        public Query Insert(object dto)
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

            string columnsQuery = string.Empty;

            string insertQuery = "insert into " + TableName;

            string parameterValues = "";

            for (int i = 0; i < valuesList.Count; i++)
            {
                string parameterName = "@P" + i;

                if (valuesList.Count == 1)
                {
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


                        columnsQuery = " (" + columns[i] + ")";
                        //valuesQuery = " (" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ")";

                        valuesQuery = columns[i] + "=" + parameterName;
                        parameterValues = parameterName + "=" + value;
                    }
                }
                else
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

                    if (i == 0)
                    {
                        columnsQuery = " (" + columns[i] + ",";
                        valuesQuery = parameterName;
                        parameterValues = parameterName + "=" + value;
                        //valuesQuery = " (" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ",";
                    }
                    else
                    {
                        columnsQuery = columnsQuery + columns[i] + ",";
                        valuesQuery = valuesQuery + "," + parameterName;
                        parameterValues = parameterValues + "," + parameterName + "=" + value;
                        //valuesQuery = valuesQuery + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ",";

                    }
                }

            }

            //valuesQuery = valuesQuery.Substring(0, valuesQuery.Length - 1);
            valuesQuery = valuesQuery + ")" + QueryConstants.QueryParamsConstant + parameterValues;
            columnsQuery = columnsQuery.Substring(0, columnsQuery.Length - 1);

            insertQuery = insertQuery + columnsQuery + ")" + " values " + "(" + valuesQuery;

            Sql = insertQuery;

            return this;
        }
    }
}
