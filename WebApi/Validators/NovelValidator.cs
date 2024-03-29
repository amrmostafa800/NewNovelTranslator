﻿using FluentValidation;
using WebApi.DTOs;

namespace WebApi.Validators;

public class NovelValidator : AbstractValidator<CreateNovelDto>
{
    public NovelValidator()
    {
        RuleFor(x => x.NovelName).NotNull().WithMessage("You Must Send Novel Name");
        RuleFor(x => x.NovelName).Length(1, 200).WithMessage("Name Must Have At Last 1 char And Max 200 char");
    }
}