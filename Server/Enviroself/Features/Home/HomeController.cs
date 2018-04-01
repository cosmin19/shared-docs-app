using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Features.Home
{
    public class HomeController
    {
        [HttpGet]
        public string Get()
        {
            return "Basic web api is working";
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Basic web api is working for id = " + id;
        }
    }
}
