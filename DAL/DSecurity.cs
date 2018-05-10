using DAL.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace DAL
{
    public class DSecurity : IDSecurity
    {
        public bool AuthenticateClient(string pstrAPIKey, string pstrReferrer, string pstrController, string pstrAction)
        {
            SqlParameter[] objParams = new SqlParameter[5];

            objParams[0] = new SqlParameter("@V_APIKEY",SqlDbType.VarChar);
            objParams[0].Value = pstrAPIKey;

            objParams[1] = new SqlParameter("@V_ClientHost", SqlDbType.VarChar);
            objParams[1].Value = pstrReferrer;

            objParams[2] = new SqlParameter("@V_Controller", SqlDbType.VarChar);
            objParams[2].Value = pstrController;

            objParams[3] = new SqlParameter("@V_Action", SqlDbType.VarChar);
            objParams[3].Value = pstrAction;

            objParams[4] = new SqlParameter("@B_Result", SqlDbType.Bit);
            objParams[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(System.Configuration.ConfigurationManager.ConnectionStrings["msconn"].ToString(), CommandType.StoredProcedure, "USP_AuthenticateClient", objParams);
            return bool.Parse(objParams[4].Value.ToString());
        }
    }
}