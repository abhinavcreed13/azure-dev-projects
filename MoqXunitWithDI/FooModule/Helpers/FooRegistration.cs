using FooModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FooModule.Helpers
{
    public class FooRegistration
    {
        private readonly string _param1;
        private readonly IFooWork _instance;
        private readonly FooToken _token;
        public FooRegistration(string param1, IFooWork instance, FooToken token, IEnumerable<string> tags)
        {
            // some registration logic
            _param1 = param1;
            _instance = instance;
            _token = token;
        }

        public async Task<FooResult> Exec()
        {
            var res = await _instance.CheckFooAsync(_param1, null, new FooContext(), _token);
            return res;
        }
    }
}
