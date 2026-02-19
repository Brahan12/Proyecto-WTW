using FluentValidation;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Validators
{
    public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusRequestDto>
    {
        public UpdateTaskStatusValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .Must(BeValidStatus).WithMessage("Estado inválido. Permitidos: Pending, InProgress, Done.");
        }

        private bool BeValidStatus(string status)
        {
            return status == "Pending" || status == "InProgress" || status == "Done";
        }
    }
}
