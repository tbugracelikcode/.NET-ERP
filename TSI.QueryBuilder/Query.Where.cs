using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Where<T>(Expression<Func<T, bool>> predicate)
        {
            string where = predicate.Body.ToString();

            //var paramName = predicate.Parameters[0].Name;
            //var paramTypeName = predicate.Parameters[0].Type.Name;
            where = " where " + where.Replace("t.", "")
                         .Replace("AndAlso", "And")
                         .Replace("OrElse", "Or")
                         .Replace("\"", "'")
                         .Replace("==", "=");

            WhereSentence = where;

            //Sql = Sql + WhereSentence;

            return this;
        }

        public Query Where(string column, object value)
        {
            return Where(column, "=", value);
        }

        public Query Where(string column, string op, object value)
        {
            //Method = "select";

            string tableName = TableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                string where = " where " + column + op + " " + "'" + value.ToString() + "'";

                WhereSentence = where;

                //Sql = Sql + WhereSentence;
            }

            return this;
        }

        public Query WhereIn(string column, params string[] values)
        {
            if (values.Length > 0)
            {
                string tableName = TableName;

                if (!string.IsNullOrEmpty(tableName))
                {
                    string inSentence = "";

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i == 0)
                        {
                            inSentence = "'" + values[i].ToString() + "'";
                        }
                        else
                        {
                            inSentence = inSentence + " , " + "'" + values[i].ToString() + "'";
                        }
                    }

                    inSentence = " in " + "(" + inSentence + ")";

                    string where = " where " + column + inSentence;

                    WhereSentence = where;

                    //Sql = Sql + WhereSentence;
                }
            }
            return this;
        }

        public Query WhereNotIn(string column, params string[] values)
        {
            if (values.Length > 0)
            {
                string tableName = TableName;

                if (!string.IsNullOrEmpty(tableName))
                {
                    string inSentence = "";

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i == 0)
                        {
                            inSentence = "'" + values[i].ToString() + "'";
                        }
                        else
                        {
                            inSentence = inSentence + " , " + "'" + values[i].ToString() + "'";
                        }
                    }

                    inSentence = " not in " + "(" + inSentence + ")";

                    string where = " where " + column + inSentence;

                    WhereSentence = where;

                    //Sql = Sql + WhereSentence;
                }
            }
            return this;
        }

        public Query WhereContains(string column, string value)
        {
            string tableName = TableName;

            if(!string.IsNullOrEmpty(tableName))
            {
                string containsSentence =  column + " like '%" + value + "%'";

                string whereSentence = " where " + containsSentence;

                WhereSentence = whereSentence;

                //Sql = Sql + whereSentence;
            }

            return this;
        }

        public Query WhereStartingWith(string column, string value)
        {
            string tableName = TableName;

            if(!string.IsNullOrEmpty(tableName))
            {
                string startingwithSentence = column + " like '" + value + "%'";

                string whereSentence = " where " + startingwithSentence;

                WhereSentence = whereSentence;
                //Sql = Sql + whereSentence;
            }

            return this;
        }

        public Query WhereEndingWith(string column, string value)
        {
            string tableName = TableName;

            if(!string.IsNullOrEmpty(tableName))
            {
                string endingwithSentence = column + " like '%" + value + "'";

                string whereSentence = " where " + endingwithSentence;

                WhereSentence = whereSentence;

                //Sql = Sql + whereSentence;
            }

            return this;
        }
    }
}