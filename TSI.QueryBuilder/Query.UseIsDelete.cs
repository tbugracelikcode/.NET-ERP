using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder
{
    public partial class Query
    {
        public Query UseIsDelete(bool isDelete)
        {
            UseIsDeleteInQuery=isDelete;

            return this;
        }
    }
}
