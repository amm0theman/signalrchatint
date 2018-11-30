﻿using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(ServerSignalR.Startup))]

namespace ServerSignalR
{
    class ServerSignalR
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.Properties["host.AppMode"] = "development";
            app.UseErrorPage(new Microsoft.Owin.Diagnostics.ErrorPageOptions { ShowExceptionDetails = true });
            var hubConfiguration = new HubConfiguration
            {
                EnableJSONP = true,
                EnableDetailedErrors = true
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}