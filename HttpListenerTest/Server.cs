using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Diagnostics;
using System.Threading;

namespace HttpListenerTest
{ 
    public class Server
    {
        private HttpListener _listener;

        public Server()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:7896/");
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        }

        public void Start()
        {
            if (!HttpListener.IsSupported)
                System.Diagnostics.Debug.WriteLine("Not supported");

            _listener.Start();

            while (true)
            {
                ProcessRequest();
            }
        }

        private void ProcessRequest()
        {
            var startNew = Stopwatch.StartNew();
            var result = _listener.BeginGetContext(ListenerCallback, _listener);
            result.AsyncWaitHandle.WaitOne();
            startNew.Stop();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = _listener.EndGetContext(result);
            Thread.Sleep(1000);
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            var receivedText = context.Request.Headers["thread"] + " Received";
            context.Response.Headers["thread"] = receivedText;
            context.Response.Close();
        }
    }
}
