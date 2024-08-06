using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.BaseClasses;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query Delete(object deleterId)
        {
            if (!UseIsDeleteInQuery)
            {
                string deleteQuery = " delete from " + TableName;
                Sql = deleteQuery;
            }
            else
            {
                string deleteQuery = "update " + TableName + " set " + IsDeletedField + "=" + "'" + "1" + "'" + ", " + DeleterIdField + "=" + "'" + deleterId + "'" + ", " + DeletionTimeField + "=" + "'" + DateTime.Now.ToString() + "'";
                Sql = deleteQuery;
            }

            return this;
        }
    }
}
