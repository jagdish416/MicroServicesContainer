using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Interfaces;

namespace DAL.Stubs
{
    public class DLoggerStub : IDLogger
    {
        public void LogRequest(string url, string pstrRequestHeader, string pstrRequestBody, Guid requestID)
        {

        }

        public void LogResponse(string pstrRequestBody, string requestid, string status)
        {
            //throw new NotImplementedException();
        }
    }
}