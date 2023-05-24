using FluentValidation;
using InnoClinic.SharedModels.DTOs.Documents.Incoming;

namespace Application.Validators;

public class DocumentIncomingDtoValidator : AbstractValidator<DocumentIncomingDto>
{
	public DocumentIncomingDtoValidator()
	{
		RuleFor(e => e.Name).Matches("^[0-9a-zA-Z-]+\\.[0-9a-zA-Z]+").NotEmpty();
	}
}
