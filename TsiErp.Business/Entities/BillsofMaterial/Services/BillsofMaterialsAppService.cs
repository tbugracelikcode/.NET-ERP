using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.BillsofMaterial.BusinessRules;
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    [ServiceRegistration(typeof(IBillsofMaterialsAppService), DependencyInjectionType.Scoped)]
    public class BillsofMaterialsAppService : ApplicationService<BillsofMaterialsResource>, IBillsofMaterialsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public BillsofMaterialsAppService(IStringLocalizer<BillsofMaterialsResource> l) : base(l)
        {
        }

        BillsofMaterialManager _manager { get; set; } = new BillsofMaterialManager();


        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {

                await _manager.CodeControl(_uow.BillsofMaterialsRepository, input.Code, L);

                var entity = ObjectMapper.Map<CreateBillsofMaterialsDto, BillsofMaterials>(input);

                var addedEntity = await _uow.BillsofMaterialsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                    lineEntity.BoMID = addedEntity.Id;
                    await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
                    await _uow.SaveChanges();
                }

                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "BillsofMaterials", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.BillsofMaterialLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, lines.BoMID, lines.Id, true);
                    await _uow.BillsofMaterialLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, id, Guid.Empty, false);
                    var list = (await _uow.BillsofMaterialLinesRepository.GetListAsync(t => t.BoMID == id));
                    foreach (var line in list)
                    {
                        await _uow.BillsofMaterialLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.BillsofMaterialsRepository.DeleteAsync(id);

                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "BillsofMaterials", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(t => t.Id == id,
                t => t.BillsofMaterialLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

                mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

                foreach (var item in mappedEntity.SelectBillsofMaterialLines)
                {
                    item.FinishedProductCode = mappedEntity.FinishedProductCode;
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                    item.UnitSetCode = (await _uow.UnitSetsRepository.GetAsync(t => t.Id == item.UnitSetID)).Code;
                    item.MaterialType = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).ProductType;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "BillsofMaterials", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                           .Join<BillsofMaterials, Products>
                           (
                               p => new { p.Id, p.Code, p.Name, p._Description },
                               b => new { FinishedProductCode = b.Code, FinishedProducName = b.Name },
                               pc => new { pc.FinishedProductID },
                               bc => new { bc.Id },
                               JoinType.Left
                           ).Where(null, true, true, Tables.BillsofMaterials);

                var billsOfMaterials = queryFactory.GetList<ListBillsofMaterialsDto>(query).ToList();
                return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(billsOfMaterials);

                //var query = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(null, true, true, "");
                //var billsOfMaterials = queryFactory.GetList<ListBillsofMaterialsDto>(query).ToList();
                //return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(billsOfMaterials);
            }

            //using (UnitOfWork _uow = new UnitOfWork())
            //{
            //    var list = await _uow.BillsofMaterialsRepository.GetListAsync(null,
            //    t => t.BillsofMaterialLines,
            //    t => t.Products);

            //    var mappedEntity = ObjectMapper.Map<List<BillsofMaterials>, List<ListBillsofMaterialsDto>>(list.ToList());

            //    return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(mappedEntity);
            //}
        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(x => x.Id == input.Id, t => t.BillsofMaterialLines);

                await _manager.UpdateControl(_uow.BillsofMaterialsRepository, input.Code, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdateBillsofMaterialsDto, BillsofMaterials>(input);

                await _uow.BillsofMaterialsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                    lineEntity.BoMID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.BillsofMaterialLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<BillsofMaterials, UpdateBillsofMaterialsDto>(entity);
                before.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "BillsofMaterials", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetSelectListAsync(Guid finishedproductId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(t => t.FinishedProductID == finishedproductId,
               t => t.BillsofMaterialLines,
               t => t.Products);

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

                mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.BillsofMaterialsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(updatedEntity);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }
    }
}
