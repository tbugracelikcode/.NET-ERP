using System;
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
    }
}
