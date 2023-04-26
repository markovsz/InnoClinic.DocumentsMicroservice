using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class PartitionNameValidator : AbstractValidator<string>
{
	public PartitionNameValidator()
	{
        RuleFor(e => e).IsEnumName(typeof(DocumentType));
    }
}
