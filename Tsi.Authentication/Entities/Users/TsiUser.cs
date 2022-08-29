using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Authentication.Abstract;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Entities.Users
{
    public class TsiUser : ITsiUser, IFullEntityObject
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
