using FluentValidation;
using NoteMiniApi.Models;

namespace NoteMiniApi.Validators
{
    public class NoteValidator : AbstractValidator<Note>
    {
        public NoteValidator()
        {
            RuleFor(x => x.Content)
            .NotEmpty().NotNull().MinimumLength(5).WithMessage("Minimum length is 5 characters");
        }
    }
}