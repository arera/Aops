using AOps.Application.UseCases.RegisterCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AOps.Application.DTOs.Customer;

namespace AOps.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IMediator mediator, ILogger<CustomersController> logger) // ✅ Correct constructor
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Create()
        {
            // Show empty form for creating customer
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for CreateCustomerDto.");
                return View(dto);
            }

            var command = new RegisterCustomerCommand(dto.Name,dto.Email,dto.Address,dto.PrimaryMobile,dto.SecondaryMobile,dto.GST);

            try
            {
                int customerId = await _mediator.Send(command, cancellationToken);
                _logger.LogInformation("Customer created successfully with ID {CustomerId}.", customerId);

                return RedirectToAction("Details", new { id = customerId });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while creating customer: {Errors}", ex.Errors);

                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating customer.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                return View(dto);
            }
        }


        public IActionResult Details(int id)
        {
            // Stub: Normally you'd call a query handler to get customer by id
            ViewBag.CustomerId = id;
            return View(); // Implement the actual view as needed
        }

}
}
