using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos
{
    public class ListEmployeeOperationsParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Puantaj Satır ID
        /// </summary>
        public Guid EmployeeScoringLineID { get; set; }
    }
}
