using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Route.BusinessRules;
using TsiErp.Business.Entities.Route.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : ApplicationService<BranchesResource>, IRoutesAppService
    {
        public RoutesAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        RouteManager _manager { get; set; } = new RouteManager();

        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.RoutesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateRoutesDto, Routes>(input);

                var addedEntity = await _uow.RoutesRepository.InsertAsync(entity);

                foreach (var item in input.SelectRouteLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                    lineEntity.RouteID = addedEntity.Id;
                    await _uow.RouteLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Routes", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.RouteLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.RoutesRepository, lines.RouteID, lines.Id, true);
                    await _uow.RouteLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.RoutesRepository, id, Guid.Empty, false);
                    var list = (await _uow.RouteLinesRepository.GetListAsync(t => t.RouteID == id));
                    foreach (var line in list)
                    {
                        await _uow.RouteLinesRepository.DeleteAsync(line.Id);
                    }

                    await _uow.RoutesRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Routes", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.RoutesRepository.GetAsync(t => t.Id == id,
                t => t.RouteLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(entity);

                mappedEntity.SelectRouteLines = ObjectMapper.Map<List<RouteLines>, List<SelectRouteLinesDto>>(entity.RouteLines.ToList());

                foreach (var item in mappedEntity.SelectRouteLines)
                {
                    item.OperationCode = (await _uow.ProductsOperationsRepository.GetAsync(t => t.Id == item.ProductsOperationID)).Code;
                    item.OperationName = (await _uow.ProductsOperationsRepository.GetAsync(t => t.Id == item.ProductsOperationID)).Name;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Routes", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.RoutesRepository.GetListAsync(null,
                t => t.RouteLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<Routes>, List<ListRoutesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListRoutesDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.RoutesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.RoutesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateRoutesDto, Routes>(input);

                await _uow.RoutesRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectRouteLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                    lineEntity.RouteID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.RouteLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.RouteLinesRepository.UpdateAsync(lineEntity);
                    }
                }
                var before = ObjectMapper.Map<Routes, UpdateRoutesDto>(entity);
                before.SelectRouteLines = ObjectMapper.Map<List<RouteLines>, List<SelectRouteLinesDto>>(entity.RouteLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Routes", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsOperationsRepository.GetListAsync(t => t.ProductID == productId, t => t.ProductsOperationLines);

                var mappedEntity = ObjectMapper.Map<List<ProductsOperations>, List<ListProductsOperationsDto>>(entity.ToList());

                return new SuccessDataResult<List<ListProductsOperationsDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> GetSelectListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.RoutesRepository.GetAsync(t => t.ProductID == productId,
                t => t.RouteLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(entity);

                mappedEntity.SelectRouteLines = ObjectMapper.Map<List<RouteLines>, List<SelectRouteLinesDto>>(entity.RouteLines.ToList());

                return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.RoutesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.RoutesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(updatedEntity);

                return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
            }
        }
    }
}
