using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos
{
    public class ListEmployeeOperationsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Puantaj ID
        /// </summary>
        public Guid EmployeeScoringID { get; set; }
        /// <summary>
        /// Puantaj Satır ID
        /// </summary>
        public Guid EmployeeScoringLineID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Personel ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid TemplateOperationID { get; set; }
        /// <summary>
        /// Şablon Operasyon Adı
        /// </summary>
        public string TemplateOperationName { get; set; }
        /// <summary>
        /// Şablon Operasyon İş Puanı
        /// </summary>
        public int TemplateOperationWorkScore { get; set; }
        /// <summary>
        /// Puan
        /// </summary>
        public int Score_ { get; set; }
        /// <summary>
        /// Hesaplamaya Dahil Edildi mi?
        /// </summary>
        public bool IsCalculated { get; set; }
    }
}
