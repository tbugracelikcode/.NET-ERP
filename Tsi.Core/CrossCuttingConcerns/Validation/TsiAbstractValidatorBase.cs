using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.CrossCuttingConcerns.Validation
{
    public abstract class TsiAbstractValidatorBase<TEntity> : AbstractValidator<TEntity>
    {
    }
}
