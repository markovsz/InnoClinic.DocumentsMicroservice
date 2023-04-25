using Domain.Enums;
using Domain.RequestParameters;
using FluentValidation;

namespace Application.Validators;

public class DocumentParametersValidator : AbstractValidator<DocumentParameters>
{
	public DocumentParametersValidator()
	{
		RuleFor(e => e.PartitionName).IsEnumName(typeof(DocumentType));
        RuleFor(e => e.DocumentName).Matches("^[0-9a-z]+\\.[0-9a-z]+").NotEmpty();
    }
}
