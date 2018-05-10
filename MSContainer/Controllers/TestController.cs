using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSContainer.Controllers
{
    public class TestController : ApiController
    {
        public string action()
        {
            return "Hi";
        }
    }
}
