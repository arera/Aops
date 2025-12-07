using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public class CustomerDropDownHandler : IRequestHandler<CustomerDropDownCommand, List<DTOs.Customer.CustomerDropdownDto>>
    {
        private ICustomerRepository _repo;

        public CustomerDropDownHandler(ICustomerRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<DTOs.Customer.CustomerDropdownDto>> Handle(CustomerDropDownCommand request, CancellationToken cancellationToken)
        {
            var customers = await _repo.GetCustomerDropdown(cancellationToken);
            return customers.Select(customer => new DTOs.Customer.CustomerDropdownDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName
            }).ToList();
        }
    }
}
