using JsonDiffer;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Branch.BusinessRules;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Logging.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService<BranchesResource>, IBranchesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public BranchesAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {

        }

        [ValidationAspect(typeof(CreateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> CreateAsync(CreateBranchesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var listQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(new { Code = input.Code }, false, false);
                
                var list = queryFactory.ControlList<Branches>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.Branches).Insert(new CreateBranchesDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Name = input.Name,
                    IsActive = true,
                    Id = GuidGenerator.CreateGuid(),
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var branches = queryFactory.Insert<SelectBranchesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Branches", LogType.Insert, branches.Id);

                return new SuccessDataResult<SelectBranchesDto>(branches);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region Delete Control

                var periodQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { BranchID = id }, true, true);
                var periods = queryFactory.Get<Periods>(periodQuery);

                if (periods != null && periods.Id != Guid.Empty)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(L["DeleteControlManager"]);
                }

                var salesPropositionQuery = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { BranchID = id }, false, false);
                var salesPropositions = queryFactory.Get<SalesPropositions>(salesPropositionQuery);

                if (salesPropositions != null && salesPropositions.Id != Guid.Empty)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(L["DeleteControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Branches).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true);

                var branches = queryFactory.Update<SelectBranchesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Branches", LogType.Delete, id);

                return new SuccessDataResult<SelectBranchesDto>(branches);
            }
        }


        public async Task<IDataResult<SelectBranchesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Branches).Select("*").Where(
                new
                {
                    Id = id
                }, true, true);
                var branch = queryFactory.Get<SelectBranchesDto>(query);


                LogsAppService.InsertLogToDatabase(branch, branch, LoginedUserService.UserId, "Branches", LogType.Get, id);

                return new SuccessDataResult<SelectBranchesDto>(branch);

            }
        }



        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync(ListBranchesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Branches).Select("*").Where(null, true, true);
                var branches = queryFactory.GetList<ListBranchesDto>(query).ToList();
                return new SuccessDataResult<IList<ListBranchesDto>>(branches);
            }
        }


        [ValidationAspect(typeof(UpdateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> UpdateAsync(UpdateBranchesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(new { Id = input.Id }, true, true);
                var entity = queryFactory.Get<Branches>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(new { Code = input.Code }, true, true);
                var list = queryFactory.GetList<Branches>(listQuery).ToList();

                if (list.Count > 0)
                {
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Branches).Update(new UpdateBranchesDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true);

                var branches = queryFactory.Update<SelectBranchesDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, branches, LoginedUserService.UserId, "Branches", LogType.Update, entity.Id);


                return new SuccessDataResult<SelectBranchesDto>(branches);
            }
        }

        public async Task<IDataResult<SelectBranchesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BranchRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.BranchRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(updatedEntity);

                return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
            }
        }
    }
}
