using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.UseCases.RegisterOrglevels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AOps.Application.UseCases.ChangePassword;
using AOps.Application.UseCases.LoginUsers;
using AOps.Application.DTOs;
using AOps.Web.Models;
using Microsoft.AspNetCore.Authorization;


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
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return View(logindto);
            }

            var command = new LoginCommand(logindto.Username, logindto.Password);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");
                return View("Index", logindto); // Show form again with error
            }

            // Redirect after successful login
            return RedirectToAction("Roles", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserRoles(RolesPageViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                model.ExistingRoles = await _mediator.Send(new FetchAllOrglevelsCommand());
                return View("Roles",model);
            }

            var command = new RegisterOrgLevelsCommand(model.NewUser.Name, model.NewUser.Email, model.NewUser.Role, model.NewUser.Mobile, model.NewUser.Password);

            try
            {
                int customerId = await _mediator.Send(command, cancellationToken);
                _logger.LogInformation("Roles created successfully with ID {UserId}.", customerId);
                 return RedirectToAction("Roles");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while creating customer: {Errors}", ex.Errors);

                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                model.ExistingRoles = await _mediator.Send(new FetchAllOrglevelsCommand());
                return View("Roles",model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating customer.");

                // Check if the exception contains "Email already exists."
                if (ex.Message.Contains("Email already exists"))
                {
                    ModelState.AddModelError("NewUser.Email", ex.Message); // Show error under Email field
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }
                model.ExistingRoles = await _mediator.Send(new FetchAllOrglevelsCommand());
                return View("Roles",model); // Return Roles view to reload modal
            }
        }


        public async Task<IActionResult> Roles()
        {
            var orgLevels = await _mediator.Send(new FetchAllOrglevelsCommand());

            var model = new RolesPageViewModel
            {
                ExistingRoles = orgLevels,
                NewUser = new RegisterOrgLevelsDot()
            };

            return View(model);
        }

        [HttpGet]
        
        public async Task<ActionResult<GetOrgLevelsDot>> GetAllOrgUsersByID(Guid Userid)
        {
            var orgUsers = await _mediator.Send(new FetchAllOrglevelByIdCommand(Userid));

            if (orgUsers == null )
                return NotFound("No users found.");

            return Ok(orgUsers);
        }

        // For Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRoles(GetOrgLevelsDot dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //

            var command = new UpdateOrglevelsCommand(dto.Name, dto.Email, dto.Role, dto.Mobile,dto.UserID, dto.IsActive);

            try
            {
                await _mediator.Send(command, cancellationToken);
                return Ok(new { success = true, message = "User updated successfully" });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
                return BadRequest(new { success = false, errors });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Unexpected error occurred." });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Guid UserID, string NewPassword)
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
                return BadRequest("Password cannot be empty.");

            await _mediator.Send(new UpdatePasswordOrglevelsCommand(UserID, NewPassword));
            return Ok();
        }
    }
}
