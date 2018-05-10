using System;
using DAL.Interfaces;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DLogger : IDLogger
    {
        public void LogRequest(string url, string pstrRequestHeader, string pstrRequestBody, Guid requestID)
        {
            SqlParameter[] objParams = new SqlParameter[4];

            objParams[0] = new SqlParameter("@V_RequestID", SqlDbType.VarChar);
            objParams[0].Value = requestID.ToString();

            objParams[1] = new SqlParameter("@V_RequestURL", SqlDbType.VarChar);
            objParams[1].Value = url;

            objParams[2] = new SqlParameter("@V_RequestHeader", SqlDbType.VarChar);
            objParams[2].Value = pstrRequestHeader;

            objParams[3] = new SqlParameter("@V_RequestBody", SqlDbType.VarChar);
            objParams[3].Value = pstrRequestBody;

            SqlHelper.ExecuteNonQuery(System.Configuration.ConfigurationManager.ConnectionStrings["msconn"].ToString(), CommandType.StoredProcedure, "USP_AddRequest", objParams);
        }

        public void LogResponse(string pstrRequestBody, string requestid, string status)
        {
            SqlParameter[] objParams = new SqlParameter[3];

            objParams[0] = new SqlParameter("@V_RequestID", SqlDbType.VarChar);
            objParams[0].Value = requestid.ToString();

            objParams[1] = new SqlParameter("@V_Response", SqlDbType.VarChar);
            objParams[1].Value = pstrRequestBody;

            objParams[2] = new SqlParameter("@V_ResponseCode", SqlDbType.VarChar);
            objParams[2].Value = status;

            SqlHelper.ExecuteNonQuery(System.Configuration.ConfigurationManager.ConnectionStrings["msconn"].ToString(), CommandType.StoredProcedure, "USP_AddResponse", objParams);

            //throw new NotImplementedException();
        }
    }
}