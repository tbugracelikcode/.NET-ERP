using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Join<T1>(Expression<Func<T1, object>> t1Expression, string leftTableColumnExpression, string rightTableColumnExpression, string joinType)
        {
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

            if (string.IsNullOrEmpty(TablesJoinKeywords))
            {
                TablesJoinKeywords = " " + joinType + " join " + t1 + " as " + t1 + " on " + TableName + "." + leftTableColumnExpression + "=" + t1 + "." + rightTableColumnExpression;
            }
            else
            {
                TablesJoinKeywords = TablesJoinKeywords + " " + joinType + " join " + t1 + " as " + t1 + " on " + TableName + "." + leftTableColumnExpression + "=" + t1 + "." + rightTableColumnExpression;
            }

            return this;
        }


        public Query Join<T1>(Expression<Func<T1, object>> t1Expression, string leftTableColumnExpression, string rightTableColumnExpression, string joinAlias, string joinType)
        {
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
                    columnList.Add(joinAlias + "." + item.Name + " as " + t1AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(joinAlias + "." + item.Name);
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

            if (string.IsNullOrEmpty(TablesJoinKeywords))
            {
                TablesJoinKeywords = " " + joinType + " join " + t1 + " as " + joinAlias + " on " + TableName + "." + leftTableColumnExpression + "=" + joinAlias + "." + rightTableColumnExpression;
            }
            else
            {
                TablesJoinKeywords = TablesJoinKeywords + " " + joinType + " join " + t1 + " as " + joinAlias + " on " + TableName + "." + leftTableColumnExpression + "=" + joinAlias + "." + rightTableColumnExpression;
            }

            return this;
        }
    }

    class PropertyVisitor : ExpressionVisitor
    {
        internal readonly List<MemberInfo> Path = new List<MemberInfo>();

        protected override Expression VisitMember(MemberExpression node)
        {
            this.Path.Add(node.Member);
            return base.VisitMember(node);
        }
    }
}
