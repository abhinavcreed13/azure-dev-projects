using FooModule.Helpers;
using FooModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FooModule
{
    public class FooLogicImplementer: IFooLogicImplementer
    {
        private readonly List<FooRegistration> _foos = new List<FooRegistration>();

        public FooLogicImplementer(IEnumerable<IFooCollection> data)
        {
            foreach(var foo in data)
            {
                _foos.AddRange(foo.GetAllRegistrations());
            }
        }

        public async Task<FinalResult> ExecuteLogic()
        {
            List<FooResult> data = new List<FooResult>();
            foreach(var _foo in _foos)
            {
                data.Add(await _foo.Exec());
            }
            var isAllGood = true;
            foreach(var result in data)
            {
                if(result.Val == FooResultVals.BadResult)
                {
                    isAllGood = false;
                }
            }
            if (isAllGood)
            {
                return new FinalResult
                {
                    FinalRes = "Good",
                    Data = data
                };
            }
            else
            {
                return new FinalResult
                {
                    FinalRes = "Bad",
                    Data = data
                };
            }
        }
    }
}
