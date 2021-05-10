using System;
using System.Collections.Generic;
using System.Text;

namespace FooHelpers
{
    public class FooResult
    {
        private readonly string Param1;
        private readonly string Param2;

        public FooResult(string param1, string param2)
        {
            Param1 = param1;
            Param2 = param2;
        }

        public static FooResult GetResult(string param1 = null, string param2 = null, IReadOnlyDictionary<string, object> data = null)
        {
            return new FooResult(param1, param2);
        }
    }
}
