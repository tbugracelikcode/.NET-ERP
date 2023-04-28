using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Join<T1, T2>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;

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

            #region T2Expression
            var t2Visitor = new PropertyVisitor();
            t2Visitor.Visit(t2Expression.Body);
            var t2Members = t2Visitor.Path;

            var t2newExpression = (NewExpression)t2Expression.Body;

            var t2AliasList = t2newExpression.Type.GetProperties().Zip(t2newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t2Members)
            {
                bool aliasControl = t2AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t2 + "." + item.Name + " as " + t2AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t2 + "." + item.Name);
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

            #region T1 Column Expression
            var t1ColumnVisitor = new PropertyVisitor();
            t1ColumnVisitor.Visit(t1ColumnExpression.Body);
            var t1ColumnMembers = t1ColumnVisitor.Path;
            #endregion

            #region T2 Column Expression
            var t2ColumnVisitor = new PropertyVisitor();
            t2ColumnVisitor.Visit(t2ColumnExpression.Body);
            var t2ColumnMembers = t2ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " + t1 + " as " + t1 + " " + joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;

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

            #region T2 Expression
            var t2Visitor = new PropertyVisitor();
            t2Visitor.Visit(t2Expression.Body);
            var t2Members = t2Visitor.Path;

            var t2newExpression = (NewExpression)t2Expression.Body;

            var t2AliasList = t2newExpression.Type.GetProperties().Zip(t2newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t2Members)
            {
                bool aliasControl = t2AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t2 + "." + item.Name + " as " + t2AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t2 + "." + item.Name);
                }
            }
            #endregion

            #region T3 Expression
            var t3Visitor = new PropertyVisitor();
            t3Visitor.Visit(t3Expression.Body);
            var t3Members = t3Visitor.Path;

            var t3newExpression = (NewExpression)t3Expression.Body;

            var t3AliasList = t3newExpression.Type.GetProperties().Zip(t3newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t3Members)
            {
                bool aliasControl = t3AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t3 + "." + item.Name + " as " + t3AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t3 + "." + item.Name);
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

            #region T1 Column Expression
            var t1ColumnVisitor = new PropertyVisitor();
            t1ColumnVisitor.Visit(t1ColumnExpression.Body);
            var t1ColumnMembers = t1ColumnVisitor.Path;
            #endregion

            #region T2 Column Expression
            var t2ColumnVisitor = new PropertyVisitor();
            t2ColumnVisitor.Visit(t2ColumnExpression.Body);
            var t2ColumnMembers = t2ColumnVisitor.Path;
            #endregion

            #region T3 Column Expression
            var t3ColumnVisitor = new PropertyVisitor();
            t3ColumnVisitor.Visit(t3ColumnExpression.Body);
            var t3ColumnMembers = t3ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

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
