using AOps.Application.DTOs;
using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.UseCases.RegisterOrglevels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AOps.Application.UseCases.ChangePassword;

namespace AOps.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IMediator mediator, ILogger<AdminController> logger) // ✅ Correct constructor
        {
            _mediator = mediator;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserRoles(OrglevelsDto.RegisterOrgLevelsCommand dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return View(dto);
            }

            var command = new RegisterOrgLevelsCommand(dto.Name,dto.Email,dto.Role,dto.Mobile,dto.PasswordHash);

            try
            {
                int customerId = await _mediator.Send(command, cancellationToken);
                _logger.LogInformation("Roles created successfully with ID {UserId}.", customerId);

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

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok("Password changed successfully.");
        }
    }
}
