using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using TSI.QueryBuilder.BaseClasses;

namespace TsiErp.Business.Extensions.DeleteControlExtension
{
    public static class DeleteControl
    {
        public static Dictionary<string, List<string>> ControlList { get; set; } = new Dictionary<string, List<string>>();

        public static bool Control(QueryFactory queryFactory, Guid id)
        {
            bool control = true;

            foreach (var fieldItem in ControlList)
            {
                if (!control)
                {
                    break;
                }

                string field = fieldItem.Key;

                foreach (var tableItem in fieldItem.Value)
                {
                    var query = queryFactory.Query().From(tableItem).Count(field).Where(field + "= '" + id + "'");
                    var result = queryFactory.Get<int>(query);

                    if (result > 0)
                    {
                        control = false;
                        break;
                    }
                }
            }

            return control;
        }
    }
}
