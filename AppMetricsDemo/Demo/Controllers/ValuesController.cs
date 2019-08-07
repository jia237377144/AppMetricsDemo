using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet("GetValue")]
        public IEnumerable<string> GetValue()
        {
            //await Delay();
            return new string[] { "value1", "value2" };
        }
        [HttpGet("GetTest")]
        public string GetTest()
        {
            //await Delay();
            return "value1";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private Task Delay()
        {
            var second = DateTime.Now.Second;

            if (second <= 20)
            {
                return Task.CompletedTask;
            }

            if (second <= 40)
            {
                return Task.Delay(TimeSpan.FromMilliseconds(50), HttpContext.RequestAborted);
            }

            return Task.Delay(TimeSpan.FromMilliseconds(100), HttpContext.RequestAborted);
        }
    }
}
