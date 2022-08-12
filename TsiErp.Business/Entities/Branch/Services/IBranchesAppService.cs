using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    public interface IBranchesAppService : ICrudAppService<Branches, SelectBranchesDto, ListBranchesDto, CreateBranchesDto, UpdateBranchesDto>
    {
    }
}
