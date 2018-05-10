using System;
using DAL;

namespace LogHandler
{
    public static class Logger
    {
        public static void LogRequest(string url, string pstrHeader, string pstrBody, Guid requestid)
        {
            DLogger lobjDLogger = new DLogger();
            lobjDLogger.LogRequest(url, pstrHeader, pstrBody, requestid);
        }

        public static void LogResponse(string pstrBody, string requestid, string status)
        {
            DLogger lobjDLogger = new DLogger();
            lobjDLogger.LogResponse(pstrBody, requestid, status);
        }
    }
}
