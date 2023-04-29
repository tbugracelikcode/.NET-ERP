using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public Query Join<T1, T2, T3, T4>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T8, object>> t8Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, Expression<Func<T8, object>> t8ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;
            string t8 = typeof(T8).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
                }
            }
            #endregion

            #region T8 Expression
            var t8Visitor = new PropertyVisitor();
            t8Visitor.Visit(t8Expression.Body);
            var t8Members = t8Visitor.Path;

            var t8newExpression = (NewExpression)t8Expression.Body;

            var t8AliasList = t8newExpression.Type.GetProperties().Zip(t8newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t8Members)
            {
                bool aliasControl = t8AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t8 + "." + item.Name + " as " + t8AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t8 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion

            #region T8 Column Expression
            var t8ColumnVisitor = new PropertyVisitor();
            t8ColumnVisitor.Visit(t8ColumnExpression.Body);
            var t8ColumnMembers = t8ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name + " " +
                joinType + " join " + t8 + " as " + t8 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t8 + "." + t8ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T8, object>> t8Expression, Expression<Func<T9, object>> t9Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, Expression<Func<T8, object>> t8ColumnExpression, Expression<Func<T9, object>> t9ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;
            string t8 = typeof(T8).Name;
            string t9 = typeof(T9).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
                }
            }
            #endregion

            #region T8 Expression
            var t8Visitor = new PropertyVisitor();
            t8Visitor.Visit(t8Expression.Body);
            var t8Members = t8Visitor.Path;

            var t8newExpression = (NewExpression)t8Expression.Body;

            var t8AliasList = t8newExpression.Type.GetProperties().Zip(t8newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t8Members)
            {
                bool aliasControl = t8AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t8 + "." + item.Name + " as " + t8AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t8 + "." + item.Name);
                }
            }
            #endregion

            #region T9 Expression
            var t9Visitor = new PropertyVisitor();
            t9Visitor.Visit(t9Expression.Body);
            var t9Members = t9Visitor.Path;

            var t9newExpression = (NewExpression)t9Expression.Body;

            var t9AliasList = t9newExpression.Type.GetProperties().Zip(t9newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t9Members)
            {
                bool aliasControl = t9AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t9 + "." + item.Name + " as " + t9AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t9 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion

            #region T8 Column Expression
            var t8ColumnVisitor = new PropertyVisitor();
            t8ColumnVisitor.Visit(t8ColumnExpression.Body);
            var t8ColumnMembers = t8ColumnVisitor.Path;
            #endregion

            #region T9 Column Expression
            var t9ColumnVisitor = new PropertyVisitor();
            t9ColumnVisitor.Visit(t9ColumnExpression.Body);
            var t9ColumnMembers = t9ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name + " " +
                joinType + " join " + t8 + " as " + t8 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t8 + "." + t8ColumnMembers[0].Name + " " +
                joinType + " join " + t9 + " as " + t9 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t9 + "." + t9ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T8, object>> t8Expression, Expression<Func<T9, object>> t9Expression, Expression<Func<T10, object>> t10Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, Expression<Func<T8, object>> t8ColumnExpression, Expression<Func<T9, object>> t9ColumnExpression, Expression<Func<T10, object>> t10ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;
            string t8 = typeof(T8).Name;
            string t9 = typeof(T9).Name;
            string t10 = typeof(T10).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
                }
            }
            #endregion

            #region T8 Expression
            var t8Visitor = new PropertyVisitor();
            t8Visitor.Visit(t8Expression.Body);
            var t8Members = t8Visitor.Path;

            var t8newExpression = (NewExpression)t8Expression.Body;

            var t8AliasList = t8newExpression.Type.GetProperties().Zip(t8newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t8Members)
            {
                bool aliasControl = t8AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t8 + "." + item.Name + " as " + t8AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t8 + "." + item.Name);
                }
            }
            #endregion

            #region T9 Expression
            var t9Visitor = new PropertyVisitor();
            t9Visitor.Visit(t9Expression.Body);
            var t9Members = t9Visitor.Path;

            var t9newExpression = (NewExpression)t9Expression.Body;

            var t9AliasList = t9newExpression.Type.GetProperties().Zip(t9newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t9Members)
            {
                bool aliasControl = t9AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t9 + "." + item.Name + " as " + t9AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t9 + "." + item.Name);
                }
            }
            #endregion

            #region T10 Expression
            var t10Visitor = new PropertyVisitor();
            t10Visitor.Visit(t10Expression.Body);
            var t10Members = t10Visitor.Path;

            var t10newExpression = (NewExpression)t10Expression.Body;

            var t10AliasList = t10newExpression.Type.GetProperties().Zip(t10newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t10Members)
            {
                bool aliasControl = t10AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t10 + "." + item.Name + " as " + t10AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t10 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion

            #region T8 Column Expression
            var t8ColumnVisitor = new PropertyVisitor();
            t8ColumnVisitor.Visit(t8ColumnExpression.Body);
            var t8ColumnMembers = t8ColumnVisitor.Path;
            #endregion

            #region T9 Column Expression
            var t9ColumnVisitor = new PropertyVisitor();
            t9ColumnVisitor.Visit(t9ColumnExpression.Body);
            var t9ColumnMembers = t9ColumnVisitor.Path;
            #endregion

            #region T10 Column Expression
            var t10ColumnVisitor = new PropertyVisitor();
            t10ColumnVisitor.Visit(t10ColumnExpression.Body);
            var t10ColumnMembers = t10ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name + " " +
                joinType + " join " + t8 + " as " + t8 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t8 + "." + t8ColumnMembers[0].Name + " " +
                joinType + " join " + t9 + " as " + t9 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t9 + "." + t9ColumnMembers[0].Name + " " +
                joinType + " join " + t10 + " as " + t10 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t10 + "." + t10ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T8, object>> t8Expression, Expression<Func<T9, object>> t9Expression, Expression<Func<T10, object>> t10Expression, Expression<Func<T11, object>> t11Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, Expression<Func<T8, object>> t8ColumnExpression, Expression<Func<T9, object>> t9ColumnExpression, Expression<Func<T10, object>> t10ColumnExpression, Expression<Func<T11, object>> t11ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;
            string t8 = typeof(T8).Name;
            string t9 = typeof(T9).Name;
            string t10 = typeof(T10).Name;
            string t11 = typeof(T11).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
                }
            }
            #endregion

            #region T8 Expression
            var t8Visitor = new PropertyVisitor();
            t8Visitor.Visit(t8Expression.Body);
            var t8Members = t8Visitor.Path;

            var t8newExpression = (NewExpression)t8Expression.Body;

            var t8AliasList = t8newExpression.Type.GetProperties().Zip(t8newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t8Members)
            {
                bool aliasControl = t8AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t8 + "." + item.Name + " as " + t8AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t8 + "." + item.Name);
                }
            }
            #endregion

            #region T9 Expression
            var t9Visitor = new PropertyVisitor();
            t9Visitor.Visit(t9Expression.Body);
            var t9Members = t9Visitor.Path;

            var t9newExpression = (NewExpression)t9Expression.Body;

            var t9AliasList = t9newExpression.Type.GetProperties().Zip(t9newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t9Members)
            {
                bool aliasControl = t9AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t9 + "." + item.Name + " as " + t9AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t9 + "." + item.Name);
                }
            }
            #endregion

            #region T10 Expression
            var t10Visitor = new PropertyVisitor();
            t10Visitor.Visit(t10Expression.Body);
            var t10Members = t10Visitor.Path;

            var t10newExpression = (NewExpression)t10Expression.Body;

            var t10AliasList = t10newExpression.Type.GetProperties().Zip(t10newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t10Members)
            {
                bool aliasControl = t10AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t10 + "." + item.Name + " as " + t10AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t10 + "." + item.Name);
                }
            }
            #endregion

            #region T11 Expression
            var t11Visitor = new PropertyVisitor();
            t11Visitor.Visit(t11Expression.Body);
            var t11Members = t11Visitor.Path;

            var t11newExpression = (NewExpression)t11Expression.Body;

            var t11AliasList = t11newExpression.Type.GetProperties().Zip(t11newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t11Members)
            {
                bool aliasControl = t11AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t11 + "." + item.Name + " as " + t11AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t11 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion

            #region T8 Column Expression
            var t8ColumnVisitor = new PropertyVisitor();
            t8ColumnVisitor.Visit(t8ColumnExpression.Body);
            var t8ColumnMembers = t8ColumnVisitor.Path;
            #endregion

            #region T9 Column Expression
            var t9ColumnVisitor = new PropertyVisitor();
            t9ColumnVisitor.Visit(t9ColumnExpression.Body);
            var t9ColumnMembers = t9ColumnVisitor.Path;
            #endregion

            #region T10 Column Expression
            var t10ColumnVisitor = new PropertyVisitor();
            t10ColumnVisitor.Visit(t10ColumnExpression.Body);
            var t10ColumnMembers = t10ColumnVisitor.Path;
            #endregion

            #region T11 Column Expression
            var t11ColumnVisitor = new PropertyVisitor();
            t11ColumnVisitor.Visit(t11ColumnExpression.Body);
            var t11ColumnMembers = t11ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name + " " +
                joinType + " join " + t8 + " as " + t8 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t8 + "." + t8ColumnMembers[0].Name + " " +
                joinType + " join " + t9 + " as " + t9 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t9 + "." + t9ColumnMembers[0].Name + " " +
                joinType + " join " + t10 + " as " + t10 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t10 + "." + t10ColumnMembers[0].Name + " " +
                joinType + " join " + t11 + " as " + t11 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t11 + "." + t11ColumnMembers[0].Name;

            query = query + WhereSentence;

            Sql = query;

            return this;
        }

        public Query Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T1, object>> t1Expression, Expression<Func<T2, object>> t2Expression, Expression<Func<T3, object>> t3Expression, Expression<Func<T4, object>> t4Expression, Expression<Func<T5, object>> t5Expression, Expression<Func<T6, object>> t6Expression, Expression<Func<T7, object>> t7Expression, Expression<Func<T8, object>> t8Expression, Expression<Func<T9, object>> t9Expression, Expression<Func<T10, object>> t10Expression, Expression<Func<T11, object>> t11Expression, Expression<Func<T12, object>> t12Expression, Expression<Func<T1, object>> t1ColumnExpression, Expression<Func<T2, object>> t2ColumnExpression, Expression<Func<T3, object>> t3ColumnExpression, Expression<Func<T4, object>> t4ColumnExpression, Expression<Func<T5, object>> t5ColumnExpression, Expression<Func<T6, object>> t6ColumnExpression, Expression<Func<T7, object>> t7ColumnExpression, Expression<Func<T8, object>> t8ColumnExpression, Expression<Func<T9, object>> t9ColumnExpression, Expression<Func<T10, object>> t10ColumnExpression, Expression<Func<T11, object>> t11ColumnExpression, Expression<Func<T12, object>> t12ColumnExpression, string joinType)
        {
            string query = "";

            string t1 = typeof(T1).Name;
            string t2 = typeof(T2).Name;
            string t3 = typeof(T3).Name;
            string t4 = typeof(T4).Name;
            string t5 = typeof(T5).Name;
            string t6 = typeof(T6).Name;
            string t7 = typeof(T7).Name;
            string t8 = typeof(T8).Name;
            string t9 = typeof(T9).Name;
            string t10 = typeof(T10).Name;
            string t11 = typeof(T11).Name;
            string t12 = typeof(T12).Name;

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

            #region T4 Expression
            var t4Visitor = new PropertyVisitor();
            t4Visitor.Visit(t4Expression.Body);
            var t4Members = t4Visitor.Path;

            var t4newExpression = (NewExpression)t4Expression.Body;

            var t4AliasList = t4newExpression.Type.GetProperties().Zip(t4newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t4Members)
            {
                bool aliasControl = t4AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t4 + "." + item.Name + " as " + t4AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t4 + "." + item.Name);
                }
            }
            #endregion

            #region T5 Expression
            var t5Visitor = new PropertyVisitor();
            t5Visitor.Visit(t5Expression.Body);
            var t5Members = t5Visitor.Path;

            var t5newExpression = (NewExpression)t5Expression.Body;

            var t5AliasList = t5newExpression.Type.GetProperties().Zip(t5newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t5Members)
            {
                bool aliasControl = t5AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t5 + "." + item.Name + " as " + t5AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t5 + "." + item.Name);
                }
            }
            #endregion

            #region T6 Expression
            var t6Visitor = new PropertyVisitor();
            t6Visitor.Visit(t6Expression.Body);
            var t6Members = t6Visitor.Path;

            var t6newExpression = (NewExpression)t6Expression.Body;

            var t6AliasList = t6newExpression.Type.GetProperties().Zip(t6newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t6Members)
            {
                bool aliasControl = t6AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t6 + "." + item.Name + " as " + t6AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t6 + "." + item.Name);
                }
            }
            #endregion

            #region T7 Expression
            var t7Visitor = new PropertyVisitor();
            t7Visitor.Visit(t7Expression.Body);
            var t7Members = t7Visitor.Path;

            var t7newExpression = (NewExpression)t7Expression.Body;

            var t7AliasList = t7newExpression.Type.GetProperties().Zip(t7newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t7Members)
            {
                bool aliasControl = t7AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t7 + "." + item.Name + " as " + t7AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t7 + "." + item.Name);
                }
            }
            #endregion

            #region T8 Expression
            var t8Visitor = new PropertyVisitor();
            t8Visitor.Visit(t8Expression.Body);
            var t8Members = t8Visitor.Path;

            var t8newExpression = (NewExpression)t8Expression.Body;

            var t8AliasList = t8newExpression.Type.GetProperties().Zip(t8newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t8Members)
            {
                bool aliasControl = t8AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t8 + "." + item.Name + " as " + t8AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t8 + "." + item.Name);
                }
            }
            #endregion

            #region T9 Expression
            var t9Visitor = new PropertyVisitor();
            t9Visitor.Visit(t9Expression.Body);
            var t9Members = t9Visitor.Path;

            var t9newExpression = (NewExpression)t9Expression.Body;

            var t9AliasList = t9newExpression.Type.GetProperties().Zip(t9newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t9Members)
            {
                bool aliasControl = t9AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t9 + "." + item.Name + " as " + t9AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t9 + "." + item.Name);
                }
            }
            #endregion

            #region T10 Expression
            var t10Visitor = new PropertyVisitor();
            t10Visitor.Visit(t10Expression.Body);
            var t10Members = t10Visitor.Path;

            var t10newExpression = (NewExpression)t10Expression.Body;

            var t10AliasList = t10newExpression.Type.GetProperties().Zip(t10newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t10Members)
            {
                bool aliasControl = t10AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t10 + "." + item.Name + " as " + t10AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t10 + "." + item.Name);
                }
            }
            #endregion

            #region T11 Expression
            var t11Visitor = new PropertyVisitor();
            t11Visitor.Visit(t11Expression.Body);
            var t11Members = t11Visitor.Path;

            var t11newExpression = (NewExpression)t11Expression.Body;

            var t11AliasList = t11newExpression.Type.GetProperties().Zip(t11newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t11Members)
            {
                bool aliasControl = t11AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t11 + "." + item.Name + " as " + t11AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t11 + "." + item.Name);
                }
            }
            #endregion

            #region T12 Expression
            var t12Visitor = new PropertyVisitor();
            t12Visitor.Visit(t12Expression.Body);
            var t12Members = t12Visitor.Path;

            var t12newExpression = (NewExpression)t12Expression.Body;

            var t12AliasList = t12newExpression.Type.GetProperties().Zip(t12newExpression.Arguments.OfType<MemberExpression>(), (p, m) => new { AliasName = p.Name, RealName = m.Member.Name }).ToList();

            foreach (var item in t12Members)
            {
                bool aliasControl = t12AliasList.Any(t => t.RealName == item.Name);

                if (aliasControl)
                {
                    columnList.Add(t12 + "." + item.Name + " as " + t12AliasList.Where(t => t.RealName == item.Name).Select(t => t.AliasName).FirstOrDefault());
                }
                else
                {
                    columnList.Add(t12 + "." + item.Name);
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

            #region T4 Column Expression
            var t4ColumnVisitor = new PropertyVisitor();
            t4ColumnVisitor.Visit(t4ColumnExpression.Body);
            var t4ColumnMembers = t4ColumnVisitor.Path;
            #endregion

            #region T5 Column Expression
            var t5ColumnVisitor = new PropertyVisitor();
            t5ColumnVisitor.Visit(t5ColumnExpression.Body);
            var t5ColumnMembers = t5ColumnVisitor.Path;
            #endregion

            #region T6 Column Expression
            var t6ColumnVisitor = new PropertyVisitor();
            t6ColumnVisitor.Visit(t6ColumnExpression.Body);
            var t6ColumnMembers = t6ColumnVisitor.Path;
            #endregion

            #region T7 Column Expression
            var t7ColumnVisitor = new PropertyVisitor();
            t7ColumnVisitor.Visit(t7ColumnExpression.Body);
            var t7ColumnMembers = t7ColumnVisitor.Path;
            #endregion

            #region T8 Column Expression
            var t8ColumnVisitor = new PropertyVisitor();
            t8ColumnVisitor.Visit(t8ColumnExpression.Body);
            var t8ColumnMembers = t8ColumnVisitor.Path;
            #endregion

            #region T9 Column Expression
            var t9ColumnVisitor = new PropertyVisitor();
            t9ColumnVisitor.Visit(t9ColumnExpression.Body);
            var t9ColumnMembers = t9ColumnVisitor.Path;
            #endregion

            #region T10 Column Expression
            var t10ColumnVisitor = new PropertyVisitor();
            t10ColumnVisitor.Visit(t10ColumnExpression.Body);
            var t10ColumnMembers = t10ColumnVisitor.Path;
            #endregion

            #region T11 Column Expression
            var t11ColumnVisitor = new PropertyVisitor();
            t11ColumnVisitor.Visit(t11ColumnExpression.Body);
            var t11ColumnMembers = t11ColumnVisitor.Path;
            #endregion

            #region T12 Column Expression
            var t12ColumnVisitor = new PropertyVisitor();
            t12ColumnVisitor.Visit(t12ColumnExpression.Body);
            var t12ColumnMembers = t12ColumnVisitor.Path;
            #endregion


            query = "select " + columns + " from " +
                t1 + " as " + t1 + " " +
                joinType + " join " + t2 + " as " + t2 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t2 + "." + t2ColumnMembers[0].Name + " " +
                joinType + " join " + t3 + " as " + t3 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t3 + "." + t3ColumnMembers[0].Name + " " +
                joinType + " join " + t4 + " as " + t4 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t4 + "." + t4ColumnMembers[0].Name + " " +
                joinType + " join " + t5 + " as " + t5 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t5 + "." + t5ColumnMembers[0].Name + " " +
                joinType + " join " + t6 + " as " + t6 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t6 + "." + t6ColumnMembers[0].Name + " " +
                joinType + " join " + t7 + " as " + t7 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t7 + "." + t7ColumnMembers[0].Name + " " +
                joinType + " join " + t8 + " as " + t8 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t8 + "." + t8ColumnMembers[0].Name + " " +
                joinType + " join " + t9 + " as " + t9 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t9 + "." + t9ColumnMembers[0].Name + " " +
                joinType + " join " + t10 + " as " + t10 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t10 + "." + t10ColumnMembers[0].Name + " " +
                joinType + " join " + t11 + " as " + t11 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t11 + "." + t11ColumnMembers[0].Name + " " +
                joinType + " join " + t12 + " as " + t12 + " on " + t1 + "." + t1ColumnMembers[0].Name + "=" + t12 + "." + t12ColumnMembers[0].Name;

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
