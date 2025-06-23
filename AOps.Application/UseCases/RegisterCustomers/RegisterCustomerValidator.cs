using AOps.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public class RegisterCustomerValidator : AbstractValidator<RegisterCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.");

            RuleFor(x => x.PrimaryMobile)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Mobile number must be between 10 and 15 digits.");

            RuleFor(x => x.GST)
                .Matches(@"^[A-Za-z0-9]{10,20}$").When(x => !string.IsNullOrEmpty(x.GST))
                .WithMessage("GST number must be alphanumeric and between 10 to 20 characters.");
        }

       
    }

}
