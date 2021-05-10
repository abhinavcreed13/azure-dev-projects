using System.IO;
using System.Threading.Tasks;
using FooModule;
using FooModule.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FooLoginFnApp
{
    public class LogicFunc
    {
        private readonly IFooLogicImplementer _implementer;
        
        public LogicFunc(IFooLogicImplementer implementer)
        {
            _implementer = implementer;
        }

        [FunctionName("LogicFuncTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var output = _implementer.ExecuteLogic();

            return new OkObjectResult(output);
        }
    }
}

