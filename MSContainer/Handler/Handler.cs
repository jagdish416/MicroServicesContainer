using System;
using System.Web;
using MSContainer.BAL;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace MSContainer
{
    public class Handler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                ManageRequest(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ManageRequest(HttpContext context)
        {
            JObject response = null;
            try
            {
                BHandler objBHandler = new BHandler();
                response = objBHandler.ProcessRequest(context);
                byte[] output = Convert.FromBase64String(response.Value<string>("FileContent"));
                context.Response.ContentType = response.Value<string>("ContentType");
                context.Response.AddHeader("FileName", response.Value<string>("FileName"));
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + response.Value<string>("FileName"));
                context.Response.BinaryWrite(output);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
