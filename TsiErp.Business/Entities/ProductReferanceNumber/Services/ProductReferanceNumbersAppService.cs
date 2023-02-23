using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductReferanceNumber.BusinessRules;
using TsiErp.Business.Entities.ProductReferanceNumber.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    [ServiceRegistration(typeof(IProductReferanceNumbersAppService), DependencyInjectionType.Scoped)]
    public class ProductReferanceNumbersAppService : ApplicationService, IProductReferanceNumbersAppService
    {
        ProductReferanceNumberManager _manager { get; set; } = new ProductReferanceNumberManager();


        [ValidationAspect(typeof(CreateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> CreateAsync(CreateProductReferanceNumbersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductReferanceNumbersRepository, input.ReferanceNo);

                var entity = ObjectMapper.Map<CreateProductReferanceNumbersDto, ProductReferanceNumbers>(input);

                var addedEntity = await _uow.ProductReferanceNumbersRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ProductReferanceNumbers", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.ProductReferanceNumbersRepository, id);
                await _uow.ProductReferanceNumbersRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ProductReferanceNumbers", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectProductReferanceNumbersDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductReferanceNumbersRepository.GetAsync(t => t.Id == id, t => t.Products, t => t.CurrentAccountCards);
                var mappedEntity = ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ProductReferanceNumbers", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectProductReferanceNumbersDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductReferanceNumbersDto>>> GetListAsync(ListProductReferanceNumbersParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                IList<ProductReferanceNumbers> list;

                if (input.ProductId == null)
                {
                    list = await _uow.ProductReferanceNumbersRepository.GetListAsync(null, t => t.Products);
                }
                else
                {
                    list = await _uow.ProductReferanceNumbersRepository.GetListAsync(t => t.ProductID == input.ProductId, t => t.Products);
                }

                var mappedEntity = ObjectMapper.Map<List<ProductReferanceNumbers>, List<ListProductReferanceNumbersDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductReferanceNumbersDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductReferanceNumbersRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ProductReferanceNumbers>, List<SelectProductReferanceNumbersDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectProductReferanceNumbersDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateAsync(UpdateProductReferanceNumbersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductReferanceNumbersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductReferanceNumbersRepository, input.ReferanceNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateProductReferanceNumbersDto, ProductReferanceNumbers>(input);

                await _uow.ProductReferanceNumbersRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<ProductReferanceNumbers, UpdateProductReferanceNumbersDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ProductReferanceNumbers", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductReferanceNumbersRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductReferanceNumbersRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(updatedEntity);

                return new SuccessDataResult<SelectProductReferanceNumbersDto>(mappedEntity);
            }
        }
    }
}
