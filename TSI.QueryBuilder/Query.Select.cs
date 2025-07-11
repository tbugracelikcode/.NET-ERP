﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Select(params string[] columns)
        {
            //Method = "select";

            string queryColumns = "";

            for (int i = 0; i < columns.Length; i++)
            {
                if (i == 0)
                {
                    queryColumns = columns[i];
                }
                else
                {
                    queryColumns = queryColumns + "," + columns[i];
                }
            }

            if (queryColumns.Length > 0)
            {
                if (string.IsNullOrEmpty(Columns))
                {
                    Columns = queryColumns;
                }
                else
                {
                    Columns = Columns + "," + queryColumns;
                }
            }

            Sql = "select" + " * " + "from " + TableName + " as " + TableName;

            var insertPoint = Sql.IndexOf('*');

            Sql = Sql.Insert(insertPoint, queryColumns).Remove(insertPoint + queryColumns.Length, 1);

            return this;
        }

        public Query Select<T1>(Expression<Func<T1, object>> t1Expression)
        {
            string query = "";

            string t1 = typeof(T1).Name;

            List<string> columnList = new List<string>();

            #region T1 Expression
            if(t1Expression != null)
            {
                var t1Visitor = new PropertyVisitor();
                t1Visitor.Visit(t1Expression.Body);
                var t1Members = t1Visitor.Path;

                var t1newExpression = (NewExpression)t1Expression.Body;

                var t1AliasList = t1newExpression.Type.GetProperties().Zip(t1newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

                foreach (var item in t1Members)
                {
                    bool aliasControl = t1AliasList.Any(t => t.RealName == item.Name);

                    if (aliasControl)
                    {
                        columnList.Add(t1 + "." + item.Name + " as " + t1AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                    }
                    else
                    {
                        columnList.Add(t1 + "." + item.Name);
                    }
                }
            }
            else
            {
                var properties = typeof(T1).GetProperties();

                foreach (var item in properties)
                {
                    if (!columnList.Contains(item.Name))
                    {
                        columnList.Add(t1 + "." + item.Name);
                    }
                }
            }
            
            #endregion

            #region Create Columns String
            string columns = "";

            if (columnList.Count == 1)
            {
                columns = columnList[0];
            }
            else
            {
                for (int i = 0; i < columnList.Count; i++)
                {
                    if (i < columnList.Count - 1)
                    {
                        columns = columns + columnList[i] + ",";
                    }
                    else
                    {
                        columns = columns + columnList[i];
                    }
                }
            }
            #endregion

            if (string.IsNullOrEmpty(Columns))
            {
                Columns = columns;
            }
            else
            {
                Columns = Columns + "," + columns;
            }

            query = "select " + Columns + " from " + TableName + " as " + TableName;

            Sql = query;

            return this;
        }


        public Query Select()
        {
            //Method = "select";

            Sql = "select" + " * " + "from " + TableName + " as " + TableName;

            return this;
        }


        public Query Select<T1, T2>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> sumColumnExpression, string sumTable,  bool IsNullChechk, string sumQueryWhere)
        {
            string query = "";

            string t1 = typeof(T1).Name;

            List<string> columnList = new List<string>();

            if (t1Expression != null)
            {
                #region T1 Expression
                var t1Visitor = new PropertyVisitor();
                t1Visitor.Visit(t1Expression.Body);
                var t1Members = t1Visitor.Path;

                var t1newExpression = (NewExpression)t1Expression.Body;

                var t1AliasList = t1newExpression.Type.GetProperties().Zip(t1newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

                foreach (var item in t1Members)
                {
                    bool aliasControl = t1AliasList.Any(t => t.RealName == item.Name);

                    if (aliasControl)
                    {
                        columnList.Add(t1 + "." + item.Name + " as " + t1AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                    }
                    else
                    {
                        columnList.Add(t1 + "." + item.Name);
                    }
                }
                #endregion
            }
            else
            {
                var properties = typeof(T1).GetProperties();

                foreach (var item in properties)
                {
                    if (!columnList.Contains(item.Name))
                    {
                        columnList.Add(t1 + "." + item.Name);
                    }
                }
            }


            #region Sum Column Expression

            List<string> sumColumnNames = new List<string>();

            var sumColumnVisitor = new PropertyVisitor();
            sumColumnVisitor.Visit(sumColumnExpression.Body);
            var sumColumnMembers = sumColumnVisitor.Path;

            if(sumColumnMembers.Count==1)
            {
                sumColumnNames.Add(sumColumnMembers[0].Name);
            }

            if (sumColumnMembers.Count > 1)
            {

                foreach (var item in sumColumnMembers)
                {
                    sumColumnNames.Add(item.Name);
                }
                
            }


            #endregion

            #region Create Columns String
            string columns = "";

            if (columnList.Count == 1)
            {
                columns = columnList[0];
            }
            else
            {
                for (int i = 0; i < columnList.Count; i++)
                {
                    if (i < columnList.Count - 1)
                    {
                        columns = columns + columnList[i] + ",";
                    }
                    else
                    {
                        columns = columns + columnList[i];
                    }
                }
            }
            #endregion

            if (string.IsNullOrEmpty(Columns))
            {
                Columns = columns;
            }
            else
            {
                Columns = Columns + "," + columns;
            }

            if (sumColumnNames.Count==1)
            {
                string sumQuery = "";

                if (IsNullChechk)
                {
                    sumQuery = "ISNULL((SELECT SUM(" + sumColumnNames[0] + ") FROM " + sumTable + " where " + sumQueryWhere + "),0)" + " as " + sumColumnNames[0];
                }
                else
                {
                    sumQuery = "(SELECT SUM(" + sumColumnNames[0] + ") FROM " + sumTable + " where " + sumQueryWhere + ")" + " as " + sumColumnNames[0];
                }

                Columns = Columns + "," + sumQuery;
            }

            if (sumColumnNames.Count > 1)
            {
                string sumQuery = "";

                for (int i = 0; i < sumColumnNames.Count; i++)
                {
                    if(i==0)
                    {
                        if (IsNullChechk)
                        {
                            sumQuery = "ISNULL((SELECT SUM(" + sumColumnNames[i] + ") FROM " + sumTable + " where " + sumQueryWhere + "),0)" + " as " + sumColumnNames[i];
                        }
                        else
                        {
                            sumQuery = "(SELECT SUM(" + sumColumnNames[i] + ") FROM " + sumTable + " where " + sumQueryWhere + ")" + " as " + sumColumnNames[i];
                        }
                    }
                    else
                    {
                        if (IsNullChechk)
                        {
                            sumQuery = sumQuery + ", " + "ISNULL((SELECT SUM(" + sumColumnNames[i] + ") FROM " + sumTable + " where " + sumQueryWhere + "),0)" + " as " + sumColumnNames[i];
                        }
                        else
                        {
                            sumQuery = sumQuery + ", " + "(SELECT SUM(" + sumColumnNames[i] + ") FROM " + sumTable + " where " + sumQueryWhere + ")" + " as " + sumColumnNames[i];
                        }
                    }
                }           

                Columns = Columns + "," + sumQuery;
            }

            query = "select " + Columns + " from " + TableName + " as " + TableName;

            Sql = query;

            return this;
        }
    }
}
