using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application
{
    /// <summary>
    /// This is a marker class used to reference the Application assembly.
    /// It is commonly used for assembly scanning operations such as:
    /// - Registering MediatR handlers
    /// - Registering AutoMapper profiles
    /// - Registering FluentValidation validators
    /// 
    /// Example usage:
    /// services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));
    /// 
    /// This approach avoids hardcoding assembly names and keeps registration code clean and maintainable.
    /// This is registered in program.cs 
    /// </summary>
    public sealed class ApplicationAssemblyMarker { }
}
