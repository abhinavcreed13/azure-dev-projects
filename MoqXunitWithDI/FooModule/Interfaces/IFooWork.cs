using FooModule.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FooModule.Interfaces
{
    public interface IFooWork
    {
        Task<FooResult> CheckFooAsync(string param1, string param2, FooContext context, FooToken fooToken);
    }
}
