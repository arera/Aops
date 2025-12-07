using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VendorContracts
{
    public class AddVendorContractValidator : AbstractValidator<AddCustomerContractDto>
    {

        public AddVendorContractValidator()
        {

            RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor is required.");

            RuleFor(x => x.ContractDetails)
                .NotEmpty().WithMessage("Contract details are required.")
                .MaximumLength(5000).WithMessage("Contract details must not exceed 5000 characters.");

            RuleFor(x => x.ContractType)
                .NotEmpty().WithMessage("Contract type is required.")
                .MaximumLength(50).WithMessage("Contract type must not exceed 50 characters.")
                .Must(type => type == "Vehicle" || type == "Employee")
                .WithMessage("Contract type must be either 'Vehicle' or 'Employee'.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.");

            RuleFor(x => x.ContractValue)
                .NotNull().WithMessage("Contract value is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Contract value cannot be negative.");

            When(x => x.ContractType == "Vehicle", () =>
            {
                RuleFor(x => x.VehiclesAgreed)
                    .GreaterThanOrEqualTo(0).WithMessage("Vehicles agreed cannot be negative.");

                RuleFor(x => x.VehiclesUsed)
                    .GreaterThanOrEqualTo(0).WithMessage("Vehicles used cannot be negative.")
                    .LessThanOrEqualTo(x => x.VehiclesAgreed).WithMessage("Vehicles used cannot exceed agreed number.");
            });

            When(x => x.ContractType == "Employee", () =>
            {
                RuleFor(x => x.StaffAgreed)
                    .GreaterThanOrEqualTo(0).WithMessage("Staff agreed cannot be negative.");

                RuleFor(x => x.StaffUsed)
                    .GreaterThanOrEqualTo(0).WithMessage("Staff used cannot be negative.")
                    .LessThanOrEqualTo(x => x.StaffAgreed).WithMessage("Staff used cannot exceed agreed number.");
            });


            // Optional: AgreementDocument can have size/type checks if needed
            When(x => x.AgreementDocument != null, () =>
            {
                RuleFor(x => x.AgreementDocument.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024) // 5 MB
                    .WithMessage("Agreement document must be less than 5MB.");
            });
        }

    }
}
