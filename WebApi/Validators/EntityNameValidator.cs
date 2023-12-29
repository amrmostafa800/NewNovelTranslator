using FluentValidation;
using WebApi.DTOs;

namespace WebApi.Validators
{
	public class EntityNameValidator : AbstractValidator<EntityNameDto>
	{
		public EntityNameValidator()
		{
			RuleFor(x => x.EntityNames).NotNull().WithMessage("You Must Send EntityNames");
			RuleFor(x => x.EntityNames).NotEmpty().WithMessage("You Must Send EntityNames");
			RuleForEach(x => x.EntityNames).Must(entity => !string.IsNullOrEmpty(entity.EnglishName)).WithMessage("EnglishName must not be null or empty for each item in EntityNames");
		}
	}
}
