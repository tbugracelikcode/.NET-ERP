using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;

namespace TsiErp.Business.Entities.TestManagement.City.Services
{

        public interface ICitiesAppService : ICrudAppService<SelectCitiesDto, ListCitiesDto, CreateCitiesDto, UpdateCitiesDto, ListCitiesParameterDto>
        {
        }
}
