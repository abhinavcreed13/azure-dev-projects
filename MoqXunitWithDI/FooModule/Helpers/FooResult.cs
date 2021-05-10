using System;
using System.Collections.Generic;
using System.Text;

namespace FooModule.Helpers
{
    public enum FooResultVals
    {
        GoodResult = 1,
        BadResult = 2
    }

    public class FooResult
    {
        public string Param1;
        public string Param2;
        public FooResultVals Val;

        public FooResult(string param1, string param2, FooResultVals val)
        {
            Param1 = param1;
            Param2 = param2;
            Val = val;
        }
    }
}
