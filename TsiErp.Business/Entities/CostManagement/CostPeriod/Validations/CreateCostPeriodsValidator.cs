
using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;

namespace TsiErp.Business.Entities.CostManagement.CostPeriod.Validations
{
    public class CreateCostPeriodsValidator : TsiAbstractValidatorBase<CreateCostPeriodsDto>
    {
        public CreateCostPeriodsValidator()
        {
            RuleFor(x => x.CalculationStartDate)
               .NotEmpty()
               .WithMessage("ValidatorCalculationStartDateEmpty");

            RuleFor(x => x.CalculationEndDate)
               .NotEmpty()
               .WithMessage("ValidatorCalculationEndDateEmpty")
               .Must((model, calculationEndDate) => calculationEndDate > model.CalculationStartDate)
               .WithMessage("ValidatorCalculationEndDateBeforeCalculationStartDate");


            RuleFor(x => x.TotalProductionQuantity)
                .NotNull()
                .WithMessage("ValidatorTotalProductionQuantityEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorTotalProductionQuantityMin");

            RuleFor(x => x.AnnualWorkingDay)
                .NotNull()
                .WithMessage("ValidatorAnnualWorkingDayEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualWorkingDayMin");

            RuleFor(x => x.Shift)
                .NotNull()
                .WithMessage("ValidatorShiftEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorShiftMin");

            RuleFor(x => x.DailyGrossWorkingHours)
                .NotNull()
                .WithMessage("ValidatorDailyGrossWorkingHoursEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorDailyGrossWorkingHoursMin");

            RuleFor(x => x.AnnualWorkingHours)
                .NotNull()
                .WithMessage("ValidatorAnnualWorkingHoursEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualWorkingHoursMin");

            RuleFor(x => x.MontlyShiftWorkingHour)
                .NotNull()
                .WithMessage("ValidatorMontlyShiftWorkingHourEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorMontlyShiftWorkingHourMin");

            RuleFor(x => x.ShiftWorkingTime)
                .NotNull()
                .WithMessage("ValidatorShiftWorkingTimeEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorShiftWorkingTimeMin");

            RuleFor(x => x.MonthlyRentAndDues)
                .NotNull()
                .WithMessage("ValidatorMonthlyRentAndDuesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorMonthlyRentAndDuesMin");

            RuleFor(x => x.TotalRentedArea)
                .NotNull()
                .WithMessage("ValidatorTotalRentedAreaEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorTotalRentedAreaMin");

            RuleFor(x => x.EURExchangeRate)
                .NotNull()
                .WithMessage("ValidatorEURExchangeRateEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorEURExchangeRateMin");

            RuleFor(x => x.GeneralManagementExpenses)
                .NotNull()
                .WithMessage("ValidatorGeneralManagementExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorGeneralManagementExpensesMin");

            RuleFor(x => x.EmployeeSocialAssistanceExpenses)
                .NotNull()
                .WithMessage("ValidatorEmployeeSocialAssistanceExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorEmployeeSocialAssistanceExpensesMin");

            RuleFor(x => x.WorkClothesExpenses)
                .NotNull()
                .WithMessage("ValidatorWorkClothesExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorWorkClothesExpensesMin");

            RuleFor(x => x.MarketingExpense)
                .NotNull()
                .WithMessage("ValidatorMarketingExpenseEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorMarketingExpenseMin");

            RuleFor(x => x.ServiceExpense)
                .NotNull()
                .WithMessage("ValidatorServiceExpenseEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorServiceExpenseMin");

            RuleFor(x => x.FoodExpense)
                .NotNull()
                .WithMessage("ValidatorFoodExpenseEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorFoodExpenseMin");

            RuleFor(x => x.WorkplaceInsurance)
                .NotNull()
                .WithMessage("ValidatorWorkplaceInsuranceEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorWorkplaceInsuranceMin");

            RuleFor(x => x.FinancialExpense)
                .NotNull()
                .WithMessage("ValidatorFinancialExpenseEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorFinancialExpenseMin");

            RuleFor(x => x.ElectricityAndWaterExpense)
                .NotNull()
                .WithMessage("ValidatorElectricityAndWaterExpenseEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorElectricityAndWaterExpenseMin");

            RuleFor(x => x.VariousIndirectMaterials)
                .NotNull()
                .WithMessage("ValidatorVariousIndirectMaterialsEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorVariousIndirectMaterialsMin");

            RuleFor(x => x.VariousDirectMaterials)
                .NotNull()
                .WithMessage("ValidatorVariousDirectMaterialsEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorVariousDirectMaterialsMin");

            RuleFor(x => x.MaterialPurchaseTotal)
                .NotNull()
                .WithMessage("ValidatorMaterialPurchaseTotalEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorMaterialPurchaseTotalMin");

            RuleFor(x => x.AnnualGeneralManagementExpenses)
                .NotNull()
                .WithMessage("ValidatorAnnualGeneralManagementExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualGeneralManagementExpensesMin");

            RuleFor(x => x.FinishedProductStorageExpenses)
                .NotNull()
                .WithMessage("ValidatorFinishedProductStorageExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorFinishedProductStorageExpensesMin");

            RuleFor(x => x.AnnualDistributionExpenseRate)
                .NotNull()
                .WithMessage("ValidatorAnnualDistributionExpenseRateEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualDistributionExpenseRateMin");

            RuleFor(x => x.AnnualProductionExpenses)
                .NotNull()
                .WithMessage("ValidatorAnnualProductionExpensesEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualProductionExpensesMin");

            RuleFor(x => x.AnnualTurnover)
                .NotNull()
                .WithMessage("ValidatorAnnualTurnoverEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorAnnualTurnoverMin");

            RuleFor(x => x.SGA)
                .NotNull()
                .WithMessage("ValidatorSGAEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorSGAMin");

        }
    }
}
