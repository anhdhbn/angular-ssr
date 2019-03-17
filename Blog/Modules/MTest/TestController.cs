using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Modules.MTest
{
    [Route("api/test")]
    public class TestController : Controller
    {
        [Route(""), HttpGet]
        public string Test()
        {
            return "OK!";
        }
    }
}
