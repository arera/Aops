using AOps.Application.Interfaces;
using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.Validators;
using AOps.Infrastructure.Persistence;
using AOps.Infrastructure.Security;
using AOps.Application.Common.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices.Marshalling;
using AOps.Domain.Entities;


namespace AOps.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistrar
    {
        public static IServiceCollection RegisterInfra(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AOpsDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("AOps.Infrastructure")));
            // Register health checks here
            // Add EF Core DBContext health check
            services.AddHealthChecks().AddDbContextCheck<AOpsDbContext>(name: "SQL Server");
            services.AddHttpContextAccessor();

            services.Configure<BaseUrls>(config.GetSection("BaseUrls"));
            var baseUrls = config.GetSection("BaseUrls").Get<BaseUrls>();
            if (baseUrls == null || string.IsNullOrWhiteSpace(baseUrls.SiteUrl))
                throw new InvalidOperationException("BaseUrls.SiteUrl is not configured.");
            services.AddSingleton(baseUrls.SiteUrl); 



            // Infrastructure
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IOrgLevelRepository, OrgLevelRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorContractRepository, VendorContractRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleDocumentRepository, VehicleDocumentRepository>();
            services.AddScoped<ICustomerContractRepository,CustomerContractRepository>();
            services.AddScoped<ICustomerSiteRepository, CustomerSiteRepository>();
            services.AddScoped<ISiteVehicleAssignmentRepository, SiteVehicleAssignmentRepository>();
            services.AddScoped<ICustomerLoginRepository, CustomerLoginRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<ICustomerTicketRepository, CustomerTicketRepository>();
            services.AddScoped<ISiteExpenseRepository, SiteExpensesRepository>();
            services.AddScoped<IEmployeeMasterRepository, EmployeeMasterRepository>();
            services.AddScoped<ISiteEmployeeAssignmentRepository,SiteEmployeeAssignmentRepository>();
            services.AddScoped<IAOESiteMappingRepository, AOESiteMappingRepository>();
            services.AddScoped<IAdminCustomerTicket, AdminCustomerTicketRepository>();
            services.AddScoped<ICustomerDashboardsRepository,CustomerDashboardRepository>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();


            //Handlers
            // services.AddTransient<IRequestHandler<LoginCommand, LoginResponseDto>, LoginCommandHandler>();

            // Validation
            services.AddTransient<IValidator<RegisterCustomerCommand>, RegisterCustomerValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

    }
}
