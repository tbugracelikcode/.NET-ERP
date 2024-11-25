using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos;

namespace TsiErp.Business.Entities.CompanyCheck.Validations
{
    public class CreateCompanyChecksValidator : TsiAbstractValidatorBase<CreateCompanyChecksDto>
    {
        public CreateCompanyChecksValidator()
        {

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountCardID");
            RuleFor(x => x.BankAccountID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorBankAccountID");


        }
    }
}
