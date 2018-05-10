using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MSContainer.DAL
{
    public class DHandler
    {
        string SP = string.Empty;
        string Conn_Database = ConfigurationManager.ConnectionStrings["msconn"].ToString();
        public DataTable GetApiDetails(int Apiid)
        {
            try
            {
                SqlConnection Con = new SqlConnection(Conn_Database);
                Con.Open();
                DataTable APIDetails = new DataTable();
                string query = "SELECT APIName,Controller,Action  FROM TBLAPI WHERE API_ID=@Apiid AND STATUS=1";
                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@Apiid", Apiid));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(APIDetails);
                }
                Con.Close();
                return APIDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}