using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.FicheNumber.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services
{

    [ServiceRegistration(typeof(IFicheNumbersAppService), DependencyInjectionType.Scoped)]
    public class FicheNumbersAppService : ApplicationService<FicheNumbersResource>, IFicheNumbersAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public FicheNumbersAppService(IStringLocalizer<FicheNumbersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
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
            
            await Task.CompletedTask;
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
            }, "").UseIsDelete(false);

            var ficheNumbers = queryFactory.Get<SelectFicheNumbersDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectFicheNumbersDto>(ficheNumbers);

        }

        public async Task<IDataResult<IList<ListFicheNumbersDto>>> GetListAsync(ListFicheNumbersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.FicheNumbers).Select("*").UseIsDelete(false);

            var ficheNumbers = queryFactory.GetList<ListFicheNumbersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListFicheNumbersDto>>(ficheNumbers);
        }

        public async Task<IDataResult<SelectFicheNumbersDto>> UpdateAsync(UpdateFicheNumbersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new { Id = input.Id }, "").UseIsDelete(false);

            var entity = queryFactory.Get<FicheNumbers>(entityQuery);

            var query = queryFactory.Query().From(Tables.FicheNumbers).Update(new UpdateFicheNumbersDto
            {
                Id = input.Id,
                Menu_ = entity.Menu_,
                Length_ = input.Length_,
                FixedCharacter = input.FixedCharacter,
                FicheNo = entity.FicheNo + 1
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var ficheNumbers = queryFactory.Update<SelectFicheNumbersDto>(query, "Id", true);


            await Task.CompletedTask;
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
            }, "").UseIsDelete(false);

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
            }, "").UseIsDelete(false);

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

            var entityQuery = queryFactory.Query().From(Tables.FicheNumbers).Select("*").Where(new { Menu_ = menu }, "").UseIsDelete(false);

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
                }).Where(new { Menu_ = menu }, "").UseIsDelete(false);

                var updatedFicheNumber = queryFactory.Update<SelectFicheNumbersDto>(updateQuery, "Id", true);
            }

            return Task.CompletedTask;

        }
    }
}
