using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.CreationEntites;
using Tsi.Core.Entities.DataConcurrencyEntities;
using Tsi.Core.Entities.DeleterEntities;
using Tsi.Core.Entities.ModifierEntities;

namespace Tsi.Core.Entities.Auditing
{
    public interface IFullEntityObject : ICreationEntity, IModificationEntity, IDeletionEntity, ISoftDelete, IDataConcurrencyStamp,IEntity
    {
    }
}
