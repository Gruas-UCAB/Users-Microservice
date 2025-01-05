using FluentValidation;
using UsersMicroservice.src.user.application.commands.create_user.types;

namespace UsersMicroservice.src.user.infrastructure.validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(2)
                .WithMessage("Name must not be less than 2 characters")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required")
                .MinimumLength(10)
                .WithMessage("Phone must not be less than 10 characters")
                .MaximumLength(15)
                .WithMessage("Phone must not exceed 15 characters");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required")
                .MinimumLength(2)
                .WithMessage("Role must not be less than 2 characters")
                .MaximumLength(50)
                .WithMessage("Role must not exceed 50 characters");

            RuleFor(x => x.Department)
                .NotEmpty()
                .WithMessage("Department is required")
                .MinimumLength(2)
                .WithMessage("Department must not be less than 2 characters")
                .MaximumLength(50)
                .WithMessage("Department must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must not be less than 8 characters");
        }
    }
}
