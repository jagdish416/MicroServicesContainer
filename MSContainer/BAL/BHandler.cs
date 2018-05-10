using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MSContainer.DAL;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MSContainer.BAL
{
    public class BHandler
    {
        public DataTable GetApiDetails(int Apiid)
        {
            try
            {
                DHandler objDHandler = new DHandler();
                DataTable dt = objDHandler.GetApiDetails(Apiid);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public JObject ProcessRequest(HttpContext context)
        {
            JObject response = null;
            var searchParams = context.Request.QueryString;
            if (searchParams.Count > 0)
            {
                if (!string.IsNullOrEmpty(searchParams["apiid"]))
                {
                    var value = searchParams["apiid"];
                    int apiid = 0;
                    if (int.TryParse(value, out apiid))
                    {
                        DataTable dt = GetApiDetails(apiid);
                        if (dt.Rows.Count > 0)
                        {
                            response = GetResponse(searchParams["apikey"], dt.Rows[0]["Controller"].ToString(), dt.Rows[0]["Action"].ToString(), ExtractParams(searchParams));
                        }
                        else
                        {
                            throw new Exception("APIID cannot be null or Invalid APIID!!!");
                        }
                    }
                }
            }
            return response;
        }
        public JObject GetResponse(string apikey, string ControllerName, string ActionName, string Paramerters)
        {
            string URL = string.Empty;
            using (WebClient client = new WebClient())
            {
                string SUrl = ConfigurationManager.AppSettings["URL"].ToString();
                if (string.IsNullOrEmpty(Paramerters))
                    URL = SUrl + ControllerName + "/" + ActionName;
                else
                    URL = SUrl + ControllerName + "/" + ActionName + "?" + Paramerters;
                client.Headers["Content-Type"] = "application/Json";
                client.Headers["APIKEY"] = apikey;
                byte[] output = client.DownloadData(URL);
                string response = client.Encoding.GetString(output);
                JObject obj = JObject.Parse(response);
                return obj;
            }
        }
        public string ExtractParams(NameValueCollection strParams)
        {
            List<string> arrParam = new List<string>();
            foreach (string key in strParams)
            {
                if (key.ToLower() != "apiid" && key.ToLower() != "apikey")
                    arrParam.Add(string.Concat(key, "=", strParams[key]));
            }
            return string.Join("&", arrParam);
        }
    }
}