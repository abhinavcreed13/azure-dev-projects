using FooModule.Helpers;
using FooModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FooLoginFnApp.Logic
{
    public class FooWork2 : IFooWork
    {
        public Task<FooResult> CheckFooAsync(string param1, string param2, FooContext context, FooToken fooToken)
        {
            return Task.FromResult(new FooResult(param1, param2, FooResultVals.BadResult));
        }
    }
}
