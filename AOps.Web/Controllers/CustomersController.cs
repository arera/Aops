using AOps.Application.Common.Utility;
using AOps.Application.DTOs;
using AOps.Application.DTOs.Customer;
using AOps.Application.DTOs.CustomerContracts;
using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.DTOs.CustomerLogins;
using AOps.Application.DTOs.CustomerSites;
using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.DTOs.Vendor;
using AOps.Application.UseCases.ChangePassword;
using AOps.Application.UseCases.CustomerContracts;
using AOps.Application.UseCases.CustomerDashboards;
using AOps.Application.UseCases.CustomerSites;
using AOps.Application.UseCases.CustomerTicket;
using AOps.Application.UseCases.LoginCustomers;
using AOps.Application.UseCases.LookUp;
using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Application.UseCases.Vendor;
using AOps.Domain.Entities;
using AOps.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCustomerPassword(CustomerChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View("ChangePassword", dto);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                TempData["Error"] = "Session expired. Please log in again.";
                return RedirectToAction("Index", "Home");
            }

            dto.CustomerId = userId;
            dto.ipAddress= HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _mediator.Send(new CustomerChangePasswordCommand(dto));

            if (result is null || !result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result?.ErrorMessage ?? "Failed to change password.");
                return View("ChangePassword", dto);
            }

            // Sign user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> CustomerTickets()
        {
            Guid CustomerId = User.GetCustomerId();

            var tickets = await _mediator.Send(new GetAllCustomerTicketCommand(CustomerId));

            return View(tickets);
        }
        [HttpGet]
        public async Task<IActionResult> AddTicket()
        {
            Guid CustomerId = User.GetCustomerId();
            var dto = new CreateTicketDto
            {
                SiteList = await _mediator.Send(new SiteSelectCommand(CustomerId)),
                VehicleIssueTypes = Enum.GetValues(typeof(VehicleTicketIssueType))
            .Cast<VehicleTicketIssueType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList(),
             ManpowerIssueTypes = Enum.GetValues(typeof(ManpowerTicketIssueType))
            .Cast<ManpowerTicketIssueType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList()
            };

            return PartialView("_AddTicket", dto);
        }
        [HttpGet]
        public async Task<IActionResult> GetVehicleBySiteId(Guid SiteId)
        {
            var vehicles = await _mediator.Send(new VehicleSelectCommand(SiteId));
            return Json(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeBySiteId(Guid SiteId)
        {
            var employees = await _mediator.Send(new EmployeeBySiteCommand(SiteId));
            return Json(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(CreateTicketDto dto)
        {
            try
            {
                var command = new CreateTicketCommand(dto);
                var id = await _mediator.Send(command);

                return Json(new { success = true });
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Populate ModelState manually
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return PartialView("_AddTicket", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid customerId = User.GetCustomerId();
            var vehicles = await _mediator.Send(new DashboardvehicleCommand(customerId));
            var employees = await _mediator.Send(new DashboardEmployeeCommand(customerId));

            var viewModel = new DashboardViewdto
            {
                Vehicles = vehicles,
                Employees = employees
            };

            return View(viewModel);
        }


    }
}
