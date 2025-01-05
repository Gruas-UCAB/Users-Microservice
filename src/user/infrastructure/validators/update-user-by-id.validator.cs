using FluentValidation;
using UsersMicroservice.src.user.application.commands.update_user.types;
using UsersMicroservice.src.user.infrastructure.dto;

namespace UsersMicroservice.src.user.infrastructure.validators
{
    public class UpdateUserByIdValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserByIdValidator()
        {

            RuleFor(x => x.Name)
                .MinimumLength(2)
                .WithMessage("Name must not be less than 2 characters")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Phone)
                .MinimumLength(10)
                .WithMessage("Phone must not be less than 10 characters")
                .MaximumLength(15)
                .WithMessage("Phone must not exceed 15 characters");
        }
    }
}
