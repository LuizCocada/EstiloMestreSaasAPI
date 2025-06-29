using EstiloMestre.Application.SharedValidators;
using EstiloMestre.Communication.Requests;
using EstiloMestre.Exceptions.ExceptionsBase;
using FluentValidation;

namespace EstiloMestre.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NAME_EMPTY);
        RuleFor(r => r.Email).NotEmpty().WithMessage(ResourceMessagesExceptions.EMAIL_EMPTY);
        RuleFor(r => r.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(r => string.IsNullOrWhiteSpace(r.Email) is false,
            () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesExceptions.EMAIL_INVALID);
            });
        RuleFor(r => r.Phone)
            .SetValidator(new PhoneValidator<RequestRegisterUserJson>());
    }
}