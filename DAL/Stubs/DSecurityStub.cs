using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Interfaces;

namespace DAL.Stubs
{
    public class DSecurityStub : IDSecurity
    {
        public bool AuthenticateClient(string pstrAPIKey, string pstrReferrer, string pstrController, string pstrAction)
        {
            if (pstrAPIKey == "1234")
                return true;
            else
                return false;
        }
    }
}