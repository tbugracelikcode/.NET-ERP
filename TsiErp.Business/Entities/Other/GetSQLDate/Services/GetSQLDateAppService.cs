using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.Entities.Other.GetSQLDate.Dtos;

namespace TsiErp.Business.Entities.Other.GetSQLDate.Services
{
    public class GetSQLDateAppService : IGetSQLDateAppService
    {
        static QueryFactory queryFactory { get; set; } = new QueryFactory();

        public DateTime GetDateFromSQL()
        {
            var dateTimeQuery = queryFactory.Query().Select("GETDATE() AS [SQLDateTime]");

            dateTimeQuery.Sql = dateTimeQuery.Sql.Remove(33, 9);

            var dateTime = queryFactory.GetSQLDate<SelectGetSQLDatesDto>(dateTimeQuery);

            return dateTime.SQLDateTime;
        }
    }
}
