using FluentValidation;
using AOps.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class RegisterOrglevelsValidator : AbstractValidator<RegisterOrgLevelsCommand>
    {
        private readonly IOrgLevelRepository _orgLevelRepository;

        public RegisterOrglevelsValidator(IOrgLevelRepository orglevelRepository)
        {
            _orgLevelRepository = orglevelRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists.");

            RuleFor(x => x.Mobile)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^\+?\d{10,12}$").WithMessage("Mobile number must be between 10 and 15 digits.");

            RuleFor(x => x.passwordhash)
                .NotEmpty().WithMessage("Password is required.");
                
        }
        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            // Check repository if email exists
            return !await _orgLevelRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}
