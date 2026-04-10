using FluentValidation;

namespace crm_api.Validators
{
    public class SalesTypeCreateDtoValidator : AbstractValidator<SalesTypeCreateDto>
    {
        public SalesTypeCreateDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SalesType)
                .NotEmpty()
                .WithMessage(localizationService.GetLocalizedString("SalesTypeCreateDtoValidator.SalesTypeRequired"))
                .MaximumLength(20)
                .WithMessage(localizationService.GetLocalizedString("SalesTypeCreateDtoValidator.SalesTypeTooLong"))
                .Must(BeValidSalesType)
                .WithMessage(localizationService.GetLocalizedString("SalesTypeCreateDtoValidator.SalesTypeInvalid"));

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetLocalizedString("SalesTypeCreateDtoValidator.NameRequired"))
                .MaximumLength(150)
                .WithMessage(localizationService.GetLocalizedString("SalesTypeCreateDtoValidator.NameTooLong"));
        }

        private static bool BeValidSalesType(string? salesType)
        {
            if (string.IsNullOrWhiteSpace(salesType))
            {
                return false;
            }

            return Enum.TryParse<SalesTypeEnum>(salesType.Trim(), ignoreCase: true, out _);
        }
    }
}
