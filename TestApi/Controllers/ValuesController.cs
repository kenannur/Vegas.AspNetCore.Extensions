using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Vegas.AspNetCore.Common.Controllers;

namespace TestApi.Controllers
{
    public class ValuesController : HttpControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetAllAsync()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string GetAsync(int id)
        {
            return $"value {id}";
        }
    }

    public class TestsController : ApiControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetAllAsync()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string GetAsync(int id)
        {
            return $"value {id}";
        }
    }
}
