using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Where(object constraints, bool IsActive, string op)
        {
            string where = "";

            var dictionary = new Dictionary<string, object>();

            if(constraints != null)
            {
                foreach (var item in constraints.GetType().GetRuntimeProperties())
                {
                    dictionary.Add(item.Name, item.GetValue(constraints));
                }

                int counter = 0;

                foreach (var dict in dictionary)
                {
                    if (counter == 0)
                    {
                        where = dict.Key + "=" + "'" + dict.Value + "'";
                        counter++;
                    }
                    else
                    {
                        where = where + " " + op + " " + dict.Key + "=" + "'" + dict.Value + "'";
                    }
                }
            }
           

            if (IsActive)
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where = where + " And" + " IsActive='1'";
                }
                else
                {
                    where = " IsActive='1'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where = where + " And" + " IsActive='0'";
                }
                else
                {
                    where = " IsActive='0'";
                }
            }

            WhereSentence = where;

            return this;
        }

        //public Query Where(object constraints, string op)
        //{
        //    string where = "";

        //    var dictionary = new Dictionary<string, object>();

        //    if (constraints != null)
        //    {
        //        foreach (var item in constraints.GetType().GetRuntimeProperties())
        //        {
        //            dictionary.Add(item.Name, item.GetValue(constraints));
        //        }

        //        int counter = 0;

        //        foreach (var dict in dictionary)
        //        {
        //            if (counter == 0)
        //            {
        //                where = dict.Key + "=" + "'" + dict.Value + "'";
        //                counter++;
        //            }
        //            else
        //            {
        //                where = where + " " + op + " " + dict.Key + "=" + "'" + dict.Value + "'";
        //            }
        //        }
        //    }

        //    WhereSentence = where;

        //    return this;
        //}

        public Query Where(string query)
        {
            WhereSentence = query;

            return this;
        }

        public Query Where<T>(Expression<Func<T, bool>> predicate, bool IsActive)
        {
            string where = "";

            //"((Id = 278d3684-a800-feeb-101f-3a06d571d66e) And (Name = 'Şube-1'))"

            if (predicate != null)
            {
                var visitor = new Visitor();

                var queryExpression = (Expression<Func<T, bool>>)visitor.Visit(predicate);
                //var queryExpression2 = visitor.Visit(predicate);

                //where = queryExpression.Body.ToString().Replace("t.", "")
                //         .Replace("AndAlso", "And")
                //         .Replace("OrElse", "Or")
                //         .Replace("\"", "'")
                //         .Replace("==", "=");
            }

            if (IsActive)
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where = where + " And" + " IsActive='1'";
                }
                else
                {
                    where = " IsActive='1'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where = where + " And" + " IsActive='0'";
                }
                else
                {
                    where = " IsActive='0'";
                }
            }

            WhereSentence = where;

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
                string where = column + op + " " + "'" + value.ToString() + "'";

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

                    string where = column + inSentence;

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

                    string where = column + inSentence;

                    WhereSentence = where;

                    //Sql = Sql + WhereSentence;
                }
            }
            return this;
        }

        public Query WhereContains(string column, string value)
        {
            string tableName = TableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                string containsSentence = column + " like '%" + value + "%'";

                string whereSentence = containsSentence;

                WhereSentence = whereSentence;

                //Sql = Sql + whereSentence;
            }

            return this;
        }

        public Query WhereStartingWith(string column, string value)
        {
            string tableName = TableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                string startingwithSentence = column + " like '" + value + "%'";

                string whereSentence = startingwithSentence;

                WhereSentence = whereSentence;
                //Sql = Sql + whereSentence;
            }

            return this;
        }

        public Query WhereEndingWith(string column, string value)
        {
            string tableName = TableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                string endingwithSentence = column + " like '%" + value + "'";

                string whereSentence = endingwithSentence;

                WhereSentence = whereSentence;

                //Sql = Sql + whereSentence;
            }

            return this;
        }

    }

    class Visitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var expression = Visit(memberExpression.Expression);

            if (expression is ConstantExpression)
            {
                object container = ((ConstantExpression)expression).Value;

                var member = memberExpression.Member;

                if (member is FieldInfo)
                {
                    string value = ((FieldInfo)member).GetValue(container).ToString();

                    return Expression.Constant(value);
                }

                if (member is PropertyInfo)
                {
                    object value = ((PropertyInfo)member).GetValue(container, null);
                    return Expression.Constant(value);
                }
            }
            return base.VisitMember(memberExpression);
        }
    }
}