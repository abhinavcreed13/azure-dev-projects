using FooModule.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FooModule.Interfaces
{
    public interface IFooLogicImplementer
    {
        Task<FinalResult> ExecuteLogic();
    }
}
