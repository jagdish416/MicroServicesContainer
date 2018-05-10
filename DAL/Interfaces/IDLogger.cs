using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Interfaces
{
    public interface IDLogger
    {
        void LogRequest(string url, string pstrRequestHeader, string pstrRequestBody, Guid requestid);
        void LogResponse(string pstrRequestBody, string requestid, string status);
    }
}