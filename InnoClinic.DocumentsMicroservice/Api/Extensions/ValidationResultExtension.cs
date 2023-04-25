using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Api.Extensions;

public static class ValidationResultExtension
{
    public static void HandleValidationResult(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return;
        var resultAsDictionary = validationResult.ToDictionary();
        var result = JsonConvert.SerializeObject(resultAsDictionary);
        throw new ValidationException(result);
    }
}
