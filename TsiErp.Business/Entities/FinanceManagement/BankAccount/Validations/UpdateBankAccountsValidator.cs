using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.BankAccount.Validations
{
    public class UpdateBankAccountsValidator : TsiAbstractValidatorBase<UpdateBankAccountsDto>
    {
        public UpdateBankAccountsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(200)
                .WithMessage("ValidatorNameMaxLenght");

        }
    }
}
