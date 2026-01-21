using AOps.Application.UseCases.LoginUsers;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AOps.Application.DTOs;
using AOps.Application.DTOs.CustomerLogins;
using AOps.Application.UseCases.LoginCustomers;

namespace AOps.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Returns Login.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return View(logindto);
            }

            var command = new LoginCommand(logindto.Username,logindto.Password);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");
                return View("~/Views/Admin/Index.cshtml", logindto); // Show form again with error
            }

            // Redirect after successful login
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerLogin(LoginRequestDto logindto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return View("~/Views/Home/Index.cshtml",logindto);
            }

            var command = new LoginRequestCommand(logindto);

            var result = await _mediator.Send(command);

            if (result.Success)
            {

                return RedirectToAction("Index", "Customers");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Login failed.");
                return View("~/Views/Home/Index.cshtml", logindto);

            }
        }
    }
}
