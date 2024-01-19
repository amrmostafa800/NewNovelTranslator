using FluentValidation;
using WebApi.DTOs;

namespace WebApi.Validators;

public class NovelUserValidator : AbstractValidator<AddNovelUserDto>
{
    public NovelUserValidator()
    {
        RuleFor(x => x.UserName).NotNull().WithMessage("Username Is Required");
        RuleFor(x => x.UserName).Length(1, 200).WithMessage("Username Must Have At Last 1 char And Max 200 char");
        RuleFor(x => x.NovelId).NotNull().WithMessage("NovelId Is Required");
    }
}