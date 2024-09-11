using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TSI.QueryBuilder.Constants.Join;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Where(object constraints, string joinSeperator)
        {
            string where = "";

            JoinSeperator = joinSeperator;

            var dictionary = new Dictionary<string, object>();

            string parameterValues = "";

            if (constraints != null)
            {
                foreach (var item in constraints.GetType().GetRuntimeProperties())
                {
                    if(item.PropertyType == typeof(Nullable<DateTime>) || item.PropertyType == typeof(DateTime))
                    {
                        dictionary.Add(item.Name, item.GetValue(constraints) + "*dym*");
                    }
                    else
                    {
                        dictionary.Add(item.Name, item.GetValue(constraints));
                    }
                }

                int parameterCounter = 0;

                foreach (var dict in dictionary)
                {
                    string parameterName = "@W" + parameterCounter;

                    string whereClause = dict.Key + "=" + parameterName;

                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        whereClause = joinSeperator + "." + whereClause;
                    }

                    if (parameterCounter == 0)
                    {
                        where = whereClause;

                        parameterValues = parameterName + "=" + dict.Value;
                    }
                    else
                    {
                        where = where + " And " + whereClause;

                        parameterValues = parameterValues + "," + parameterName + "=" + dict.Value;
                    }

                    parameterCounter++;
                }

                WhereSentence = where + QueryConstants.QueryWhereParamsConstant + parameterValues;

            }
            else
            {
                WhereSentence = where;
            }

            return this;
        }

        public Query Where(string query)
        {
            WhereSentence = query;

            return this;
        }

        public Query WhereDateRangeIsEqual(string column, DateTime firstParameter, DateTime secondParameter, string joinSeperator)
        {
            string where = "";

            JoinSeperator = joinSeperator;

            string parameterValues = "";

            if (!string.IsNullOrEmpty(column))
            {
                if (!string.IsNullOrEmpty(joinSeperator))
                {
                    column = joinSeperator + "." + column;
                }


                string whereClause = column + ">=" + " @W0" + " and " + column + "<=" + " @W1 ";
                where = whereClause;

                string firtParamValue = "@W0=" + firstParameter + "*dym*";
                string secondParamValue = "@W1=" + secondParameter + "*dym*";

                parameterValues = firtParamValue + "," + secondParamValue;

                WhereSentence = where + QueryConstants.QueryWhereParamsConstant + parameterValues;
            }
            else
            {
                WhereSentence = where;
            }

            return this;
        }

        public Query AndWhereRange(string leftColumn, string rightColumn, DateTime inputValue, string joinSeperator)
        {
            if (!string.IsNullOrEmpty(WhereSentence))
            {
                string where = "";

                JoinSeperator = joinSeperator;

                string parameterValues = "";

                if (!string.IsNullOrEmpty(leftColumn) && !string.IsNullOrEmpty(rightColumn))
                {
                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        leftColumn = joinSeperator + "." + leftColumn;
                        rightColumn = joinSeperator + "." + rightColumn;
                    }

                    string[] whereSql = WhereSentence.Split(QueryConstants.QueryWhereParamsConstant);

                    string whereQuery = whereSql[0];
                    string whereParameters = whereSql[1];

                    int counter = whereQuery.Split("And").Length;

                    string parameterName = "@W" + counter;

                    string whereClause = parameterName + ">=" + leftColumn + " and " + parameterName + "<=" + rightColumn;
                    where = whereClause;

                    string firstParamValue = parameterName + "=" + inputValue + "*dym*";

                    parameterValues = firstParamValue;



                    string parameters = whereParameters + "," + parameterValues;


                    WhereSentence = whereQuery + " and " + where + QueryConstants.QueryWhereParamsConstant + parameters;
                }
            }
            return this;
        }

        public Query Where(string column, object value, string joinSeperator)
        {
            return Where(column, "=", value, joinSeperator);
        }

        public Query OrWhere(object constraints, string joinSeperator)
        {
            string where = "";

            JoinSeperator = joinSeperator;

            var dictionary = new Dictionary<string, object>();

            if (constraints != null)
            {
                foreach (var item in constraints.GetType().GetRuntimeProperties())
                {
                    dictionary.Add(item.Name, item.GetValue(constraints));
                }

                int counter = 0;

                foreach (var dict in dictionary)
                {
                    string whereClause = dict.Key + "=" + "'" + dict.Value + "'";

                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        whereClause = joinSeperator + "." + dict.Key + "=" + "'" + dict.Value + "'";
                    }

                    if (counter == 0)
                    {
                        if (string.IsNullOrEmpty(WhereSentence))
                        {
                            where = whereClause;
                            counter++;
                        }
                        else
                        {
                            where = " Or " + whereClause;
                            counter++;
                        }
                    }
                    else
                    {
                        where = where + " Or " + whereClause;
                    }
                }
            }

            WhereSentence = WhereSentence + " " + where;

            return this;
        }

        public Query OrWhere(object constraints, bool useIsActive, bool IsActive, string joinSeperator)
        {
            string where = "";

            JoinSeperator = joinSeperator;

            var dictionary = new Dictionary<string, object>();

            if (constraints != null)
            {
                foreach (var item in constraints.GetType().GetRuntimeProperties())
                {
                    dictionary.Add(item.Name, item.GetValue(constraints));
                }

                int counter = 0;

                foreach (var dict in dictionary)
                {
                    string whereClause = dict.Key + "=" + "'" + dict.Value + "'";

                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        whereClause = joinSeperator + "." + dict.Key + "=" + "'" + dict.Value + "'";
                    }

                    if (counter == 0)
                    {
                        if (string.IsNullOrEmpty(WhereSentence))
                        {
                            where = whereClause;
                            counter++;
                        }
                        else
                        {
                            where = " Or " + whereClause;
                            counter++;
                        }
                    }
                    else
                    {
                        where = where + " Or " + whereClause;
                    }
                }
            }

            if (string.IsNullOrEmpty(WhereSentence))
            {
                if (useIsActive)
                {
                    if (IsActive)
                    {
                        string isActiveField = " IsActive='1'";

                        if (!string.IsNullOrEmpty(joinSeperator))
                        {
                            isActiveField = joinSeperator + "." + isActiveField.Trim();
                        }

                        if (!string.IsNullOrEmpty(where))
                        {
                            where = where + " And" + isActiveField;
                        }
                        else
                        {
                            where = isActiveField;
                        }
                    }
                    else
                    {
                        string isActiveField = " IsActive='0'";

                        if (!string.IsNullOrEmpty(joinSeperator))
                        {
                            isActiveField = joinSeperator + "." + isActiveField.Trim();
                        }

                        if (!string.IsNullOrEmpty(where))
                        {
                            where = where + " And" + isActiveField;
                        }
                        else
                        {
                            where = isActiveField;
                        }
                    }
                }
            }

            WhereSentence = WhereSentence + " " + where;

            return this;
        }

        public Query Where(string column, string op, object value, string joinSeperator)
        {
            //Method = "select * from Branches where Code<>'001'";

            string tableName = TableName;

            string where = "";

            JoinSeperator = joinSeperator;

            string parameterValues = "";


            if (!string.IsNullOrEmpty(tableName))
            {
                string parameterName = "@W0";

                //string whereClause = column + op + " " + "'" + value.ToString() + "'";

                string whereClause = column + op + parameterName;

                if (!string.IsNullOrEmpty(joinSeperator))
                {
                    //whereClause = joinSeperator + "." + column + op + " " + "'" + value.ToString() + "'";
                    whereClause = joinSeperator + "." + whereClause;
                }

                if (string.IsNullOrEmpty(WhereSentence))
                {
                    where = whereClause;
                    parameterValues = parameterName + "=" + value.ToString();
                    WhereSentence = where + QueryConstants.QueryWhereParamsConstant + parameterValues;
                }
                else
                {
                    where = whereClause;
                    parameterValues = parameterValues + "," + parameterName + "=" + value.ToString();
                    //WhereSentence = WhereSentence + " And " + where;
                    WhereSentence = WhereSentence + " And " + where + QueryConstants.QueryWhereParamsConstant + parameterValues;
                }

            }

            return this;
        }

        public Query WhereIn(string column, string joinSeperator, params string[] values)
        {
            JoinSeperator = joinSeperator;

            if (values.Length > 0)
            {
                string tableName = TableName;


                if (!string.IsNullOrEmpty(tableName))
                {
                    string inSentence = "";

                    string where = "";

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

                    where = column + inSentence;

                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        where = joinSeperator + "." + column + inSentence;
                    }

                    if (string.IsNullOrEmpty(WhereSentence))
                    {
                        WhereSentence = where;
                    }
                    else
                    {
                        WhereSentence = WhereSentence + " And " + where;
                    }
                }
            }
            return this;
        }

        public Query WhereNotIn(string column, string joinSeperator, params string[] values)
        {
            JoinSeperator = joinSeperator;

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

                    if (!string.IsNullOrEmpty(joinSeperator))
                    {
                        where = joinSeperator + "." + column + inSentence;
                    }

                    if (string.IsNullOrEmpty(WhereSentence))
                    {
                        WhereSentence = where;
                    }
                    else
                    {
                        WhereSentence = WhereSentence + " And " + where;
                    }
                }
            }
            return this;
        }

        public Query WhereContains(string column, string value, string joinSeperator)
        {
            string tableName = TableName;

            JoinSeperator = joinSeperator;

            if (!string.IsNullOrEmpty(tableName))
            {
                string containsSentence = column + " like '%" + value + "%'";

                if (!string.IsNullOrEmpty(joinSeperator))
                {
                    containsSentence = joinSeperator + "." + column + " like '%" + value + "%'";
                }

                string whereSentence = containsSentence;

                if (string.IsNullOrEmpty(WhereSentence))
                {
                    WhereSentence = whereSentence;
                }
                else
                {
                    WhereSentence = WhereSentence + " And " + whereSentence;
                }
            }

            return this;
        }

        public Query WhereStartingWith(string column, string value, string joinSeperator)
        {
            string tableName = TableName;

            JoinSeperator = joinSeperator;

            if (!string.IsNullOrEmpty(tableName))
            {
                string startingwithSentence = column + " like '" + value + "%'";

                if (!string.IsNullOrEmpty(joinSeperator))
                {
                    startingwithSentence = joinSeperator + "." + column + " like '" + value + "%'";
                }

                string whereSentence = startingwithSentence;

                if (string.IsNullOrEmpty(WhereSentence))
                {
                    WhereSentence = whereSentence;
                }
                else
                {
                    WhereSentence = WhereSentence + " And " + whereSentence;
                }
            }

            return this;
        }

        public Query WhereEndingWith(string column, string value, string joinSeperator)
        {
            string tableName = TableName;

            JoinSeperator = joinSeperator;

            if (!string.IsNullOrEmpty(tableName))
            {
                string endingwithSentence = column + " like '%" + value + "'";

                if (!string.IsNullOrEmpty(joinSeperator))
                {
                    endingwithSentence = joinSeperator + "." + column + " like '%" + value + "'";
                }

                string whereSentence = endingwithSentence;

                if (string.IsNullOrEmpty(WhereSentence))
                {
                    WhereSentence = whereSentence;
                }
                else
                {
                    WhereSentence = WhereSentence + " And " + whereSentence;
                }
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