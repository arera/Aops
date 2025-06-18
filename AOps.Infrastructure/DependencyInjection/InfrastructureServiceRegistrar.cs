using AOps.Infrastructure.Persistence;
using AOps.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.RegisterCustomers;
using AOps.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AOps.Application.DTOs;
using AOps.Application.UseCases.LoginUsers;
using AOps.Application.UseCases.RegisterOrglevels;
using AOps.Application;

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

            // Infrastructure
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
           // services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<IOrgLevelRepository, OrgLevelRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
           // services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);


            //Handlers
           // services.AddTransient<IRequestHandler<LoginCommand, LoginResponseDto>, LoginCommandHandler>();

            // Validation
            services.AddTransient<IValidator<RegisterCustomerCommand>, RegisterCustomerValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

    }
}
