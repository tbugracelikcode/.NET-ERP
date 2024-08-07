using TsiErp.UretimEkranUI.Models;
using static TsiErp.UretimEkranUI.Utilities.EnumUtilities.States;

namespace TsiErp.UretimEkranUI.Services
{
    public class AppService
    {
        public static Guid EmployeeID { get; set; }

        public static string EmployeeName { get; set; }

        public static string EmployeePassword { get; set; }

        public static OperationDetailTable CurrentOperation { get; set; } = new OperationDetailTable();

        public static OperationAdjustmentTable OperationAdjustment { get; set; } = new OperationAdjustmentTable();

        public static ParametersTable ProgramParameters { get; set; } = new ParametersTable();
        public static OperationHaltReasonsTable OperationHaltReasons { get; set; } = new OperationHaltReasonsTable();

        public static AdjustmentState AdjustmentState { get; set; }

    }
}
