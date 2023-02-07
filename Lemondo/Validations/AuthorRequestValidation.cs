using Lemondo.ClientClass;
using FluentValidation;
using Lemondo.Requestes;

namespace DIMVC.Validations
{
    public class AuthorRequestValidation : AbstractValidator<AuthorRequest>
    {
        public AuthorRequestValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.YearOfBirth)
                .NotEmpty();
        }
    }
}
