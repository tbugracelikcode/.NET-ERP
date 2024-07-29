using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TSI.QueryBuilder.Helpers;
using TSI.QueryBuilder.MappingAttributes;
using TSI.QueryBuilder.Models;

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
            QuerySQL querySQL = new QuerySQL();

            var valuesList = dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList();

            string[] columns = new string[valuesList.Count];

            int counter = 0;

            foreach (PropertyInfo prop in dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList())
            {
                columns[counter] = prop.Name;
                counter++;
            }

            //string valuesQuery = string.Empty;

            string columnsQuery = string.Empty;

            string QueryParametersStr = string.Empty;

            string insertQuery = "insert into " + TableName;

            querySQL.ParameterList.Clear();

            for (int i = 0; i < valuesList.Count; i++)
            {
                if (valuesList.Count == 1)
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columns[i] + ")";
                        QueryParametersStr = " (" + "@" + columns[i] + ")";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add("@" + columns[i], value);
                        //valuesQuery = " (" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ")";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columns[i] + ",";
                        QueryParametersStr = " (" + "@" + columns[i] + ",";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add("@" + columns[i], value);
                        //valuesQuery = " (" + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ",";
                    }
                    else
                    {
                        columnsQuery = columnsQuery + columns[i] + ",";
                        QueryParametersStr = QueryParametersStr + "@" + columns[i] + ",";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add("@" + columns[i], value);
                        //valuesQuery = valuesQuery + "'" + (valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null)) + "'" + ",";

                    }
                }
            }

            //valuesQuery = valuesQuery.Substring(0, valuesQuery.Length - 1);
            columnsQuery = columnsQuery.Substring(0, columnsQuery.Length - 1);

            QueryParametersStr = QueryParametersStr.Substring(0, QueryParametersStr.Length - 1);

            //insertQuery = insertQuery + columnsQuery + ")" + " values " + valuesQuery + ")";

            QueryParametersStr = QueryParametersStr + ")";

            insertQuery = insertQuery + columnsQuery + ")" + " values " + QueryParametersStr;

            querySQL.Sql = insertQuery;

            InsertHelper.InsertQueryList(querySQL);

            Sql = querySQL.Sql;

            return this;
        }

        public Query Insert(object dto, object parameters)
        {

            QuerySQL querySQL = new QuerySQL();

            var valuesList = dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList();

            string[] columns = new string[valuesList.Count];

            int counter = 0;

            foreach (PropertyInfo prop in dto.GetType().GetProperties().Where(t => t.CustomAttributes.Count() == 0).ToList())
            {
                columns[counter] = prop.Name;
                counter++;
            }

            string columnsQuery = string.Empty;

            string QueryParametersStr = string.Empty;

            string insertQuery = "insert into " + TableName;


            for (int i = 0; i < valuesList.Count; i++)
            {
                if (valuesList.Count == 1)
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columns[i] + ")";
                        QueryParametersStr = " (" + "@" + columns[i] + ")";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add(" (" + "@" + columns[i] + ")", value);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        columnsQuery = " (" + columns[i] + ",";
                        QueryParametersStr = " (" + "@" + columns[i] + ",";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add(" (" + "@" + columns[i] + ")", value);
                    }
                    else
                    {
                        columnsQuery = columnsQuery + columns[i] + ",";
                        QueryParametersStr = QueryParametersStr + "@" + columns[i] + ",";
                        object value = valuesList[i].PropertyType == typeof(Decimal) ? Convert.ToString(valuesList[i].GetValue(dto, null)).Replace(",", ".") : valuesList[i].GetValue(dto, null);

                        querySQL.ParameterList.Add(" (" + "@" + columns[i] + ")", value);
                    }
                }
            }

            columnsQuery = columnsQuery.Substring(0, columnsQuery.Length - 1);
            QueryParametersStr = QueryParametersStr + ")";

            insertQuery = insertQuery + columnsQuery + ")" + " values " + QueryParametersStr;

            querySQL.Sql = insertQuery;

            InsertHelper.InsertQueryList(querySQL);

            var a = InsertHelper.InsertQueris;

            Sql = insertQuery;

            return this;
        }
    }
}
