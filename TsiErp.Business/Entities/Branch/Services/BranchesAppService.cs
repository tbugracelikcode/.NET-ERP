using Microsoft.JSInterop;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Branch.BusinessRules;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService<BranchesResource>, IBranchesAppService
    {

        BranchesManager _manager { get; set; } = new BranchesManager();


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
                var listQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(null, true, "And");
                var list = queryFactory.GetList<Branches>(listQuery).ToList();

                await _manager.CodeControl(list, input.Code, L);

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

                //    input.Id = addedEntity.Id;
                //    var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Branches", LogType.Insert, addedEntity.Id);
                //    await _uow.LogsRepository.InsertAsync(log);

                return new SuccessDataResult<SelectBranchesDto>(branches);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(new { Id = id }, true, "And");
                var entity = queryFactory.Get<Branches>(entityQuery);

                var periodQuery = queryFactory.Query().From(Tables.Periods).Select("*").Where(new { BranchID = id }, true, "And");
                var periods = queryFactory.Get<Periods>(periodQuery);

                if (periods.Id != Guid.Empty)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(L["DeleteControlManager"]);
                }



                //SalesPropositions Delete Kontrol


                var query = queryFactory.Query().From(Tables.Branches).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, "And");

                var branches = queryFactory.Update<SelectBranchesDto>(query, "Id", true);


                //Log Kayıt



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
                    }, true, "And");
                var branch = queryFactory.Get<SelectBranchesDto>(query);
                return new SuccessDataResult<SelectBranchesDto>(branch);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync(ListBranchesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Branches).Select("*").Where(null, true, "And");
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
                var entityQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(new { Id = input.Id }, true, "And");
                var entity = queryFactory.Get<Branches>(entityQuery);


                var listQuery = queryFactory.Query().From(Tables.Branches).Select("*").Where(null, true, "And");
                var list = queryFactory.GetList<Branches>(listQuery).ToList();

                await _manager.UpdateControl(list, input.Code, input.Id, entity, L);

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
                }).Where(new { Id = input.Id }, true, "And");

                var branches = queryFactory.Update<SelectBranchesDto>(query, "Id", true);

                //var log = LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, "Branches", LogType.Update, entity.Id);


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
