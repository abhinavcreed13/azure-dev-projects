using FooModule.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FooModule.Interfaces
{
    public interface IFooCollection
    {
        public IEnumerable<FooRegistration> GetAllRegistrations();
    }
}
