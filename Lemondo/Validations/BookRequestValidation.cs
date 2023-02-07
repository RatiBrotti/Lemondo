using FluentValidation;
using Lemondo.ClientClass;

namespace Lemondo.Validations
{
    public class BookRequestValidation : AbstractValidator<BookRequest>
    {
        public BookRequestValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.PublicationDate)
                .NotEmpty();
        }
    }
}
