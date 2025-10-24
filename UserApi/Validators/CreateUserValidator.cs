namespace UserApi.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(15);
    }
}
