using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.StockManagement.StockSection.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockSection;
using TsiErp.Entities.Entities.StockManagement.StockSection.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockSections.Page;

namespace TsiErp.Business.Entities.StockSection.Services
{
    [ServiceRegistration(typeof(IStockSectionsAppService), DependencyInjectionType.Scoped)]
    public class StockSectionsAppService : ApplicationService<StockSectionsResource>, IStockSectionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StockSectionsAppService(IStringLocalizer<StockSectionsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStockSectionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockSectionsDto>> CreateAsync(CreateStockSectionsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockSections).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<StockSections>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StockSections).Insert(new CreateStockSectionsDto
            {
                Code = input.Code,
                Name = input.Name,
                Description_ = input.Description_,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var StockSections = queryFactory.Insert<SelectStockSectionsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StockSectionsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockSections, LogType.Insert, addedEntityId);


            return new SuccessDataResult<SelectStockSectionsDto>(StockSections);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("StockSectionID", new List<string>
            {

                Tables.StockAddressLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.StockSections).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var StockSections = queryFactory.Update<SelectStockSectionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockSections, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStockSectionsDto>(StockSections);
            }

        }

        public async Task<IDataResult<SelectStockSectionsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StockSections).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var StockSection = queryFactory.Get<SelectStockSectionsDto>(query);


            LogsAppService.InsertLogToDatabase(StockSection, StockSection, LoginedUserService.UserId, Tables.StockSections, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockSectionsDto>(StockSection);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockSectionsDto>>> GetListAsync(ListStockSectionsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockSections).Select("*").Where(null, false, false, "");
            var StockSections = queryFactory.GetList<ListStockSectionsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockSectionsDto>>(StockSections);

        }


        [ValidationAspect(typeof(UpdateStockSectionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockSectionsDto>> UpdateAsync(UpdateStockSectionsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockSections).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<StockSections>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StockSections).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<StockSections>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StockSections).Update(new UpdateStockSectionsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                Description_ = input.Description_,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var StockSections = queryFactory.Update<SelectStockSectionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StockSections, LoginedUserService.UserId, Tables.StockSections, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockSectionsDto>(StockSections);

        }

        public async Task<IDataResult<SelectStockSectionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockSections).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<StockSections>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockSections).Update(new UpdateStockSectionsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Description_ = entity.Description_,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId

            }).Where(new { Id = id }, false, false, "");

            var StockSections = queryFactory.Update<SelectStockSectionsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockSectionsDto>(StockSections);

        }
    }
}
