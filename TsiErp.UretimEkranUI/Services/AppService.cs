using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Fatek.CommunicationCore;
using TsiErp.UretimEkranUI.Models;
using static TsiErp.UretimEkranUI.Utilities.EnumUtilities.States;

namespace TsiErp.UretimEkranUI.Services
{
    public class AppService
    {
        public Guid EmployeeID { get; set; } = Guid.Parse("d71be8fe-07ce-4ff0-940f-f6d778c16181");

        public static OperationDetailDto CurrentOperation { get; set; } = new OperationDetailDto();

        public static OperationAdjustmentDto OperationAdjustment { get; set; } = new OperationAdjustmentDto();

        public static FatekCommunication FatekCommunication { get; set; }

        public static ParametersTable ProgramParameters { get; set; } = new ParametersTable();

        public static AdjustmentState AdjustmentState { get; set; }

    }
}
