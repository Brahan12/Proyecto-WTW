using FluentValidation;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskRequestDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título de la tarea es obligatorio.")
                .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("La tarea debe tener un usuario asignado.");
        }
    }
}
