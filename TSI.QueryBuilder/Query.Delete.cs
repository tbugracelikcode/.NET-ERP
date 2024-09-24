using System;
using System.Collections.Generic;
using System.Text;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;

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
                string parameterValues = "";
                string IsDeletedParamField = "@P0";
                string DeleterIdParamField = "@P1";
                string DeletionTimeParamField = "@P2";

                //string deleteQuery = "update " + TableName + " set " + IsDeletedField + "=" + "'" + "1" + "'" + ", " + DeleterIdField + "=" + "'" + deleterId + "'" + ", " + DeletionTimeField + "=" + "'" + DateTime.Now.ToString() + "'";

                string deleteQuery = "update " + TableName + " set " + IsDeletedField + "=" + IsDeletedParamField + ", " + DeleterIdField + "=" + DeleterIdParamField + ", " + DeletionTimeField + "=" + DeletionTimeParamField;

                parameterValues = IsDeletedParamField + "=" + "1";
                parameterValues = parameterValues + "," + DeleterIdParamField + "=" + deleterId;
                parameterValues = parameterValues + "," + DeletionTimeParamField + "=" + DateTime.Now.ToString()+ "*dym*";

                deleteQuery = deleteQuery + QueryConstants.QueryParamsConstant + parameterValues;

                Sql = deleteQuery;
            }

            return this;
        }
    }
}
