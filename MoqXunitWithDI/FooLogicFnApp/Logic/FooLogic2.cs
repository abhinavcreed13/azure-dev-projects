using FooModule.Helpers;
using FooModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FooLoginFnApp.Logic
{
    public class FooLogic2: IFooCollection
    {
        private readonly IFooWork _fooWork;

        public FooLogic2(IFooWork fooWork)
        {
            _fooWork = fooWork;
        }

        public IEnumerable<FooRegistration> GetAllRegistrations()
        {
            return new List<FooRegistration>
            {
                new FooRegistration("logic_reg_2_1", _fooWork, new FooToken(), new List<string> {"tag1", "tag2"}),
                new FooRegistration("logic_reg_2_2", _fooWork, new FooToken(), new List<string> {"tag1", "tag2"})
            };
        }
    }
}
