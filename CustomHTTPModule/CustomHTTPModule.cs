using System;
using System.IO;
using System.Text;
using System.Web;
using LogHandler;
using System.Runtime.Serialization.Formatters.Binary;

namespace CustomHTTPModule
{
    public class CustomHTTPModule : IHttpModule
    {
        //private const string Realm = "My Realm";
        public static string RequestID;
        public HttpApplication _context;
        private StreamWatcher _watcher;
        public void Init(HttpApplication context)
        {
            // Register event handlers
            _context = context;
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
            //context.RequestCompleted += Context_RequestCompleted;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            _watcher = new StreamWatcher(_context.Response.Filter);
            _context.Response.Filter = _watcher;
            
            StringBuilder lstrHeader = new StringBuilder();
            foreach (string h in _context.Request.Headers.AllKeys)
            {
                lstrHeader.AppendLine(h + ":" + _context.Request.Headers.GetValues(h)[0]);
            }
            Guid lstrRequestID = Guid.NewGuid();
            RequestID = lstrRequestID.ToString();
            _context.Response.Headers.Add("RequestID", RequestID);
            string lstrBody = GetRequestBody(_context.Request);
            System.Threading.ThreadPool.QueueUserWorkItem(_ => Logger.LogRequest(_context.Request.Url.ToString(), lstrHeader.ToString(), lstrBody, lstrRequestID));
        }
        private string GetRequestBody(HttpRequest Request)
        {
            string documentContents;
            Stream receiveStream = Request.GetBufferedInputStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            documentContents = readStream.ReadToEnd();
            Request.InputStream.Position = 0;
            return documentContents;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(_ => Logger.LogResponse(_watcher.ToString(), RequestID, _context.Context != null ? _context.Context.Response.Status : ""));
        }

        public void Dispose()
        {
        }
    }
}