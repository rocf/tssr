using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SX.TSSR.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
