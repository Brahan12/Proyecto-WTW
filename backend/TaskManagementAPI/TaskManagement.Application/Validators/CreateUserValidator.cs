using FluentValidation;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("El nombre del usuario es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no puede superar 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.")
                .MaximumLength(150).WithMessage("El correo no puede superar 150 caracteres.");
        }
    }
}
