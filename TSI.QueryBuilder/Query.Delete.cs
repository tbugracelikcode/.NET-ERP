using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Helpers;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Delete(object deleterId)
        {
            if (!_IsSoftDelete)
            {
                string deleteQuery = " delete from " + TableName;
                Sql = deleteQuery;
            }
            else
            {
                UpdateQuerySQL querySQL = new UpdateQuerySQL();

                querySQL.ParameterList.Clear();

                querySQL.ParameterList.Add("@" + IsDeletedField, "1");
                querySQL.ParameterList.Add("@" + DeleterIdField, deleterId);
                querySQL.ParameterList.Add("@" + DeletionTimeField, DateTime.Now.ToString());

                //string deleteQuery = "update " + TableName + " set " + IsDeletedField + "=" + "'" + "1" + "'" + ", " + DeleterIdField + "=" + "'" + deleterId + "'" + ", " + DeletionTimeField + "=" + "'" + DateTime.Now.ToString() + "'";

                string deleteQuery = "update " + TableName + " set " + IsDeletedField + "=" + "@" + IsDeletedField + ", " + DeleterIdField + "=" + "@" + DeleterIdField + ", " + DeletionTimeField + "=" + "@" + DeletionTimeField;

                querySQL.Sql = deleteQuery;

                UpdateHelper.UpdateQueryList(querySQL);

                Sql = querySQL.Sql;
            }

            return this;
        }
    }
}
