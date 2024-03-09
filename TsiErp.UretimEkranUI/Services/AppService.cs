using TsiErp.UretimEkranUI.Models;
using static TsiErp.UretimEkranUI.Utilities.EnumUtilities.States;

namespace TsiErp.UretimEkranUI.Services
{
    public class AppService
    {
        public static Guid EmployeeID { get; set; }

        public static string EmployeeName { get; set; }

        public static string EmployeePassword { get; set; }

        public static OperationDetailDto CurrentOperation { get; set; } = new OperationDetailDto();

        public static OperationAdjustmentDto OperationAdjustment { get; set; } = new OperationAdjustmentDto();

        public static ParametersTable ProgramParameters { get; set; } = new ParametersTable();
        public static OperationHaltReasonsTable OperationHaltReasons { get; set; } = new OperationHaltReasonsTable();

        public static AdjustmentState AdjustmentState { get; set; }

    }
}
