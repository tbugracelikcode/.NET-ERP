using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.FicheNumber.Page;
using TsiErp.Localizations.Resources.FinanceManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services
{

    [ServiceRegistration(typeof(IFicheNumbersAppService), DependencyInjectionType.Scoped)]
    public class FicheNumbersAppService : ApplicationService<FicheNumbersResource>, IFicheNumbersAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public FicheNumbersAppService(IStringLocalizer<FicheNumbersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> CreateAsync(CreateFicheNumbersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.FicheNumbers).Insert(new CreateFicheNumbersDto
                {
                    Id = GuidGenerator.CreateGuid(),
                    FicheNo = input.FicheNo,
                    FixedCharacter = input.FixedCharacter,
                    Length_ = input.Length_,
                    Menu_ = input.Menu_
                }).UseIsDelete(false);


                var ficheNumbers = queryFactory.Insert<SelectFicheNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.FicheNumbers, LogType.Insert, ficheNumbers.Id);

                return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);
            }
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new
                {
                    Id = id
                }, false, false, "").UseIsDelete(false);

                var ficheNumbers = queryFactory.Get<SelectFicheNumbersDto>(query);


                LogsAppService.InsertLogToDatabase(ficheNumbers, ficheNumbers, LoginedUserService.UserId, Tables.FicheNumbers, LogType.Get, id);

                return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);

            }
        }

        public async Task<IDataResult<IList<ListFicheNumbersDto>>> GetListAsync(ListFicheNumbersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").UseIsDelete(false);

                var ficheNumbers = queryFactory.GetList<ListFicheNumbersDto>(query).ToList();

                return new SuccessDataResult<IList<ListFicheNumbersDto>>(ficheNumbers);
            }
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> UpdateAsync(UpdateFicheNumbersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);

                var entity = queryFactory.Get<FicheNumbers>(entityQuery);

                var query = queryFactory.Query().From(Tables.FicheNumbers).Update(new UpdateFicheNumbersDto
                {
                    Id = input.Id,
                    Menu_ = entity.Menu_,
                    Length_ = input.Length_,
                    FixedCharacter = input.FixedCharacter,
                    FicheNo = entity.FicheNo + 1
                }).Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);

                var ficheNumbers = queryFactory.Update<SelectFicheNumbersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, ficheNumbers, LoginedUserService.UserId, Tables.FicheNumbers, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);
            }
        }

        public Task<IDataResult<SelectFicheNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
