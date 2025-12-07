using AOps.Application.Common.Utility;
using AOps.Application.DTOs;
using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.DTOs.AOESite;
using AOps.Application.DTOs.Customer;
using AOps.Application.DTOs.CustomerContracts;
using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.DTOs.CustomerSites;
using AOps.Application.DTOs.DropDown;
using AOps.Application.DTOs.Employees;
using AOps.Application.DTOs.Expense;
using AOps.Application.DTOs.Vehicle;
using AOps.Application.DTOs.VehicleDocuments;
using AOps.Application.DTOs.Vendor;
using AOps.Application.UseCases.AdminCustomerTicket;
using AOps.Application.UseCases.AdminDashboards;
using AOps.Application.UseCases.AOESites;
using AOps.Application.UseCases.ChangePassword;
using AOps.Application.UseCases.CustomerContracts;
using AOps.Application.UseCases.CustomerDashboards;
using AOps.Application.UseCases.CustomerSites;
using AOps.Application.UseCases.CustomerTicket;
using AOps.Application.UseCases.EmployeeMasters;
using AOps.Application.UseCases.LoginUsers;
using AOps.Application.UseCases.LookUp;
using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.UseCases.RegisterOrglevels;
using AOps.Application.UseCases.SiteExpense;
using AOps.Application.UseCases.UploadVehicleDocuments;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Application.UseCases.Vendor;
using AOps.Application.UseCases.VendorContracts;
using AOps.Domain.Entities;
using AOps.Domain.Enums;
using AOps.Web.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics.Contracts;
using System.Security.Claims;


namespace AOps.Web.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IMediator mediator, ILogger<AdminController> logger) // ✅ Correct constructor
        {
            _mediator = mediator;
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }
        public IActionResult AddVendor()
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
                return View("Roles", model);
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
                return View("Roles", model);
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
                return View("Roles", model); // Return Roles view to reload modal
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

            if (orgUsers == null)
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

            var command = new UpdateOrglevelsCommand(dto.Name, dto.Email, dto.Role, dto.Mobile, dto.UserID, dto.IsActive);

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

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePasswordRequestDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                TempData["Error"] = "Session expired. Please log in again.";
                return RedirectToAction("Index", "Home");
            }

            dto.UserId = userId;

            var result = await _mediator.Send(new ChangePasswordCommand(dto.UserId, dto.CurrentPassword, dto.NewPassword));

            if (result is null || !result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result?.ErrorMessage ?? "Failed to change password.");
                return View("ChangePassword", dto);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["Success"] = "Password changed successfully.";
            // return RedirectToAction("Index", "Home");
            return View("ChangePassword", new ChangePasswordRequestDto());
        }

        /// This Section is for Add Customer//////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(CreateCustomerDto dto, CancellationToken cancellationToken)
        {
            dto.CustomerId = Guid.Empty;
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for CreateCustomerDto.");

                return BadRequest(ModelState);
            }

            var command = new RegisterCustomerCommand(
                dto.Name,
                dto.Email,
                dto.Address,
                dto.PrimaryMobile,
                dto.SecondaryMobile,
                dto.GST
            );

            try
            {
                Guid customerId = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Customer created successfully with ID {CustomerId}.", customerId);

                return Ok(new
                {
                    success = true,
                    customerId
                });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .SelectMany(kvp => kvp.Value.Errors.Select(err => new
                                       {
                                           field = kvp.Key,
                                           message = err.ErrorMessage
                                       }))
                                       .ToList();

                return BadRequest(new { success = false, errors });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }

                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .SelectMany(kvp => kvp.Value.Errors.Select(err => new
                                       {
                                           field = kvp.Key,
                                           message = err.ErrorMessage
                                       }))
                                       .ToList();

                return BadRequest(new { success = false, errors });
            }
        }



        public async Task<IActionResult> Customers()
        {
            List<CreateCustomerDto> Customers = new List<CreateCustomerDto>();
            var Allcustomers = await _mediator.Send(new FetchAllCustomerCommand());
            Customers = Allcustomers;

            return View(Customers);
        }

        [HttpGet]
        public async Task<ActionResult<CreateCustomerDto>> GetAllCustomerByID(Guid Userid)
        {
            var orgUsers = await _mediator.Send(new FetchAllCustomerByIdCommand(Userid));

            if (orgUsers == null)
                return NotFound("No Customer found.");

            return Ok(orgUsers);
        }

        // For Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomer(CreateCustomerDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //

            var command = new UpdateCustomerCommand(dto.CustomerId, dto.Name, dto.Email, dto.PrimaryMobile, dto.SecondaryMobile, dto.GST, dto.IsActive, dto.Address);

            try
            {
                await _mediator.Send(command, cancellationToken);
                return Ok(new { success = true, message = "User updated successfully" });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .SelectMany(kvp => kvp.Value.Errors.Select(err => new
                                       {
                                           field = kvp.Key,
                                           message = err.ErrorMessage
                                       }))
                                       .ToList();

                return BadRequest(new { success = false, errors });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }

                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .SelectMany(kvp => kvp.Value.Errors.Select(err => new
                                       {
                                           field = kvp.Key,
                                           message = err.ErrorMessage
                                       }))
                                       .ToList();

                return BadRequest(new { success = false, errors });
            }
        }

        // Vendor Section ///////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Vendor()
        {
            List<GetVendorDto> vendor = new List<GetVendorDto>();
            var Allvendors = await _mediator.Send(new FetchAllVendorCommand());
            vendor = Allvendors;

            return View(vendor);
        }

        [HttpGet]
        public async Task<IActionResult> VendorDropDown()
        {
            List<VendorDropdownDto> vendordd = new List<VendorDropdownDto>();
            var Allvendors = await _mediator.Send(new VendorDropDownCommand());
            vendordd = Allvendors;

            return Json(vendordd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVendor(AddVendorDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return RedirectToAction("Vendor", "Admin");
            }

            var command = new AddVendorCommand(model.VendorName, model.VendorMobile, model.VendorEmail, model.VendorAddress);

            try
            {
                int vendorId = await _mediator.Send(command, cancellationToken);
                _logger.LogInformation("Vendor created successfully with ID {UserId}.", vendorId);
                return RedirectToAction("Vendor", "Admin");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while creating customer: {Errors}", ex.Errors);

                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return RedirectToAction("Vendor", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating customer.");

                // Check if the exception contains "Email already exists."
                if (ex.Message.Contains("Mobile already exists"))
                {
                    ModelState.AddModelError("Mobile", ex.Message); // Show error under Email field
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }
                return RedirectToAction("Vendor", "Admin"); // Return Roles view to reload modal
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(EditVendorDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for vendor edit.");
                return RedirectToAction("Vendor", "Admin"); // Ideally reload the page with validation error support
            }

            var command = new UpdateVendorCommand(
                model.VendorId,
                model.VendorName,
                model.VendorMobile,
                model.VendorEmail,
                model.VendorAddress,
                model.IsActive
            );

            try
            {
                await _mediator.Send(command, cancellationToken);
                _logger.LogInformation("Vendor updated successfully with ID {VendorId}.", model.VendorId);
                return RedirectToAction("Vendor", "Admin");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while updating vendor: {Errors}", ex.Errors);

                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return RedirectToAction("Vendor", "Admin"); // Or handle through a ViewModel to keep modal open
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while editing vendor.");

                if (ex.Message.Contains("Mobile already exists"))
                {
                    ModelState.AddModelError("VendorMobile", ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }

                return RedirectToAction("Vendor", "Admin");
            }
        }

        //----------------Vendor Contract ------------------------------------------------------//

        [HttpPost]
        public async Task<IActionResult> AddVendorContract([FromForm] Application.DTOs.Vendor.AddCustomerContractDto dto)
        {
            try
            {
                var command = new AddVendorContractCommand(dto);
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

                // Rehydrate VendorList
                dto.VendorList = await _mediator.Send(new VendorDropDownCommand());

                return PartialView("_AddVendorContract", dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> VendorContract()
        {
            var Allvendorcontracts = await _mediator.Send(new GetAllVendorContractCommand());

            return View(Allvendorcontracts);
        }

        [HttpGet]
        public async Task<IActionResult> AddVendorContract()
        {
            var vendors = await _mediator.Send(new VendorDropDownCommand()); // returns List<VendorDropdownDto>

            var model = new Application.DTOs.Vendor.AddCustomerContractDto
            {
                VendorList = vendors.Select(v => new VendorDropdownDto
                {
                    VendorId = v.VendorId,
                    VendorName = v.VendorName
                }).ToList()
            };

            return PartialView("_AddVendorContract", model);
        }

        [HttpGet]
        public async Task<IActionResult> VendorContractById(string contractid)
        {
            var vendorcontracts = await _mediator.Send(new GetVendorContractByIdCommand(contractid));

            return View(vendorcontracts);
        }

        public async Task<IActionResult> EditVendorContract(string contractId)
        {
            var contract = await _mediator.Send(new GetVendorContractByIdCommand(contractId));
            if (contract == null)
            {
                return NotFound("Contract not found.");
            }

            var dto = new EditVendorContractDto
            {
                VendorId = contract.VendorId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractType = contract.ContractType,
                ContractDetails = contract.ContractDetails,
                ContractValue = contract.ContractValue,
                VehiclesAgreed = contract.VehiclesAgreed,
                VehiclesUsed = contract.VehiclesUsed,
                StaffAgreed = contract.StaffAgreed,
                StaffUsed = contract.StaffUsed,
                AgreementDocumentPath = contract.AgreementDocument,
                VendorList = await _mediator.Send(new VendorDropDownCommand())
            };

            return PartialView("_EditVendorContract", dto);
        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateVendorContract([FromForm] EditVendorContractDto dto)
        {
            try
            {
                var command = new UpdateVendorContractCommand(dto);
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

                // Rehydrate VendorList
                dto.VendorList = await _mediator.Send(new VendorDropDownCommand());

                return PartialView("_EditVendorContract", dto);
            }
        }
        /// <summary>
        /// Customer Contract Starts From here/////////////////////////////////////////////////////////////
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CustomerContract()
        {
            var Allcustomercontracts = await _mediator.Send(new GetAllCustomerContractCommand());

            return View(Allcustomercontracts);
        }

        [HttpGet]
        public async Task<IActionResult> AddCustomerContract()
        {
            var customers = await _mediator.Send(new CustomerDropDownCommand());

            var model = new Application.DTOs.CustomerContracts.AddCustomerContractDto
            {
                CustomerList = customers.Select(v => new CustomerDropdownDto
                {
                    CustomerId = v.CustomerId,
                    CustomerName = v.CustomerName
                }).ToList()

                
            };

            return PartialView("_AddCustomerContract",model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerContract([FromForm] Application.DTOs.CustomerContracts.AddCustomerContractDto dto)
        {
            try
            {
                var command = new AddCustomerContractCommand(dto);
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

                // Rehydrate VendorList
                dto.CustomerList = await _mediator.Send(new CustomerDropDownCommand());

                return PartialView("_AddVendorContract", dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> CustomerContractById(string contractid)
        {
            var cutomercontracts = await _mediator.Send(new GetCustomerContractByIdCommand(contractid));

            return View(cutomercontracts);
        }

        public async Task<IActionResult> EditCustomerContract(string contractId)
        {
            var contract = await _mediator.Send(new GetCustomerContractByIdCommand(contractId));
            if (contract == null)
            {
                return NotFound("Contract not found.");
            }

            var dto = new EditCustomerContractDto
            {
                CustomerId = contract.CustomerId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractType = contract.ContractType,
                ContractDetails = contract.ContractDetails,
                ContractValue = contract.ContractValue,
                VehiclesAgreed = contract.VehiclesAgreed,
                VehiclesUsed = contract.VehiclesUsed,
                StaffAgreed = contract.StaffAgreed,
                StaffUsed = contract.StaffUsed,
                AgreementDocumentPath = contract.AgreementDocument,
                CustomerList = await _mediator.Send(new CustomerDropDownCommand())
            };

            return PartialView("_EditCustomerContract",dto);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCustomerContract([FromForm] EditCustomerContractDto dto)
        {
            try
            {
                var command = new UpdateCustomerContractCommand(dto);
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

                // Rehydrate VendorList
                dto.CustomerList = await _mediator.Send(new CustomerDropDownCommand());

                return PartialView("_EditCustomerContract", dto);
            }
        }


        /// <summary>
        /// Vehicle Model Starts From here/////////////////////////////////////////////////////////////
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Vehicle()
        {
            var AllVehicle = await _mediator.Send(new GetAllVehicleCommand());

            return View(AllVehicle);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromForm] AddVehicleDto dto)
        {
            try
            {
                var command = new AddVehicleCommand(dto);
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

                // Rehydrate VendorList
                dto.VendorList = await _mediator.Send(new VendorDropDownCommand());

                return PartialView("_AddVehicle", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddVehicle()
        {
            var vendors = await _mediator.Send(new VendorDropDownCommand()); // returns List<VendorDropdownDto>

            var model = new AddVehicleDto
            {
                VendorList = vendors.Select(v => new VendorDropdownDto
                {
                    VendorId = v.VendorId,
                    VendorName = v.VendorName
                }).ToList()
            };

            return PartialView("_AddVehicle", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditVehicleDetails(Guid VehicleId)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdCommand(VehicleId));
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            var dto = new EditVehicleDto
            {
                VendorId = vehicle.VendorId,
                VehicleId = vehicle.VehicleId,
                VehicleFuelType = vehicle.VehicleFuelType,
                VehicleModel = vehicle.VehicleModel,
                VehicleNumber = vehicle.VehicleNumber,
                VehicleType = vehicle.VehicleType,
                AmbulanceType = vehicle.AmbulanceType,
                ExistingDocumentPath = vehicle.RegistrationDocumentPath,
                VendorList = await _mediator.Send(new VendorDropDownCommand()),
                VContractList = await _mediator.Send(new VendorContactDropdownCommand (vehicle.VendorId))
            };

            return PartialView("_EditVehicle", dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicle([FromForm] EditVehicleDto dto)
        {
            try
            {
                var command = new UpdateVehicleCommand(dto);
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

                // Rehydrate VendorList
                dto.VendorList = await _mediator.Send(new VendorDropDownCommand());

                return PartialView("_EditVehicle", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContractsByVendor(Guid vendorId)
        {
            var contracts = await _mediator.Send(new VendorContactDropdownCommand(vendorId));
            var result = contracts.Select(c => new
            {
                contractName = c.VContractId
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> AddVehicleDocument()
        {
            var vehicles = await _mediator.Send(new VehicleDropDownCommand()); // returns List<VendorDropdownDto>

            var model = new AddVehicleDocumentDto
            {
                VehicleList = vehicles.Select(v => new VehicleDropdownDto
                {
                    VehicleId = v.VehicleId,
                    VehicleNumber = v.VehicleNumber
                }).ToList()
            };

            return PartialView("_AddVehicleDocument", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleDocument([FromForm] AddVehicleDocumentDto dto)
        {
            try
            {
                var command = new AddVehicleDocumentsCommand(dto);
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

                // Rehydrate VendorList
                dto.VehicleList = await _mediator.Send(new VehicleDropDownCommand());

                return PartialView("_AddVehicleDocument", dto);
            }
        }
        [HttpGet]
        public async Task<IActionResult>VehicleDocument()
        {
            var AllVehicledoc = await _mediator.Send(new FetchVehicleDocumentsCommand());

            return View(AllVehicledoc);
        }

        [HttpGet]
        public async Task<IActionResult> EditVehicleDocuments(Guid DocumentId)
        {
            var vehicle = await _mediator.Send(new FetchVehiclDocumentByIdCommand(DocumentId));
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            var dto = new EditVehicleDocumentDto
            {
                VehicleList = await _mediator.Send(new VehicleDropDownCommand()),
                DocumentId = vehicle.DocumentId,
                OldFileUrl = vehicle.DocumentPath,
                ExpiryDate = vehicle.ExpireOn,
                VehicleId = vehicle.VehicleId,
                DocumentList = Enum.Parse<VehicleDocumentType>(vehicle.DocumentName, true)

            };

            return PartialView("_EditVehicleDocument", dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicleDocument([FromForm] EditVehicleDocumentDto dto)
        {
            try
            {
                var command = new UpdateVehicleDocumentsCommand(dto);
                var id = await _mediator.Send(command);

                return Json(new { success = true });
            }
            catch (ValidationException ex)
            {
                // Populate ModelState manually
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                // Rehydrate VendorList
                return PartialView("_EditVehicleDocument", dto);
            }
        }

        /////////////////////////////////////////Customer Site//////////////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> CustomerSite()
        {
            var AllSite = await _mediator.Send(new FetchCustomerSiteCommand());

            return View(AllSite);
        }
        [HttpGet]
        public async Task<IActionResult> AddCustomerSite()
        {
            var dto = new AddCustomerSiteDto
            {
                CustomerList = await _mediator.Send(new CustomerDropDownCommand()),
                CustomerContract = new List<CustomerContractDropdown>(),
                VehicleList = await _mediator.Send(new VehicleDropDownCommand()),
                EmployeeSelectList = await _mediator.Send(new EmployeeSelectCommand())
            };

            return PartialView("_AddCustomerSite", dto);
            // "_AddCustomerSitePartial.cshtml" should have your Razor form
        }
        [HttpGet]
        public async Task<IActionResult> GetContractsByCustomer(Guid customerId)
        {
            var contracts = await _mediator.Send(new GetCustomerContractDropdownCommand(customerId));
            var result = contracts.Select(c => new
            {
                contractName = c.Ccontractid
            });

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerSite(AddCustomerSiteDto dto)
        {
            try
            {
                var command = new AddCustomerSiteCommand(dto);
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

                // Rehydrate VendorList
              //  dto.VehicleList = await _mediator.Send(new VehicleDropDownCommand());

                return PartialView("_AddCustomerSite", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomerSite(Guid siteId)
        {
            var site = await _mediator.Send(new FetchCustomerSiteByIdCommand(siteId));
            if (site == null)
            {
                return NotFound("Site not found.");
            }
            var dto = new EditCustomerSiteDto
            {
                SiteId = site.SiteId,
                CustomerId = site.CustomerId,
                SiteName = site.SiteName,
                SiteCity = site.SiteCity,
                SiteState = site.SiteState,
                SiteZip = site.SiteZip,
                SiteCountry = site.SiteCountry,
                CustomerContractId = site.CustomerContract, // contractId comes as string in GetCustomerSiteDto
                AssignedVehicleIds = site.AssignedVehicleIds ?? new List<Guid>(),
                AssignedEmployeeIds = site.AssignedEmployeeIds?? new List<Guid>(),

                // Dropdowns
                CustomerList = await _mediator.Send(new CustomerDropDownCommand()),
                CustomerContract = await _mediator.Send(new GetCustomerContractDropdownCommand(site.CustomerId)),
                VehicleList = await _mediator.Send(new VehicleDropDownCommand()),
                EmployeeSelectList = await _mediator.Send(new EmployeeSelectCommand())
            };
            return PartialView("_EditCustomerSite",dto); // ✅ Correct partial name
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerSite(EditCustomerSiteDto dto)
        {
            try
            {
                var command = new UpdateCustomerSiteCommand(dto);
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

                // Rehydrate VendorList
                dto.VehicleList = await _mediator.Send(new VehicleDropDownCommand());

                return PartialView("_EditCustomerSite", dto);
            }
        }
        //------------------SiteExpenses------------------------------------------------------------//
        [HttpPost]
        public async Task<IActionResult> AddSiteExpense([FromForm] AddExpenseDto dto)
        {

            try
            {
                var command = new AddSiteExpenseCommand(dto);
                var id = await _mediator.Send(command);

                return Json(new { success = true, expenseId = id });
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return PartialView("_AddSiteExpense", dto);
            }
            catch (Exception ex)
            {
                // Optional: log the exception for debugging
                // _logger.LogError(ex, "Error while adding site expense");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the expense.");
                return PartialView("_AddSiteExpense", dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatesiteExpense([FromForm] EditExpenseDto dto)
        {
            try
            {
                var command = new UpdateExpenseCommand(dto);
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

                return PartialView("_EditSiteExpense", dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> AddSiteExpense()
        {
            var dto = new AddExpenseDto
            {
                SiteList = await _mediator.Send(new SiteSelectCommand(null)),
                ExecutiveList = await _mediator.Send(new UserSelectCommand(1))
            };

            return PartialView("_AddSiteExpense", dto);
        }

        [HttpGet]
        public async Task<IActionResult> SiteExpense()
        {
            var Allexpenses = await _mediator.Send(new FetchExpenseCommand());

            return View(Allexpenses);
        }

        [HttpGet]
        public async Task<IActionResult> EditSiteExpense(Guid id)
        {
            var expen = await _mediator.Send(new FetchExpenseByIdCommand(id));
            if (expen == null)
            {
                return NotFound("Expense not found.");
            }

            var dto = new EditExpenseDto
            {
                ExpenseId = expen.ExpenseId,
                ExpenseRemark = expen.ExpenseRemark,
                ExpenseDate = expen.ExpenseDate,
                ExpenseAmount = expen.ExpenseAmount,
                ExecutiveId = expen.ExecutiveId,
                ExpenseType = expen.ExpenseType,
                SiteId = expen.SiteId,
                SiteList = await _mediator.Send(new SiteSelectCommand(null)), // Populate dropdown
                ExecutiveList = await _mediator.Send(new UserSelectCommand(1)) // Populate dropdown
            };

            return PartialView("_EditSiteExpense", dto);
        }

        //------------EmployeeMaster--------------------------------------//
        [HttpGet]
        public async Task<IActionResult> Employee()
        {
            var AllEmployee = await _mediator.Send(new FetchEmployeeCommand());

            return View(AllEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromForm] AddEmployeesDto dto)
        {
            try
            {
                var command = new AddEmployeeCommand(dto);
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


                return PartialView("_AddEmployee", dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditEmployeeDetails(Guid EmployeeId)
        {
            var employee = await _mediator.Send(new FetchEmployeeByIdCommand(EmployeeId));
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var dto = new EditEmployeesDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Phone = employee.Phone,
                Email = employee.Email,
                Gender = employee.Gender,
                Address = employee.Address,
                EStatus = employee.EStatus,
                Designation = employee.Designation,
                EmploymentType = employee.EmploymentType,
                GovernmentId = employee.GovernmentId,       
                ProfessionalId = employee.ProfessionalId
            };

            return PartialView("_EditEmployee", dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee([FromForm] EditEmployeesDto dto)
        {
            try
            {
                var command = new UpdateEmployeeCommand(dto);
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

                return PartialView("_EditEmployee", dto);
            }
        }

        /////////////////////// AOE SITE MAPPING //////////////////////////

        [HttpGet]
        public async Task<IActionResult> AddAoeSiteMapping()
        {
            var dto = new AddAoeSiteDto
            {
                SiteList = await _mediator.Send(new SiteSelectCommand(null)),
                ExecutiveList = await _mediator.Send(new UserSelectCommand(1))
            };

            return PartialView("_AddAoeMapping", dto);
        }
        [HttpGet]
        public async Task<IActionResult> AoeSiteMapping()
        {
            var AllSites = await _mediator.Send(new FetchAllAOESiteMappingCommand());

            return View(AllSites);
        }
        [HttpPost]
        public async Task<IActionResult> AddAoeSiteMapping(AddAoeSiteDto dto)
        {
            try
            {
                var command = new AddAOEMappingCommand(dto);
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

                // Rehydrate VendorList
                //  dto.VehicleList = await _mediator.Send(new VehicleDropDownCommand());

                return PartialView("_AddAoeMapping", dto);
            }
        }
/// <summary>
/// ////////////////////TICKET UPDATE///////////////////////////////////////////////////////////
/// </summary>
/// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ServiceTickets()
        {
            var tickets = await _mediator.Send(new FetchAllTicketsRaisedCommand());

            return View(tickets);
        }
        [HttpGet]
        public async Task<IActionResult> EditServiceTicket(Guid ticketId)
        {
            var ticket = await _mediator.Send(new FetchTicketByIdCommand(ticketId));
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }
            var updateDto = new UpdateServiceTicketsDto
            {
                TicketId = ticket.TicketId,
                TicketPriority = ticket.TicketPriority,
                ResolutionNote = ticket.ResolutionNote,
                CurrentStatus = ticket.CurrentStatus,
                ClosedAt = ticket.ClosedAt
            };


            return PartialView("_UpdateTicketStatus", updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTicket(UpdateServiceTicketsDto dto)
        {
            try
            {
                var command = new UpdateTicketsCommand(dto);
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

                return PartialView("_UpdateTicketStatus", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Dashboards()
        {
            var customer = await _mediator.Send(new CustomerContractExpiryCommand());
            var vehicles = await _mediator.Send(new ExpiredVehicleDocumentsCommand());
            var vendor = await _mediator.Send(new VendorContractExpiryCommand());

            var viewModel = new ADashboardViewDto
            {
                VehicleDocuments = vehicles,
                VendorContracts = vendor,
                CustomerContracts = customer
               
            };

            return View(viewModel);
        }

    }


}
