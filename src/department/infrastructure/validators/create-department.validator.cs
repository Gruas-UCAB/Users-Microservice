using FluentValidation;
using UsersMicroservice.src.department.application.commands.create_department.types;


namespace UsersMicroservice.src.department.infrastructure.validators
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(2)
                .WithMessage("Name must not be less than 2 characters")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");
        }
    }
}
