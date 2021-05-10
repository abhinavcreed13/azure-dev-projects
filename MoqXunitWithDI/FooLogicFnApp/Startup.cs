using FooLoginFnApp.Logic;
using FooModule;
using FooModule.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(FooLoginFnApp.Startup))]
namespace FooLoginFnApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // inject dependencies
            builder.Services.AddScoped<IFooCollection, FooLogic1>(sp =>
            {
                return new FooLogic1(new FooWork1());
            });
            builder.Services.AddScoped<IFooCollection, FooLogic2>(sp =>
            {
                return new FooLogic2(new FooWork2());
            });

            // inject implementer
            builder.Services.AddScoped<IFooLogicImplementer, FooLogicImplementer>();
        }
    }
}
