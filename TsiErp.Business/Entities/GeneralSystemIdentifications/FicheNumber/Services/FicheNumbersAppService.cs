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
            var query = queryFactory.Query().From(Tables.FicheNumbers).Insert(new CreateFicheNumbersDto
            {
                Id = GuidGenerator.CreateGuid(),
                FicheNo = input.FicheNo,
                FixedCharacter = input.FixedCharacter,
                Length_ = input.Length_,
                Menu_ = input.Menu_
            }).UseIsDelete(false);

            var ficheNumbers = queryFactory.Insert<SelectFicheNumbersDto>(query, "Id", true);

            return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);

        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new
            {
                Id = id
            }, false, false, "").UseIsDelete(false);

            var ficheNumbers = queryFactory.Get<SelectFicheNumbersDto>(query);

            return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);

        }

        public async Task<IDataResult<IList<ListFicheNumbersDto>>> GetListAsync(ListFicheNumbersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").UseIsDelete(false);

            var ficheNumbers = queryFactory.GetList<ListFicheNumbersDto>(query).ToList();

            return new SuccessDataResult<IList<ListFicheNumbersDto>>(ficheNumbers);
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> UpdateAsync(UpdateFicheNumbersDto input)
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


            return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);
        }

        public Task<IDataResult<SelectFicheNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public string GetFicheNumberAsync(string menu)
        {
            var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new
            {
                Menu_ = menu
            }, false, false, "").UseIsDelete(false);

            var ficheNumbers = queryFactory.Get<SelectFicheNumbersDto>(query);

            string ficheNo = string.Empty;



            if (ficheNumbers != null)
            {
                ficheNo = ficheNumbers.FixedCharacter;

                for (int i = 0; i < ficheNumbers.Length_ - ficheNumbers.FicheNo.ToString().Length; i++)
                {
                    ficheNo = ficheNo + "0";
                }

                ficheNo = ficheNo + ficheNumbers.FicheNo;
            }

            return ficheNo;

        }

        public Task UpdateFicheNumberAsync(string menu, string progFicheNumber)
        {
            #region Get From FicheNumber Table
            var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new
            {
                Menu_ = menu
            }, false, false, "").UseIsDelete(false);

            var ficheNumbers = queryFactory.Get<SelectFicheNumbersDto>(query);

            string ficheNo = string.Empty;

            if (ficheNumbers != null)
            {
                ficheNo = ficheNumbers.FixedCharacter;

                for (int i = 0; i < ficheNumbers.Length_ - ficheNumbers.FicheNo.ToString().Length; i++)
                {
                    ficheNo = ficheNo + "0";
                }

                ficheNo = ficheNo + ficheNumbers.FicheNo;
            }
            #endregion

            var entityQuery = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new { Menu_ = menu }, false, false, "").UseIsDelete(false);

            var entity = queryFactory.Get<FicheNumbers>(entityQuery);


            if (ficheNo == progFicheNumber)
            {
                var updateQuery = queryFactory.Query().From(Tables.FicheNumbers).Update(new UpdateFicheNumbersDto
                {
                    Id = entity.Id,
                    Menu_ = entity.Menu_,
                    Length_ = entity.Length_,
                    FixedCharacter = entity.FixedCharacter,
                    FicheNo = entity.FicheNo + 1
                }).Where(new { Menu_ = menu }, false, false, "").UseIsDelete(false);

                var updatedFicheNumber = queryFactory.Update<SelectFicheNumbersDto>(updateQuery, "Id", true);
            }

            return Task.CompletedTask;

        }
    }
}
