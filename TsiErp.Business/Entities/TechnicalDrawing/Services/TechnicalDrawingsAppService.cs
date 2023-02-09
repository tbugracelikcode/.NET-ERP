using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.Business.Entities.TechnicalDrawing.Validations;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Business.Entities.TechnicalDrawing.BusinessRules;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    [ServiceRegistration(typeof(ITechnicalDrawingsAppService), DependencyInjectionType.Scoped)]
    public class TechnicalDrawingsAppService : ApplicationService, ITechnicalDrawingsAppService
    {
        private readonly ITechnicalDrawingsRepository _repository;

        TechnicalDrawingManager _manager { get; set; } = new TechnicalDrawingManager();

        public TechnicalDrawingsAppService(ITechnicalDrawingsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> CreateAsync(CreateTechnicalDrawingsDto input)
        {
            await _manager.CodeControl(_repository, input.RevisionNo);

            var entity = ObjectMapper.Map<CreateTechnicalDrawingsDto, TechnicalDrawings>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectTechnicalDrawingsDto>(ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectTechnicalDrawingsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Products);
            var mappedEntity = ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(entity);
            return new SuccessDataResult<SelectTechnicalDrawingsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTechnicalDrawingsDto>>> GetListAsync(ListTechnicalDrawingsParameterDto input)
        {
            IList<TechnicalDrawings> list;

            if(input.ProductId == null)
            {
                 list = await _repository.GetListAsync(null, t => t.Products);
            }
            else
            {
                 list = await _repository.GetListAsync( t => t.ProductID == input.ProductId, t => t.Products);
            }
            

            var mappedEntity = ObjectMapper.Map<List<TechnicalDrawings>, List<ListTechnicalDrawingsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListTechnicalDrawingsDto>>(mappedEntity);
        }


        public async Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId)
        {
            var list = await _repository.GetListAsync(t => t.ProductID == productId, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<TechnicalDrawings>, List<SelectTechnicalDrawingsDto>>(list.ToList());

            return new SuccessDataResult<IList<SelectTechnicalDrawingsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateAsync(UpdateTechnicalDrawingsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.RevisionNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateTechnicalDrawingsDto, TechnicalDrawings>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectTechnicalDrawingsDto>(ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(mappedEntity));
        }
    }
}
